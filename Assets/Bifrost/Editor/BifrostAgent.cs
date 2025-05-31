using System.Threading.Tasks;
using UnityEngine;
using Bifrost.Editor.UI;
using Bifrost.Editor.AI;
using Bifrost.Editor.AI.Providers;
using System.Linq;

namespace Bifrost.Editor
{
    public class BifrostAgent
    {
        private IBifrostLLMProvider provider;
        private PromptTemplateManager promptManager;
        private UnityContextAnalyzer contextAnalyzer;

        public BifrostAgent(PromptTemplateManager promptManager, UnityContextAnalyzer contextAnalyzer)
        {
            this.promptManager = promptManager;
            this.contextAnalyzer = contextAnalyzer;
        }

        private void GetProviderSettings(out BifrostProvider providerType, out string apiKey, out string endpoint, out string model)
        {
            bool useGlobal = UnityEditor.EditorPrefs.GetBool("Bifrost_UseGlobalSettings", false);
            if (useGlobal)
            {
                providerType = (BifrostProvider)UnityEditor.EditorPrefs.GetInt("Bifrost_Global_Provider", (int)BifrostProvider.OpenRouter);
                apiKey = UnityEditor.EditorPrefs.GetString("Bifrost_Global_ApiKey", "");
                endpoint = UnityEditor.EditorPrefs.GetString("Bifrost_Global_Endpoint", "https://openrouter.ai/api/v1/chat/completions");
                model = UnityEditor.EditorPrefs.GetString("Bifrost_Global_Model", "openrouter/auto");
            }
            else
            {
                var settings = BifrostSettings.GetOrCreateSettings();
                providerType = settings.Provider;
                apiKey = settings.OpenAIApiKey;
                endpoint = settings.OpenAIEndpoint;
                model = settings.SelectedModel;
            }
        }

        private IBifrostLLMProvider GetProvider(BifrostProvider providerType)
        {
            switch (providerType)
            {
                case BifrostProvider.OpenAI:
                    return new OpenAIProvider();
                case BifrostProvider.Anthropic:
                    return new AnthropicProvider();
                case BifrostProvider.Gemini:
                    return new GeminiProvider();
                case BifrostProvider.HuggingFace:
                    return new HuggingFaceProvider();
                case BifrostProvider.Local:
                    return new LocalProvider();
                case BifrostProvider.OpenRouter:
                default:
                    return new OpenRouterProvider();
            }
        }

        private LLMRequestOptions GetLLMRequestOptions()
        {
            var options = new LLMRequestOptions();
            bool useGlobal = UnityEditor.EditorPrefs.GetBool("Bifrost_UseGlobalSettings", false);
            if (useGlobal)
            {
                options.maxTokens = UnityEditor.EditorPrefs.GetInt("Bifrost_Global_MaxTokens", 2048);
                options.temperature = UnityEditor.EditorPrefs.GetFloat("Bifrost_Global_Temperature", 0.7f);
                options.topP = UnityEditor.EditorPrefs.GetFloat("Bifrost_Global_TopP", 1.0f);
                options.frequencyPenalty = UnityEditor.EditorPrefs.GetFloat("Bifrost_Global_FrequencyPenalty", 0.0f);
                options.presencePenalty = UnityEditor.EditorPrefs.GetFloat("Bifrost_Global_PresencePenalty", 0.0f);
                options.timeoutSeconds = UnityEditor.EditorPrefs.GetInt("Bifrost_Global_TimeoutSeconds", 60);
                int headerCount = UnityEditor.EditorPrefs.GetInt("Bifrost_Global_CustomHeaderCount", 0);
                for (int i = 0; i < headerCount; i++)
                {
                    string key = UnityEditor.EditorPrefs.GetString($"Bifrost_Global_CustomHeaderKey_{i}", "");
                    string value = UnityEditor.EditorPrefs.GetString($"Bifrost_Global_CustomHeaderValue_{i}", "");
                    if (!string.IsNullOrEmpty(key))
                        options.customHeaders.Add(new UI.BifrostSettingsUI.CustomHeader { key = key, value = value });
                }
            }
            else
            {
                var settings = BifrostSettings.GetOrCreateSettings();
                options.maxTokens = settings.MaxTokens;
                options.temperature = settings.Temperature;
                options.topP = settings.TopP;
                options.frequencyPenalty = settings.FrequencyPenalty;
                options.presencePenalty = settings.PresencePenalty;
                options.timeoutSeconds = settings.TimeoutSeconds;
                if (settings.CustomHeaders != null)
                    options.customHeaders.AddRange(settings.CustomHeaders);
            }
            return options;
        }

        private string BuildPromptWithContext(string userPrompt, ProjectContext context)
        {
            // Summarize context (keep concise for LLM input)
            string contextSummary = $"UNITY PROJECT CONTEXT:\n" +
                $"Scripts: {context.ScriptPaths.Count}, Scenes: {context.ScenePaths.Count}, Prefabs: {context.PrefabPaths.Count}, Materials: {context.MaterialPaths.Count}, ScriptableObjects: {context.ScriptableObjectPaths.Count}, Folders: {context.Folders.Count}\n" +
                $"Active Scene: {context.ActiveScene?.Path}\n" +
                $"Root GameObjects: {string.Join(", ", context.ActiveScene?.GameObjects?.Select(go => go.Name) ?? new string[0])}\n";
            return contextSummary + "\n\nUSER REQUEST:\n" + userPrompt;
        }

        public async Task<string> CompleteAsync(string prompt)
        {
            GetProviderSettings(out var providerType, out var apiKey, out var endpoint, out var model);
            provider = GetProvider(providerType);
            var options = GetLLMRequestOptions();
            try
            {
                var context = await contextAnalyzer.AnalyzeProjectAsync();
                // Prepend instruction for JSON output
                string schemaInstruction = "Respond ONLY with a JSON object matching this schema: {\"systemName\":string,\"steps\":[string]}";
                string promptWithContext = schemaInstruction + "\n\n" + BuildPromptWithContext(prompt, context);
                return await provider.CompleteAsync(promptWithContext, model, apiKey, endpoint, options);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"BifrostAgent Error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> TestConnectionAsync()
        {
            GetProviderSettings(out var providerType, out var apiKey, out var endpoint, out var model);
            provider = GetProvider(providerType);
            try
            {
                return await provider.TestConnectionAsync(apiKey, endpoint, model);
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"BifrostAgent TestConnection Error: {ex.Message}");
                return false;
            }
        }

        // Example: PlanGameSystemAsync (with LLM response parsing)
        public async Task<LLMGameSystemPlan> PlanGameSystemAsync(string description)
        {
            string response = await CompleteAsync(description);
            if (string.IsNullOrEmpty(response))
            {
                Debug.LogError("BifrostAgent: LLM response was empty or null.");
                return LLMGameSystemPlan.Fallback(description);
            }
            if (LLMGameSystemPlan.TryParse(response, out var plan))
            {
                return plan;
            }
            else
            {
                Debug.LogError("Raw LLM response: " + (response ?? "<null>"));
                Debug.LogError("BifrostAgent: Failed to parse LLM response into LLMGameSystemPlan. Using fallback plan.");
                return LLMGameSystemPlan.Fallback(description);
            }
        }
    }
}