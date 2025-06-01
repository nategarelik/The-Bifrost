using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Bifrost.Editor.Settings
{
    /// <summary>
    /// ScriptableObject for Bifrost persistent settings
    /// </summary>
    public class BifrostSettings : ScriptableObject
    {
        private const string SETTINGS_PATH = "Assets/Bifrost/Resources/BifrostSettings.asset";

        [SerializeField] private BifrostProvider provider = BifrostProvider.OpenRouter;
        [SerializeField] private string openAIApiKey;
        [SerializeField] private string openAIEndpoint = "https://openrouter.ai/api/v1/chat/completions";
        [SerializeField] private string selectedModel = "openrouter/auto";
        [SerializeField] private BifrostTheme theme = BifrostTheme.System;
        [SerializeField] private int maxTokens = 2048;
        [SerializeField] private float temperature = 0.7f;
        [SerializeField] private int timeoutSeconds = 60;
        [SerializeField] private float topP = 1.0f;
        [SerializeField] private float frequencyPenalty = 0.0f;
        [SerializeField] private float presencePenalty = 0.0f;
        [SerializeField] private List<Bifrost.Editor.UI.BifrostSettingsUI.CustomHeader> customHeaders = new List<Bifrost.Editor.UI.BifrostSettingsUI.CustomHeader>();

        public BifrostProvider Provider => provider;
        public string OpenAIApiKey => openAIApiKey;
        public string OpenAIEndpoint => openAIEndpoint;
        public string SelectedModel => selectedModel;
        public BifrostTheme Theme => theme;
        public int MaxTokens => maxTokens;
        public float Temperature => temperature;
        public int TimeoutSeconds => timeoutSeconds;
        public float TopP => topP;
        public float FrequencyPenalty => frequencyPenalty;
        public float PresencePenalty => presencePenalty;
        public List<Bifrost.Editor.UI.BifrostSettingsUI.CustomHeader> CustomHeaders => customHeaders;

        public static BifrostSettings GetOrCreateSettings()
        {
            BifrostSettingsUI.EnsureBifrostResourcesFolder();
            var settings = AssetDatabase.LoadAssetAtPath<BifrostSettings>(SETTINGS_PATH);
            if (settings == null)
            {
                settings = ScriptableObject.CreateInstance<BifrostSettings>();
                AssetDatabase.CreateAsset(settings, SETTINGS_PATH);
                AssetDatabase.SaveAssets();
            }
            return settings;
        }
    }
}