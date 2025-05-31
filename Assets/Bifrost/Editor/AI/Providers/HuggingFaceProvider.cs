using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using Bifrost.Editor; // For LLMRequestOptions
using System.Collections.Generic; // For List/Dictionary
using Newtonsoft.Json; // USING NEWTONSOFT

namespace Bifrost.Editor.AI.Providers
{
    // Helper classes for HuggingFace
    [System.Serializable]
    public class HuggingFaceParameters
    {
        [JsonProperty("max_new_tokens", NullValueHandling = NullValueHandling.Ignore)]
        public int? MaxNewTokens { get; set; }

        [JsonProperty("temperature", NullValueHandling = NullValueHandling.Ignore)]
        public float? Temperature { get; set; }

        // Add other common HuggingFace parameters if needed, e.g., top_p, top_k
        // [JsonProperty("top_p", NullValueHandling = NullValueHandling.Ignore)]
        // public float? TopP { get; set; }
    }

    [System.Serializable]
    public class HuggingFaceRequestBody
    {
        [JsonProperty("inputs")]
        public string Inputs { get; set; }

        [JsonProperty("parameters", NullValueHandling = NullValueHandling.Ignore)]
        public HuggingFaceParameters Parameters { get; set; }

        // Some models might expect an "options" field for things like "wait_for_model"
        // [JsonProperty("options", NullValueHandling = NullValueHandling.Ignore)]
        // public object Options { get; set; }
    }

    public class HuggingFaceProvider : IBifrostLLMProvider
    {
        public string Name => "HuggingFace";

        public async Task<string> CompleteAsync(string promptText, string modelIdOrEndpoint, string apiKey, string endpointUrlToUse, LLMRequestOptions options)
        {
            // For HuggingFace, 'modelIdOrEndpoint' might be just the model ID if using the generic inference API endpoint,
            // or it could be a full custom inference endpoint URL.
            // 'endpointUrlToUse' would typically be the base HuggingFace API URL if not a custom endpoint.
            // We'll assume endpointUrlToUse is the actual full URL to POST to.

            var requestBody = new HuggingFaceRequestBody
            {
                Inputs = promptText,
                Parameters = new HuggingFaceParameters()
            };

            if (options != null)
            {
                requestBody.Parameters.MaxNewTokens = options.maxTokens;
                requestBody.Parameters.Temperature = options.temperature;
                // requestBody.Parameters.TopP = options.topP; // If you add TopP to HuggingFaceParameters & LLMRequestOptions
            }

            string jsonPayload = JsonConvert.SerializeObject(requestBody, new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            });

            Debug.LogWarning($"HuggingFaceProvider - JSON PAYLOAD BEING SENT (Newtonsoft): {jsonPayload}");

            // If modelIdOrEndpoint is a full URL, use it directly. Otherwise, construct from endpointUrlToUse and modelIdOrEndpoint.
            // For simplicity, we'll assume endpointUrlToUse is the complete URL for the request.
            // If you need to append model_id to a base URL, that logic would go here.

            using (UnityWebRequest req = new UnityWebRequest(endpointUrlToUse, "POST"))
            {
                req.timeout = options?.timeoutSeconds ?? 60;
                byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonPayload);
                req.uploadHandler = new UploadHandlerRaw(bodyRaw);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                if (!string.IsNullOrEmpty(apiKey)) // API key is often optional for public models, but required for private/rate limits
                {
                    req.SetRequestHeader("Authorization", $"Bearer {apiKey}");
                }

                if (options != null && options.customHeaders != null)
                {
                    foreach (var header in options.customHeaders)
                    {
                        if (!string.IsNullOrEmpty(header.key))
                            req.SetRequestHeader(header.key, header.value);
                    }
                }

                var op = req.SendWebRequest();
                while (!op.isDone)
                    await Task.Yield();

                if (req.result != UnityWebRequest.Result.Success)
                {
                    Debug.LogError($"HuggingFaceProvider Error: {req.error} - Status Code: {req.responseCode}");
                    if (req.downloadHandler != null && !string.IsNullOrEmpty(req.downloadHandler.text))
                    {
                        Debug.LogError($"HuggingFaceProvider - Server Response Body: {req.downloadHandler.text}");
                    }
                    else
                    {
                        Debug.LogError("HuggingFaceProvider - No response body from server.");
                    }
                    return null;
                }
                // HuggingFace responses can be complex (e.g., a list with a dictionary containing "generated_text")
                // This part might need custom parsing based on expected model output.
                // For now, returning the raw text. Consider a helper to extract "generated_text".
                return req.downloadHandler.text;
            }
        }

        public async Task<bool> TestConnectionAsync(string apiKey, string endpoint, string modelId) // modelId for HuggingFace
        {
            string testPrompt = "Hello!";
            // Assuming 'endpoint' is the full URL for the test, or a base URL that 'CompleteAsync' can use with modelId
            var testOptions = new LLMRequestOptions { maxTokens = 10 }; // Keep test payload small
            string result = await CompleteAsync(testPrompt, modelId, apiKey, endpoint, testOptions);

            // A more robust test might try to deserialize the result and check for a specific field
            // like "generated_text", but for now, a non-empty response is a basic pass.
            return !string.IsNullOrEmpty(result);
        }
    }
}