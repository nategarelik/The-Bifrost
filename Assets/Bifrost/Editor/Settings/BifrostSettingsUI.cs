using UnityEngine;
using UnityEditor;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bifrost.Editor.AI;

namespace Bifrost.Editor.Settings
{
    // WARNING: EditorPrefs is not secure for storing API keys in production. Use a secure storage method for release builds.

    public enum BifrostProvider
    {
        OpenAI,
        Anthropic,
        Gemini,
        HuggingFace,
        OpenRouter,
        Local
    }

    /// <summary>
    /// Interface for API key storage abstraction.
    /// </summary>
    public interface IApiKeyStorage
    {
        string GetApiKey(string keyName);
        void SetApiKey(string keyName, string value);
    }

    /// <summary>
    /// Default implementation using EditorPrefs (not secure).
    /// </summary>
    public class DefaultEditorPrefsApiKeyStorage : IApiKeyStorage
    {
        public string GetApiKey(string keyName) => EditorPrefs.GetString(keyName, "");
        public void SetApiKey(string keyName, string value) => EditorPrefs.SetString(keyName, value);
    }

    /// <summary>
    /// Settings/configuration UI for The Bifrost. Supports global (EditorPrefs) and per-project (ScriptableObject) persistence.
    /// </summary>
    public class BifrostSettingsUI
    {
        private BifrostSettings settings;
        private SerializedObject serializedSettings;
        private bool useGlobalSettings;
        private const string GLOBAL_SETTINGS_KEY = "Bifrost_GlobalSettings";
        private const string GLOBAL_USE_KEY = "Bifrost_UseGlobalSettings";
        private Bifrost.Editor.AI.BifrostAgent testAgent;
        private bool isTestingConnection = false;
        private IApiKeyStorage apiKeyStorage = new DefaultEditorPrefsApiKeyStorage();

        [Serializable]
        public class CustomHeader
        {
            public string key;
            public string value;
        }

        public BifrostSettingsUI()
        {
            useGlobalSettings = EditorPrefs.GetBool(GLOBAL_USE_KEY, false);
            settings = BifrostSettings.GetOrCreateSettings();
            serializedSettings = new SerializedObject(settings);
            testAgent = new Bifrost.Editor.AI.BifrostAgent(null, null); // Only for connection test
        }

