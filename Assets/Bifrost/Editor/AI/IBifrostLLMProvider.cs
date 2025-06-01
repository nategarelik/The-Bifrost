using System.Threading.Tasks;
using System.Collections.Generic;
using Bifrost.Editor.Settings;
using Newtonsoft.Json.Linq;

namespace Bifrost.Editor.AI
{
    public interface IBifrostLLMProvider
    {
        string Name { get; }
        Task<string> CompleteAsync(string prompt, string model, string apiKey, string endpoint, LLMRequestOptions options);
        Task<bool> TestConnectionAsync(string apiKey, string endpoint, string model);
        
        // Enhanced interface properties
        bool SupportsStreaming { get; }
        bool SupportsToolCalling { get; }
        string DisplayName { get; }
        bool RequiresAPIKey { get; }
    }
    
    // Extended interface for advanced features
    public interface IBifrostLLMProviderAdvanced : IBifrostLLMProvider
    {
        Task<string> CompleteWithToolsAsync(string prompt, string model, List<JObject> tools, string apiKey, string endpoint, LLMRequestOptions options);
        Task<string> StreamCompleteAsync(string prompt, string model, string apiKey, string endpoint, LLMRequestOptions options, System.Action<string> onToken);
    }

    public class LLMRequestOptions
    {
        public int maxTokens = 1024;
        public float temperature = 0.7f;
        public float topP = 1.0f;
        public float frequencyPenalty = 0.0f;
        public float presencePenalty = 0.0f;
        public int timeoutSeconds = 60;
        public List<Bifrost.Editor.Settings.BifrostSettingsUI.CustomHeader> customHeaders = new List<Bifrost.Editor.Settings.BifrostSettingsUI.CustomHeader>();
        public List<string> StopSequences { get; set; }
        public Dictionary<string, string> CustomHeaders { get; set; }
    }
}