using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using Bifrost.Editor;
using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bifrost.Editor.AI.Providers
{
    // Helper classes for Newtonsoft JSON serialization
    public class OpenRouterMessage
    {
        [JsonProperty("role")]
        public string Role { get; set; }

        [JsonProperty("content")]
        public string Content { get; set; }
    }

    public class OpenRouterChoice
    {
        [JsonProperty("message")]
        public OpenRouterMessage Message { get; set; }
    }

    public class OpenRouterResponse
    {
        [JsonProperty("choices")]
        public List<OpenRouterChoice> Choices { get; set; }
    }

    public class OpenRouterRequestBody
    {
        [JsonProperty("model")]
        public string Model { get; set; }

        [JsonProperty("messages")]
        public List<OpenRouterMessage> Messages { get; set; }

        [JsonProperty("max_tokens", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxTokens { get; set; }

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public float? Temperature { get; set; }

        [JsonProperty("top_p", NullValueHandling = NullValueHandling.Ignore)]
        public float? TopP { get; set; }

        [JsonProperty("frequency_penalty", NullValueHandling = NullValueHandling.Ignore)]
        public float? FrequencyPenalty { get; set; }

        [JsonProperty("presence_penalty", NullValueHandling = NullValueHandling.Ignore)]
        public float? PresencePenalty { get; set; }

        // Add other OpenRouter supported fields if needed, e.g.:
        // [JsonProperty("stream", NullValueHandling = NullValueHandling.Ignore)]
        // public bool? Stream { get; set; }

        // [JsonProperty("stop", NullValueHandling = NullValueHandling.Ignore)]
        // public object Stop { get; set; } // Can be string or array of strings
    }

    public class OpenRouterProvider : IBifrostLLMProvider
    {
        public string Name => "OpenRouter";

        public async Task<string> CompleteAsync(string promptText, string modelName, string apiKey, string endpointUrl, LLMRequestOptions options)
        {
            var requestMessages = new List<OpenRouterMessage> {
                new OpenRouterMessage { Role = "user", Content = promptText }
            };

            var requestBody = new OpenRouterRequestBody
            {
                Model = modelName,
                Messages = requestMessages
            };

            if (options != null)
            {
                requestBody.MaxTokens = options.maxTokens;
                requestBody.Temperature = options.temperature;
                requestBody.TopP = options.topP;
                requestBody.FrequencyPenalty = options.frequencyPenalty;
                requestBody.PresencePenalty = options.presencePenalty;
                // Note: timeoutSeconds from LLMRequestOptions is for the UnityWebRequest, not typically part of OpenRouter JSON body.
            }

            string jsonPayload = JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore // Omit fields if their value is null
            });

            Debug.Log($"OpenRouterProvider - Sending request to {modelName}");

            using (UnityWebRequest req = new UnityWebRequest(endpointUrl, "POST"))
            {
                req.timeout = options?.timeoutSeconds ?? 60; // Use timeout from options
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
                req.uploadHandler = new UploadHandlerRaw(bodyRaw);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                req.SetRequestHeader("Authorization", $"Bearer {apiKey}");

                // Add custom headers from LLMRequestOptions
                if (options != null && options.customHeaders != null)
                {
                    foreach (var header in options.customHeaders)
                    {
                        if (!string.IsNullOrEmpty(header.key))
                            req.SetRequestHeader(header.key, header.value);
                    }
                }
                // Optional but recommended OpenRouter headers:
                // req.SetRequestHeader("HTTP-Referer", "YOUR_APP_URL_OR_PROJECT_NAME"); 
                // req.SetRequestHeader("X-Title", "YOUR_APP_OR_PROJECT_NAME");

                var op = req.SendWebRequest();
                while (!op.isDone)
                    await Task.Yield();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"OpenRouterProvider Error: {req.error} - Status Code: {req.responseCode}");
                    if (req.downloadHandler != null && !string.IsNullOrEmpty(req.downloadHandler.text))
                    {
                        Debug.LogError($"OpenRouterProvider - Server Response Body: {req.downloadHandler.text}");
                    }
                    else
                    {
                        Debug.LogError("OpenRouterProvider - No response body from server.");
                    }
                    return null;
                }

                string responseText = req.downloadHandler.text;

                try
                {
                    // Try to parse and extract just the message content
                    var responseObj = JsonConvert.DeserializeObject<OpenRouterResponse>(responseText);
                    if (responseObj?.Choices != null && responseObj.Choices.Count > 0 &&
                        responseObj.Choices[0].Message != null &&
                        !string.IsNullOrEmpty(responseObj.Choices[0].Message.Content))
                    {
                        // Return just the message content for easier processing
                        return responseObj.Choices[0].Message.Content;
                    }

                    // If the structured parse failed, try with JObject for more flexibility
                    var jObj = JObject.Parse(responseText);
                    if (jObj["choices"] is JArray choices && choices.Count > 0)
                    {
                        if (choices[0]["message"] is JObject message && message["content"] != null)
                        {
                            return message["content"].ToString();
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.LogWarning($"Failed to parse OpenRouter response as structured object: {ex.Message}. Using raw response.");
                    // If parsing fails, just return the raw response
                }

                return responseText;
            }
        }

        public async Task<bool> TestConnectionAsync(string apiKey, string endpoint, string model)
        {
            string testPrompt = "Say hello.";
            // Create default options for the test call.
            // The timeout for the test call will be the default in UnityWebRequest or the one set in CompleteAsync's req.timeout.
            var testOptions = new LLMRequestOptions();
            string result = await CompleteAsync(testPrompt, model, apiKey, endpoint, testOptions);
            return !string.IsNullOrEmpty(result);
        }
    }
}