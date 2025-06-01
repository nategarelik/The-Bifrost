using UnityEditor;
using UnityEngine;
using Bifrost.Editor.UI;
using Bifrost.Editor.Settings;
using Bifrost.Editor.Prompts;
using Bifrost.Editor.AI;
using Bifrost.Editor.Context;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Bifrost.Editor.UI
{
    public class BifrostEditorWindow : EditorWindow
    {
        private bool showOnboarding = false;
        private const string ONBOARDING_SHOWN_KEY = "Bifrost_OnboardingShown";
        private BifrostPromptLibraryUI promptLibraryUI;
        private bool showSteamGuide = false;
        private PromptTemplateManager promptManager;
        private enum Tab { Chat, Settings, Modes, PromptLibrary, Debug }
        private Tab currentTab = Tab.Chat;
        private string errorMessage = null;
        private BifrostSettingsUI settingsUI;
        private BifrostModeEditor modeEditor;
        private BifrostChatUI chatUI;
        private List<string> logMessages = new List<string>();
        private const int MaxLogMessages = 100;
        private BifrostAgent bifrostAgent;
        private UnityContextAnalyzer contextAnalyzer;
        private string selectedMode = "GameDev";
        private List<string> availableModes = new List<string> { "Code", "Architect", "Designer", "GameDev" };

        [MenuItem("Window/Bifrost AI Assistant")]
        public static void ShowWindow()
        {
            GetWindow<BifrostEditorWindow>("Bifrost AI Assistant");
        }

        private void OnEnable()
        {
            BifrostSettingsUI.EnsureBifrostResourcesFolder();
            chatUI = new BifrostChatUI();
            settingsUI = new BifrostSettingsUI();
            modeEditor = new BifrostModeEditor();
            promptManager = new PromptTemplateManager();
            promptLibraryUI = new BifrostPromptLibraryUI(promptManager);
            contextAnalyzer = new UnityContextAnalyzer();
            bifrostAgent = new BifrostAgent(promptManager, contextAnalyzer);
            chatUI.OnMessageSent += OnUserMessageAsync;
            chatUI.OnClearChat += () => LogToPanel("Chat history cleared.");
            if (!EditorPrefs.GetBool(ONBOARDING_SHOWN_KEY, false))
            {
                showOnboarding = true;
                EditorPrefs.SetBool(ONBOARDING_SHOWN_KEY, true);
            }
            LogToPanel("Bifrost Editor Window enabled.");
        }

        private void LogToPanel(string message)
        {
            if (logMessages.Count >= MaxLogMessages)
                logMessages.RemoveAt(0);
            logMessages.Add($"[{System.DateTime.Now:HH:mm:ss}] {message}");
            Debug.Log($"[Bifrost] {message}");
        }

        private async void OnUserMessageAsync(string message)
        {
            chatUI.SetProcessingState(true);
            errorMessage = null;
            LogToPanel($"User message: {message} (Mode: {selectedMode})");
            try
            {
                // Optionally prepend mode to the prompt
                string modePrompt = $"[Mode: {selectedMode}]\n{message}";
                var (llmPlan, rawResponse) = await bifrostAgent.PlanGameSystemAsync(modePrompt);
                if (llmPlan != null && llmPlan.steps != null && llmPlan.steps.Count > 0)
                {
                    chatUI.AddResponse($"AI Plan:\n{string.Join("\n", llmPlan.steps)}");
                    LogToPanel("AI plan received and displayed.");
                }
                else
                {
                    chatUI.AddResponse("AI did not return a valid plan.");
                    LogToPanel("AI did not return a valid plan.");
                }
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
                chatUI.AddResponse($"Error: {ex.Message}");
                LogToPanel($"Error: {ex.Message}");
            }
            finally
            {
                chatUI.SetProcessingState(false);
            }
        }

        private void OnGUI()
        {
            if (chatUI == null) chatUI = new BifrostChatUI();
            if (settingsUI == null) settingsUI = new BifrostSettingsUI();
            if (modeEditor == null) modeEditor = new BifrostModeEditor();
            if (promptLibraryUI == null && promptManager != null) promptLibraryUI = new BifrostPromptLibraryUI(promptManager);

            DrawTabs();
            EditorGUILayout.Space();

            if (showOnboarding)
            {
                DrawOnboardingPanel();
                return;
            }

            if (showSteamGuide)
            {
                DrawSteamGuidePanel();
                return;
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            }

            switch (currentTab)
            {
                case Tab.Chat:
                    if (chatUI != null) DrawChatTab();
                    break;
                case Tab.Settings:
                    if (settingsUI != null) settingsUI.Draw();
                    break;
                case Tab.Modes:
                    DrawModesTab();
                    break;
                case Tab.PromptLibrary:
                    if (promptLibraryUI != null) DrawPromptLibraryTab();
                    break;
                case Tab.Debug:
                    DrawDebugTab();
                    break;
            }
        }

        private void DrawTabs()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(currentTab == Tab.Chat, new GUIContent("Chat", "Chat with the AI assistant"), EditorStyles.toolbarButton)) currentTab = Tab.Chat;
            if (GUILayout.Toggle(currentTab == Tab.Settings, new GUIContent("Settings", "Configure LLM and tool options"), EditorStyles.toolbarButton)) currentTab = Tab.Settings;
            if (GUILayout.Toggle(currentTab == Tab.Modes, new GUIContent("Modes", "Switch between Bifrost modes"), EditorStyles.toolbarButton)) currentTab = Tab.Modes;
            if (GUILayout.Toggle(currentTab == Tab.PromptLibrary, new GUIContent("Prompt Library", "Browse and use prompt templates"), EditorStyles.toolbarButton)) currentTab = Tab.PromptLibrary;
            if (GUILayout.Toggle(currentTab == Tab.Debug, new GUIContent("Debug", "View logs and errors"), EditorStyles.toolbarButton)) currentTab = Tab.Debug;
            if (GUILayout.Button(new GUIContent("Steam Guide", "How to get a Steam-ready game"), EditorStyles.toolbarButton)) showSteamGuide = true;
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawOnboardingPanel()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Welcome to The Bifrost!", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("\nBifrost lets you use natural language to develop Unity games.\n\n- Use the chat to ask for scripts, assets, or scene changes.\n- Configure your LLM provider and advanced options in Settings.\n- Use the Prompt Library for quick-start templates.\n\nFor more help, see the documentation or contact support.");
            if (GUILayout.Button("Close"))
            {
                showOnboarding = false;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawPromptLibraryTab()
        {
            promptLibraryUI.Draw();
            if (promptLibraryUI.SelectedTemplateContent != null)
            {
                if (GUILayout.Button("Insert into Chat Input"))
                {
                    chatUI.SetInput(promptLibraryUI.SelectedTemplateContent);
                }
            }
        }

        private void DrawSteamGuidePanel()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("How to Get a Steam-Ready Game", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("1. Prepare your game for build (scenes, assets, settings).\n2. Integrate Steamworks.NET or Facepunch.Steamworks.\n3. Set up Steam app on Steamworks dashboard.\n4. Build and upload using SteamPipe.\n5. Test with Steam client.\n6. Follow Steam's release checklist.");
            if (GUILayout.Button("Close"))
            {
                showSteamGuide = false;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawChatTab()
        {
            try
            {
                chatUI.Draw(new Rect(0, 0, position.width, position.height));
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"BifrostEditorWindow.DrawChatTab Exception: {ex.Message}\n{ex.StackTrace}");
                errorMessage = $"Error in chat UI: {ex.Message}";
                LogToPanel($"Error in chat UI: {ex.Message}");
            }
        }

        private void DrawModesTab()
        {
            EditorGUILayout.LabelField("Select AI Mode:", EditorStyles.boldLabel);
            for (int i = 0; i < availableModes.Count; i++)
            {
                bool isSelected = selectedMode == availableModes[i];
                if (GUILayout.Toggle(isSelected, availableModes[i], EditorStyles.radioButton))
                {
                    if (!isSelected)
                    {
                        selectedMode = availableModes[i];
                        LogToPanel($"Mode changed to: {selectedMode}");
                    }
                }
            }
            EditorGUILayout.Space();
            modeEditor.Draw();
        }

        private void DrawDebugTab()
        {
            EditorGUILayout.LabelField("Debug Log", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            if (logMessages.Count == 0)
            {
                EditorGUILayout.HelpBox("No log messages yet.", MessageType.Info);
            }
            else
            {
                foreach (var msg in logMessages)
                {
                    EditorGUILayout.LabelField(msg);
                }
            }
            if (GUILayout.Button("Clear Log"))
            {
                logMessages.Clear();
            }
        }
    }
}