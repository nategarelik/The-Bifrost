using System.Threading.Tasks;

namespace Bifrost.Editor
{
    public interface IBifrostLLMProvider
    {
        string Name { get; }
        Task<string> CompleteAsync(string prompt, string model, string apiKey, string endpoint, LLMRequestOptions options);
        Task<bool> TestConnectionAsync(string apiKey, string endpoint, string model);
    }

    public class LLMRequestOptions
    {
        public int maxTokens = 1024;
        public float temperature = 0.7f;
        public float topP = 1.0f;
        public float frequencyPenalty = 0.0f;
        public float presencePenalty = 0.0f;
        public int timeoutSeconds = 60;
        public System.Collections.Generic.List<Bifrost.Editor.UI.BifrostSettingsUI.CustomHeader> customHeaders = new System.Collections.Generic.List<Bifrost.Editor.UI.BifrostSettingsUI.CustomHeader>();
    }
}