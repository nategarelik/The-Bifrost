using UnityEngine;
using UnityEditor;
using System;
using System.Threading.Tasks;

namespace Bifrost.Editor.UI
{
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
    /// Settings/configuration UI for The Bifrost. Supports global (EditorPrefs) and per-project (ScriptableObject) persistence.
    /// </summary>
    public class BifrostSettingsUI
    {
        private BifrostSettings settings;
        private SerializedObject serializedSettings;
        private bool useGlobalSettings;
        private const string GLOBAL_SETTINGS_KEY = "Bifrost_GlobalSettings";
        private const string GLOBAL_USE_KEY = "Bifrost_UseGlobalSettings";
        private Bifrost.Editor.BifrostAgent testAgent;
        private bool isTestingConnection = false;

        public BifrostSettingsUI()
        {
            useGlobalSettings = EditorPrefs.GetBool(GLOBAL_USE_KEY, false);
            settings = BifrostSettings.GetOrCreateSettings();
            serializedSettings = new SerializedObject(settings);
            testAgent = new Bifrost.Editor.BifrostAgent(null, null); // Only for connection test
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

            string apiKey = EditorPrefs.GetString("Bifrost_Global_ApiKey", "");
            string endpoint = EditorPrefs.GetString("Bifrost_Global_Endpoint", "https://openrouter.ai/api/v1/chat/completions");
            string model = EditorPrefs.GetString("Bifrost_Global_Model", "openrouter/auto");
            int theme = EditorPrefs.GetInt("Bifrost_Global_Theme", 0);

            // Only show OpenRouter fields for this reference implementation
            if (provider == BifrostProvider.OpenRouter)
            {
                apiKey = EditorGUILayout.TextField("API Key", apiKey);
                endpoint = EditorGUILayout.TextField("Endpoint", endpoint);
                model = EditorGUILayout.TextField("Model", model);
            }
            else
            {
                EditorGUILayout.HelpBox("Only OpenRouter is implemented as a reference.", MessageType.Info);
            }

            theme = (int)(BifrostTheme)EditorGUILayout.EnumPopup("Theme", (BifrostTheme)theme);

            EditorPrefs.SetString("Bifrost_Global_ApiKey", apiKey);
            EditorPrefs.SetString("Bifrost_Global_Endpoint", endpoint);
            EditorPrefs.SetString("Bifrost_Global_Model", model);
            EditorPrefs.SetInt("Bifrost_Global_Theme", theme);

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
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("openAIApiKey"), new GUIContent("API Key"));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("openAIEndpoint"), new GUIContent("Endpoint (optional)"));
                EditorGUILayout.PropertyField(serializedSettings.FindProperty("selectedModel"), new GUIContent("Model"));
            }
            else
            {
                EditorGUILayout.HelpBox("Only OpenRouter is implemented as a reference.", MessageType.Info);
            }

            EditorGUILayout.PropertyField(serializedSettings.FindProperty("theme"), new GUIContent("Theme"));

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
    }

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

        public BifrostProvider Provider => provider;
        public string OpenAIApiKey => openAIApiKey;
        public string OpenAIEndpoint => openAIEndpoint;
        public string SelectedModel => selectedModel;
        public BifrostTheme Theme => theme;

        public static BifrostSettings GetOrCreateSettings()
        {
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

    public enum BifrostTheme
    {
        System,
        Light,
        Dark
    }
} 