        public void Draw()
        {
            // Global settings toggle
            EditorGUILayout.BeginHorizontal();
            bool newUseGlobal = EditorGUILayout.ToggleLeft("Use global settings for all projects", useGlobalSettings);
            if (newUseGlobal != useGlobalSettings)
            {
                useGlobalSettings = newUseGlobal;
                EditorPrefs.SetBool(GLOBAL_USE_KEY, useGlobalSettings);
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            if (useGlobalSettings)
            {
                DrawGlobalSettings();
            }
            else
            {
                DrawProjectSettings();
            }
        }

        private void DrawGlobalSettings()
        {
            EditorGUILayout.LabelField("Global Bifrost Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Load from EditorPrefs
            var provider = (BifrostProvider)EditorPrefs.GetInt("Bifrost_Global_Provider", (int)BifrostProvider.OpenRouter);
            provider = (BifrostProvider)EditorGUILayout.EnumPopup("Provider", provider);
            EditorPrefs.SetInt("Bifrost_Global_Provider", (int)provider);

            string apiKey = apiKeyStorage.GetApiKey("Bifrost_Global_ApiKey");
            string endpoint = EditorPrefs.GetString("Bifrost_Global_Endpoint", "https://openrouter.ai/api/v1/chat/completions");
            string model = EditorPrefs.GetString("Bifrost_Global_Model", "openrouter/auto");
            int theme = EditorPrefs.GetInt("Bifrost_Global_Theme", 0);
            int maxTokens = EditorPrefs.GetInt("Bifrost_Global_MaxTokens", 2048);
            float temperature = EditorPrefs.GetFloat("Bifrost_Global_Temperature", 0.7f);
            int timeoutSeconds = EditorPrefs.GetInt("Bifrost_Global_TimeoutSeconds", 60);
            float topP = EditorPrefs.GetFloat("Bifrost_Global_TopP", 1.0f);
            float frequencyPenalty = EditorPrefs.GetFloat("Bifrost_Global_FrequencyPenalty", 0.0f);
            float presencePenalty = EditorPrefs.GetFloat("Bifrost_Global_PresencePenalty", 0.0f);

            // Only show OpenRouter fields for this reference implementation
            if (provider == BifrostProvider.OpenRouter)
            {
                apiKey = EditorGUILayout.TextField(new GUIContent("API Key", "Your API key for the selected provider."), apiKey);
                endpoint = EditorGUILayout.TextField(new GUIContent("Endpoint", "The API endpoint URL."), endpoint);
                model = EditorGUILayout.TextField(new GUIContent("Model", "The model name or ID to use."), model);
                maxTokens = EditorGUILayout.IntField(new GUIContent("Max Tokens", "Maximum number of tokens in the response."), maxTokens);
                temperature = EditorGUILayout.Slider(new GUIContent("Temperature", "Controls randomness: lower is more deterministic."), temperature, 0f, 2f);
                topP = EditorGUILayout.Slider(new GUIContent("Top P", "Nucleus sampling: limits the next token selection to a subset with cumulative probability top_p."), topP, 0f, 1f);
                frequencyPenalty = EditorGUILayout.Slider(new GUIContent("Frequency Penalty", "Penalizes new tokens based on their frequency so far."), frequencyPenalty, 0f, 2f);
                presencePenalty = EditorGUILayout.Slider(new GUIContent("Presence Penalty", "Penalizes new tokens based on whether they appear in the text so far."), presencePenalty, 0f, 2f);
                timeoutSeconds = EditorGUILayout.IntField(new GUIContent("Timeout (s)", "Request timeout in seconds."), timeoutSeconds);
            }
            else
            {
                EditorGUILayout.HelpBox("Only OpenRouter is implemented as a reference.", MessageType.Info);
            }

            theme = (int)(BifrostTheme)EditorGUILayout.EnumPopup("Theme", (BifrostTheme)theme);

            apiKeyStorage.SetApiKey("Bifrost_Global_ApiKey", apiKey);
            EditorPrefs.SetString("Bifrost_Global_Endpoint", endpoint);
            EditorPrefs.SetString("Bifrost_Global_Model", model);
            EditorPrefs.SetInt("Bifrost_Global_Theme", theme);
            EditorPrefs.SetInt("Bifrost_Global_MaxTokens", maxTokens);
            EditorPrefs.SetFloat("Bifrost_Global_Temperature", temperature);
            EditorPrefs.SetInt("Bifrost_Global_TimeoutSeconds", timeoutSeconds);
            EditorPrefs.SetFloat("Bifrost_Global_TopP", topP);
            EditorPrefs.SetFloat("Bifrost_Global_FrequencyPenalty", frequencyPenalty);
            EditorPrefs.SetFloat("Bifrost_Global_PresencePenalty", presencePenalty);

            EditorGUILayout.LabelField("Custom Headers", EditorStyles.boldLabel);
            int headerCount = EditorPrefs.GetInt("Bifrost_Global_CustomHeaderCount", 0);
            for (int i = 0; i < headerCount; i++)
            {
                string key = EditorPrefs.GetString($"Bifrost_Global_CustomHeaderKey_{i}", "");
                string value = EditorPrefs.GetString($"Bifrost_Global_CustomHeaderValue_{i}", "");
                EditorGUILayout.BeginHorizontal();
                key = EditorGUILayout.TextField(new GUIContent("Key", "Header key"), key);
                value = EditorGUILayout.TextField(new GUIContent("Value", "Header value"), value);
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    // Remove header
                    for (int j = i; j < headerCount - 1; j++)
                    {
                        EditorPrefs.SetString($"Bifrost_Global_CustomHeaderKey_{j}", EditorPrefs.GetString($"Bifrost_Global_CustomHeaderKey_{j + 1}", ""));
                        EditorPrefs.SetString($"Bifrost_Global_CustomHeaderValue_{j}", EditorPrefs.GetString($"Bifrost_Global_CustomHeaderValue_{j + 1}", ""));
                    }
                    headerCount--;
                    EditorPrefs.SetInt("Bifrost_Global_CustomHeaderCount", headerCount);
                    i--;
                    EditorGUILayout.EndHorizontal();
                    continue;
                }
                EditorGUILayout.EndHorizontal();
                EditorPrefs.SetString($"Bifrost_Global_CustomHeaderKey_{i}", key);
                EditorPrefs.SetString($"Bifrost_Global_CustomHeaderValue_{i}", value);
            }
            if (GUILayout.Button("Add Header"))
            {
                EditorPrefs.SetInt("Bifrost_Global_CustomHeaderCount", headerCount + 1);
            }
            EditorGUILayout.Space();

            GUI.enabled = !isTestingConnection;
            if (GUILayout.Button(isTestingConnection ? "Testing..." : "Test Connection"))
            {
                TestConnectionAsync(true);
            }
            GUI.enabled = true;
        }

        private void DrawProjectSettings()
        {
            serializedSettings.Update();

            EditorGUILayout.LabelField("Project Bifrost Settings", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Provider selection
            var providerProp = serializedSettings.FindProperty("provider");
            EditorGUILayout.PropertyField(providerProp, new GUIContent("Provider"));
            BifrostProvider provider = (BifrostProvider)providerProp.enumValueIndex;

            // Only show OpenRouter fields for this reference implementation
            if (provider == BifrostProvider.OpenRouter)
            {
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("openAIApiKey"), new GUIContent("API Key", "Your API key for the selected provider."));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("openAIEndpoint"), new GUIContent("Endpoint (optional)", "The API endpoint URL."));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("selectedModel"), new GUIContent("Model", "The model name or ID to use."));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("maxTokens"), new GUIContent("Max Tokens", "Maximum number of tokens in the response."));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("temperature"), new GUIContent("Temperature", "Controls randomness: lower is more deterministic."));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("topP"), new GUIContent("Top P", "Nucleus sampling: limits the next token selection to a subset with cumulative probability top_p."));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("frequencyPenalty"), new GUIContent("Frequency Penalty", "Penalizes new tokens based on their frequency so far."));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("presencePenalty"), new GUIContent("Presence Penalty", "Penalizes new tokens based on whether they appear in the text so far."));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("timeoutSeconds"), new GUIContent("Timeout (s)", "Request timeout in seconds."));
            }
            else
            {
                EditorGUILayout.HelpBox("Only OpenRouter is implemented as a reference.", MessageType.Info);
            }

            EditorGUILayout.PropertyField(serializedSettings.FindProperty("theme"), new GUIContent("Theme"));

            EditorGUILayout.LabelField("Custom Headers", EditorStyles.boldLabel);
            var headersProp = serializedSettings.FindProperty("customHeaders");
            for (int i = 0; i < headersProp.arraySize; i++)
            {
                var headerProp = headersProp.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.PropertyField(headerProp.FindPropertyRelative("key"), new GUIContent("Key", "Header key"));
                EditorGUILayout.PropertyField(headerProp.FindPropertyRelative("value"), new GUIContent("Value", "Header value"));
                if (GUILayout.Button("-", GUILayout.Width(20)))
                {
                    headersProp.DeleteArrayElementAtIndex(i);
                    EditorGUILayout.EndHorizontal();
                    break;
                }
                EditorGUILayout.EndHorizontal();
            }
            if (GUILayout.Button("Add Header"))
            {
                headersProp.InsertArrayElementAtIndex(headersProp.arraySize);
            }
            EditorGUILayout.Space();

            GUI.enabled = !isTestingConnection;
            if (GUILayout.Button(isTestingConnection ? "Testing..." : "Test Connection"))
            {
                TestConnectionAsync(false);
            }
            GUI.enabled = true;

            EditorGUILayout.Space();
            if (GUILayout.Button("Save Settings"))
            {
                serializedSettings.ApplyModifiedProperties();
                EditorUtility.SetDirty(settings);
                AssetDatabase.SaveAssets();
            }
        }

        private async void TestConnectionAsync(bool global)
        {
            isTestingConnection = true;
            try
            {
                bool ok = await testAgent.TestConnectionAsync();
                EditorUtility.DisplayDialog("Test Connection", ok ? "Connection successful!" : "Connection failed.", "OK");
            }
            catch (Exception ex)
            {
                EditorUtility.DisplayDialog("Test Connection", $"Error: {ex.Message}", "OK");
            }
            finally
            {
                isTestingConnection = false;
            }
        }

        public static void EnsureBifrostResourcesFolder()
        {
            string[] parts = { "Assets", "Bifrost", "Resources" };
            string current = parts[0];
            for (int i = 1; i < parts.Length; i++)
            {
                string next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }

    public enum BifrostTheme
    {
        System,
        Light,
        Dark
    }
}