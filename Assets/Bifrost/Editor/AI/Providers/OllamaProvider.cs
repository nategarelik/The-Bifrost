using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;
using System.Linq;

namespace Bifrost.Editor.AI.Providers
{
    [Serializable]
    public class OllamaProvider : IBifrostLLMProvider
    {
        private static readonly HttpClient httpClient = new HttpClient { Timeout = TimeSpan.FromMinutes(5) };
        private const string DEFAULT_ENDPOINT = "http://localhost:11434";

        public string DisplayName => "Ollama (Local)";
        public bool RequiresAPIKey => false;
        public bool SupportsStreaming => true;
        public bool SupportsToolCalling => true; // Some models support function calling

        public string DefaultModel => "llama3.2";
        public string ModelPlaceholder => "llama3.2, mistral, codellama, etc.";
        public string ModelTooltip => "Enter the name of your locally installed Ollama model";
        public string EndpointTooltip => "Ollama API endpoint (default: http://localhost:11434)";

        public async Task<string> CompleteAsync(string prompt, string model, string apiKey, string endpoint, LLMRequestOptions options)
        {
            try
            {
                endpoint = string.IsNullOrEmpty(endpoint) ? DEFAULT_ENDPOINT : endpoint;
                model = string.IsNullOrEmpty(model) ? DefaultModel : model;

                var requestBody = new
                {
                    model = model,
                    prompt = prompt,
                    stream = false,
                    options = new
                    {
                        temperature = options?.Temperature ?? 0.7f,
                        top_p = options?.TopP ?? 1.0f,
                        num_predict = options?.MaxTokens ?? 2048,
                        stop = options?.StopSequences
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, $"{endpoint}/api/generate")
                {
                    Content = content
                };

                // Add custom headers if provided
                if (options?.CustomHeaders != null)
                {
                    foreach (var header in options.CustomHeaders)
                    {
                        request.Headers.TryAddWithoutValidation(header.Key, header.Value);
                    }
                }

                var response = await httpClient.SendAsync(request);
                var responseContent = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    Debug.LogError($"Ollama API error: {response.StatusCode} - {responseContent}");
                    throw new Exception($"Ollama API error: {response.StatusCode}");
                }

                var responseJson = JObject.Parse(responseContent);
                return responseJson["response"]?.ToString() ?? "";
            }
            catch (HttpRequestException httpEx)
            {
                Debug.LogError($"Ollama HTTP error: {httpEx.Message}");
                throw new Exception($"Failed to connect to Ollama. Make sure Ollama is running at {endpoint}");
            }
            catch (TaskCanceledException)
            {
                throw new Exception("Ollama request timed out. Try increasing the timeout or using a smaller model.");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ollama completion error: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> TestConnectionAsync(string apiKey, string endpoint, string model)
        {
            try
            {
                endpoint = string.IsNullOrEmpty(endpoint) ? DEFAULT_ENDPOINT : endpoint;
                
                // Test if Ollama is running
                var response = await httpClient.GetAsync($"{endpoint}/api/tags");
                if (!response.IsSuccessStatusCode)
                {
                    return false;
                }

                // Check if the specified model exists
                if (!string.IsNullOrEmpty(model))
                {
                    var content = await response.Content.ReadAsStringAsync();
                    var tags = JObject.Parse(content);
                    var models = tags["models"] as JArray;
                    
                    if (models != null)
                    {
                        var modelExists = models.Any(m => m["name"]?.ToString() == model);
                        if (!modelExists)
                        {
                            Debug.LogWarning($"Model '{model}' not found in Ollama. Available models: {string.Join(", ", models.Select(m => m["name"]))}");
                        }
                    }
                }

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ollama connection test failed: {ex.Message}");
                return false;
            }
        }

        public async Task<List<string>> GetAvailableModels(string endpoint = null)
        {
            try
            {
                endpoint = string.IsNullOrEmpty(endpoint) ? DEFAULT_ENDPOINT : endpoint;
                
                var response = await httpClient.GetAsync($"{endpoint}/api/tags");
                if (!response.IsSuccessStatusCode)
                {
                    return new List<string> { DefaultModel };
                }

                var content = await response.Content.ReadAsStringAsync();
                var tags = JObject.Parse(content);
                var models = tags["models"] as JArray;
                
                if (models != null)
                {
                    return models.Select(m => m["name"]?.ToString()).Where(n => !string.IsNullOrEmpty(n)).ToList();
                }

                return new List<string> { DefaultModel };
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to get Ollama models: {ex.Message}");
                return new List<string> { DefaultModel };
            }
        }

        public async Task<string> StreamCompleteAsync(string prompt, string model, string apiKey, string endpoint, LLMRequestOptions options, Action<string> onToken)
        {
            try
            {
                endpoint = string.IsNullOrEmpty(endpoint) ? DEFAULT_ENDPOINT : endpoint;
                model = string.IsNullOrEmpty(model) ? DefaultModel : model;

                var requestBody = new
                {
                    model = model,
                    prompt = prompt,
                    stream = true,
                    options = new
                    {
                        temperature = options?.Temperature ?? 0.7f,
                        top_p = options?.TopP ?? 1.0f,
                        num_predict = options?.MaxTokens ?? 2048,
                        stop = options?.StopSequences
                    }
                };

                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var request = new HttpRequestMessage(HttpMethod.Post, $"{endpoint}/api/generate")
                {
                    Content = content
                };

                using (var response = await httpClient.SendAsync(request, HttpCompletionOption.ResponseHeadersRead))
                {
                    if (!response.IsSuccessStatusCode)
                    {
                        var errorContent = await response.Content.ReadAsStringAsync();
                        throw new Exception($"Ollama API error: {response.StatusCode} - {errorContent}");
                    }

                    using (var stream = await response.Content.ReadAsStreamAsync())
                    using (var reader = new StreamReader(stream))
                    {
                        var fullResponse = new StringBuilder();
                        string line;
                        
                        while ((line = await reader.ReadLineAsync()) != null)
                        {
                            if (string.IsNullOrWhiteSpace(line)) continue;
                            
                            try
                            {
                                var chunk = JObject.Parse(line);
                                var token = chunk["response"]?.ToString();
                                
                                if (!string.IsNullOrEmpty(token))
                                {
                                    fullResponse.Append(token);
                                    onToken?.Invoke(token);
                                }
                                
                                if (chunk["done"]?.ToObject<bool>() == true)
                                {
                                    break;
                                }
                            }
                            catch (JsonException)
                            {
                                // Skip invalid JSON lines
                            }
                        }
                        
                        return fullResponse.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ollama streaming error: {ex.Message}");
                throw;
            }
        }

        public async Task<string> CompleteWithToolsAsync(string prompt, string model, List<JObject> tools, string apiKey, string endpoint, LLMRequestOptions options)
        {
            // Some Ollama models support function calling (e.g., certain versions of Llama)
            // For models that don't support it natively, we can use prompt engineering
            
            try
            {
                endpoint = string.IsNullOrEmpty(endpoint) ? DEFAULT_ENDPOINT : endpoint;
                model = string.IsNullOrEmpty(model) ? DefaultModel : model;

                // Check if model supports function calling
                var supportsNativeFunctions = model.Contains("llama") || model.Contains("mistral");
                
                if (supportsNativeFunctions)
                {
                    // Use native function calling if supported
                    var requestBody = new
                    {
                        model = model,
                        prompt = prompt,
                        stream = false,
                        tools = tools.Select(t => new
                        {
                            type = "function",
                            function = new
                            {
                                name = t["name"]?.ToString(),
                                description = t["description"]?.ToString(),
                                parameters = t["inputSchema"]
                            }
                        }).ToList(),
                        options = new
                        {
                            temperature = options?.Temperature ?? 0.7f,
                            top_p = options?.TopP ?? 1.0f,
                            num_predict = options?.MaxTokens ?? 2048
                        }
                    };

                    var json = JsonConvert.SerializeObject(requestBody);
                    var content = new StringContent(json, Encoding.UTF8, "application/json");

                    var response = await httpClient.PostAsync($"{endpoint}/api/generate", content);
                    var responseContent = await response.Content.ReadAsStringAsync();

                    if (!response.IsSuccessStatusCode)
                    {
                        // Fallback to prompt engineering if function calling fails
                        return await CompleteWithPromptEngineeredTools(prompt, model, tools, endpoint, options);
                    }

                    var responseJson = JObject.Parse(responseContent);
                    return responseJson["response"]?.ToString() ?? "";
                }
                else
                {
                    // Use prompt engineering for models without native function calling
                    return await CompleteWithPromptEngineeredTools(prompt, model, tools, endpoint, options);
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Ollama tools completion error: {ex.Message}");
                // Fallback to regular completion
                return await CompleteAsync(prompt, model, apiKey, endpoint, options);
            }
        }

        private async Task<string> CompleteWithPromptEngineeredTools(string prompt, string model, List<JObject> tools, string endpoint, LLMRequestOptions options)
        {
            // Build a prompt that includes tool descriptions
            var toolsDescription = "Available tools:\n";
            foreach (var tool in tools)
            {
                toolsDescription += $"\n- {tool["name"]}: {tool["description"]}\n";
                toolsDescription += $"  Parameters: {tool["inputSchema"]?.ToString(Formatting.Indented)}\n";
            }

            var enhancedPrompt = $@"{toolsDescription}

Instructions: You are an AI assistant with access to the above tools. When you need to use a tool, respond with a JSON object in this format:
{{
    ""tool"": ""tool_name"",
    ""arguments"": {{ /* tool arguments */ }}
}}

User request: {prompt}";

            return await CompleteAsync(enhancedPrompt, model, "", endpoint, options);
        }

        public async Task PullModelAsync(string model, string endpoint = null)
        {
            try
            {
                endpoint = string.IsNullOrEmpty(endpoint) ? DEFAULT_ENDPOINT : endpoint;
                
                var requestBody = new { name = model };
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync($"{endpoint}/api/pull", content);
                
                if (!response.IsSuccessStatusCode)
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Failed to pull model: {errorContent}");
                }

                Debug.Log($"Successfully pulled Ollama model: {model}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to pull Ollama model: {ex.Message}");
                throw;
            }
        }
    }
}