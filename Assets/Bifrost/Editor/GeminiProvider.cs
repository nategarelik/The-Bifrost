using System.Threading.Tasks;
using UnityEngine.Networking;
using UnityEngine;
using System.Text;
using Bifrost.Editor;

namespace Bifrost.Editor
{
    public class GeminiProvider : IBifrostLLMProvider
    {
        public string Name => "Gemini";

        public async Task<string> CompleteAsync(string prompt, string model, string apiKey, string endpoint, LLMRequestOptions options)
        {
            var requestBody = new
            {
                model = model,
                messages = new[] {
                    new { role = "user", content = prompt }
                },
                max_tokens = options?.maxTokens ?? 1024,
                temperature = options?.temperature ?? 0.7f
            };
            string json = JsonUtility.ToJson(requestBody);
            using (UnityWebRequest req = new UnityWebRequest(endpoint, "POST"))
            {
                byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
                req.uploadHandler = new UploadHandlerRaw(bodyRaw);
                req.downloadHandler = new DownloadHandlerBuffer();
                req.SetRequestHeader("Content-Type", "application/json");
                req.SetRequestHeader("Authorization", $"Bearer {apiKey}");
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
                    Debug.LogError($"GeminiProvider Error: {req.error}");
                    return null;
                }
                return req.downloadHandler.text;
            }
        }

        public async Task<bool> TestConnectionAsync(string apiKey, string endpoint, string model)
        {
            string testPrompt = "Say hello.";
            string result = await CompleteAsync(testPrompt, model, apiKey, endpoint);
            return !string.IsNullOrEmpty(result);
        }
    }
}