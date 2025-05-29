using System.Threading.Tasks;
using UnityEngine;
using Bifrost.Editor.UI;

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
                case BifrostProvider.OpenRouter:
                default:
                    return new OpenRouterProvider();
            }
        }

        public async Task<string> CompleteAsync(string prompt)
        {
            GetProviderSettings(out var providerType, out var apiKey, out var endpoint, out var model);
            provider = GetProvider(providerType);
            try
            {
                return await provider.CompleteAsync(prompt, model, apiKey, endpoint);
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
        public async Task<GameSystemPlan> PlanGameSystemAsync(string description)
        {
            string response = await CompleteAsync(description);
            if (string.IsNullOrEmpty(response))
            {
                Debug.LogError("BifrostAgent: LLM response was empty or null.");
                return null;
            }
            try
            {
                // Try to parse the response as JSON into GameSystemPlan
                var plan = JsonUtility.FromJson<GameSystemPlan>(response);
                if (plan == null)
                {
                    Debug.LogError("BifrostAgent: Failed to parse LLM response into GameSystemPlan.");
                }
                return plan;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"BifrostAgent: Error parsing LLM response: {ex.Message}");
                return null;
            }
        }
    }
} 