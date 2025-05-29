using System.Threading.Tasks;
using UnityEngine;

namespace Bifrost.Editor.AI.Providers
{
    public class LocalProvider : IBifrostLLMProvider
    {
        public string Name => "Local";

        public async Task<string> CompleteAsync(string prompt, string model, string apiKey, string endpoint, LLMRequestOptions options)
        {
            // TODO: Implement local model inference (e.g., via local server, ONNX, or Python bridge)
            Debug.LogWarning("LocalProvider: Local model inference is not yet implemented.");
            await Task.CompletedTask;
            return null;
        }

        public async Task<bool> TestConnectionAsync(string apiKey, string endpoint, string model)
        {
            // TODO: Implement local model health check
            Debug.LogWarning("LocalProvider: TestConnection is not yet implemented.");
            await Task.CompletedTask;
            return false;
        }
    }
}