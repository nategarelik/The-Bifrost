using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using Bifrost.Editor.UI;
using Bifrost.Editor.AI;
using System.Linq; // For mode name list

namespace Bifrost.Editor
{
    public class BifrostEditorWindow : EditorWindow
    {
        private enum Tab { Chat, Settings, Modes, PromptLibrary, SteamGuide }
        private Tab currentTab = Tab.Chat;

        // UI Components
        private BifrostChatUI chatUI;
        private BifrostSettingsUI settingsUI;
        private BifrostModeEditor modeEditor;
        private BifrostPromptLibraryUI promptLibraryUI;

        // Core Systems
        private BifrostAgent bifrostAgent;
        private UnityProjectManager projectManager;
        private PromptTemplateManager promptManager;
        private UnityContextAnalyzer contextAnalyzer;
        private GameSystemGenerator systemGenerator;
        private ImageTo3DGenerator imageTo3DGenerator;

        // State
        private string currentMode = "Code";
        private bool awaitingApproval = false;
        private GameSystemPlan pendingPlan = null;
        private string lastUserMessage = null;
        private string errorMessage = null;
        private bool showOnboarding = false;
        private const string ONBOARDING_SHOWN_KEY = "Bifrost_OnboardingShown";

        [MenuItem("Window/Bifrost AI Assistant")]
        public static void ShowWindow()
        {
            var window = GetWindow<BifrostEditorWindow>(false, "Bifrost AI Assistant", true);
            window.minSize = new Vector2(500, 600);
        }

        private async void OnEnable()
        {
            Bifrost.Editor.UI.BifrostSettingsUI.EnsureBifrostResourcesFolder();

            chatUI = new BifrostChatUI();
            settingsUI = new BifrostSettingsUI();
            modeEditor = new BifrostModeEditor();
            projectManager = new UnityProjectManager();
            promptManager = new PromptTemplateManager();
            contextAnalyzer = new UnityContextAnalyzer();
            bifrostAgent = new BifrostAgent(promptManager, contextAnalyzer);
            systemGenerator = new GameSystemGenerator(projectManager, bifrostAgent);
            imageTo3DGenerator = new ImageTo3DGenerator();
            promptLibraryUI = new BifrostPromptLibraryUI(promptManager);

            chatUI.OnMessageSent += OnUserMessage;
            chatUI.OnClearChat += () => { errorMessage = null; };

            if (!EditorPrefs.GetBool(ONBOARDING_SHOWN_KEY, false))
            {
                showOnboarding = true;
                EditorPrefs.SetBool(ONBOARDING_SHOWN_KEY, true);
                currentTab = Tab.Chat;
            }

            await promptManager.LoadTemplatesAsync();
        }

        private void OnDisable()
        {
            chatUI.OnMessageSent -= OnUserMessage;
        }

        private void OnGUI()
        {
            if (chatUI == null) chatUI = new BifrostChatUI();
            if (settingsUI == null) settingsUI = new BifrostSettingsUI();
            if (modeEditor == null) modeEditor = new BifrostModeEditor();
            if (promptManager == null) promptManager = new PromptTemplateManager();
            if (promptLibraryUI == null) promptLibraryUI = new BifrostPromptLibraryUI(promptManager);

            DrawTabs();
            EditorGUILayout.Space();

            if (showOnboarding)
            {
                DrawOnboardingPanel();
            }

            if (!string.IsNullOrEmpty(errorMessage))
            {
                EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
            }

            switch (currentTab)
            {
                case Tab.Chat:
                    DrawChatTab();
                    break;
                case Tab.Settings:
                    if (settingsUI != null) settingsUI.Draw();
                    break;
                case Tab.Modes:
                    if (modeEditor != null) modeEditor.Draw();
                    break;
                case Tab.PromptLibrary:
                    DrawPromptLibraryTab();
                    break;
                case Tab.SteamGuide:
                    DrawSteamGuidePanel();
                    break;
            }
        }

        private void DrawTabs()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            if (GUILayout.Toggle(currentTab == Tab.Chat, new GUIContent("Chat", "Chat with the AI assistant"), EditorStyles.toolbarButton)) currentTab = Tab.Chat;
            if (GUILayout.Toggle(currentTab == Tab.Settings, new GUIContent("Settings", "Configure LLM and tool options"), EditorStyles.toolbarButton)) currentTab = Tab.Settings;
            if (GUILayout.Toggle(currentTab == Tab.Modes, new GUIContent("Modes", "Customize AI modes and prompts"), EditorStyles.toolbarButton)) currentTab = Tab.Modes;
            if (GUILayout.Toggle(currentTab == Tab.PromptLibrary, new GUIContent("Prompts", "Browse prompt templates"), EditorStyles.toolbarButton)) currentTab = Tab.PromptLibrary;
            if (GUILayout.Toggle(currentTab == Tab.SteamGuide, new GUIContent("Steam Guide", "Tips for Steam release"), EditorStyles.toolbarButton)) currentTab = Tab.SteamGuide;
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawChatTab()
        {
            if (chatUI == null) return;

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Mode:", GUILayout.Width(40));
            var modeLibrary = BifrostModeLibrary.GetOrCreateLibrary();
            var modeNames = modeLibrary.Modes.Select(m => m.name).ToList();
            int selectedIdx = Mathf.Max(0, modeNames.IndexOf(currentMode));
            int newIdx = EditorGUILayout.Popup(selectedIdx, modeNames.ToArray(), GUILayout.Width(120));
            if (newIdx != selectedIdx && newIdx < modeNames.Count)
            {
                currentMode = modeNames[newIdx];
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            Rect chatRect = GUILayoutUtility.GetRect(position.width, position.height - (awaitingApproval ? 200 : 80));
            chatUI.Draw(chatRect);

            if (awaitingApproval && pendingPlan != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("AI Planned Actions:", EditorStyles.boldLabel);
                if (pendingPlan.Scripts.Any()) GUILayout.Label($"- Scripts to create/modify: {pendingPlan.Scripts.Count}");
                if (pendingPlan.Prefabs.Any()) GUILayout.Label($"- Prefabs to create/modify: {pendingPlan.Prefabs.Count}");
                if (pendingPlan.UIs.Any()) GUILayout.Label($"- UI elements to create/modify: {pendingPlan.UIs.Count}");

                EditorGUILayout.Space();
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Approve & Apply"))
                {
                    _ = ApplyPlannedSystemAsync();
                }
                if (GUILayout.Button("Reject"))
                {
                    awaitingApproval = false;
                    pendingPlan = null;
                    chatUI.AddResponse("Action rejected. No changes applied.");
                }
                EditorGUILayout.EndHorizontal();
            }
        }

        private GameSystemPlan ConvertToGameSystemPlan(LLMGameSystemPlan llmPlan)
        {
            var plan = new GameSystemPlan();
            if (llmPlan.steps != null)
            {
                foreach (var step in llmPlan.steps)
                {
                    if (string.IsNullOrWhiteSpace(step)) continue;
                    string stepLower = step.ToLower();
                    string systemName = llmPlan.systemName ?? "GeneratedSystem";
                    if (stepLower.Contains("script"))
                    {
                        plan.Scripts.Add(new PlannedScript { Path = $"Assets/Bifrost/Runtime/Scripts/{systemName}_{System.Guid.NewGuid().ToString().Substring(0, 4)}.cs", Content = "// TODO: Generated script content for: " + step });
                    }
                    else if (stepLower.Contains("prefab"))
                    {
                        plan.Prefabs.Add(new PlannedPrefab { Path = $"Assets/Bifrost/Runtime/Prefabs/{systemName}_{System.Guid.NewGuid().ToString().Substring(0, 4)}.prefab", Template = step });
                    }
                    else if (stepLower.Contains("ui") || stepLower.Contains("user interface"))
                    {
                        plan.UIs.Add(new PlannedUI { Path = $"Assets/Bifrost/Runtime/UI/{systemName}_{System.Guid.NewGuid().ToString().Substring(0, 4)}.uxml", Template = step });
                    }
                }
            }
            return plan;
        }

        private async void OnUserMessage(string message)
        {
            if (string.IsNullOrWhiteSpace(message)) return;
            errorMessage = null;
            if (chatUI == null) return;
            chatUI.SetProcessingState(true);
            lastUserMessage = message;
            try
            {
                if (message.ToLower().StartsWith("generate a ") && message.ToLower().Contains("from this image"))
                {
                    string imagePath = EditorUtility.OpenFilePanel("Select Reference Image", Application.dataPath, "png,jpg,jpeg");
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        chatUI.AddResponse("Generating 3D model from image...");
                        string prefabPath = await imageTo3DGenerator.Generate3DModelFromImageAsync(imagePath);
                        chatUI.AddResponse(!string.IsNullOrEmpty(prefabPath) ? $"3D model generated: {prefabPath}" : "Failed to generate 3D model.");
                    }
                    else
                    {
                        chatUI.AddResponse("No image selected.");
                    }
                }
                else
                {
                    chatUI.AddResponse("Planning actions...");
                    var llmPlan = await systemGenerator.PlanGameSystemAsync(message);
                    var plan = ConvertToGameSystemPlan(llmPlan);
                    if (plan != null && (plan.Scripts.Any() || plan.Prefabs.Any() || plan.UIs.Any()))
                    {
                        pendingPlan = plan;
                        awaitingApproval = true;
                        chatUI.AddResponse("Plan ready for review. Approve to apply changes.");
                    }
                    else
                    {
                        chatUI.AddResponse(llmPlan?.error ?? "AI could not generate a valid plan.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Bifrost OnUserMessage Error: {ex.Message}\n{ex.StackTrace}");
                errorMessage = $"Error: {ex.Message}";
                chatUI.AddResponse("An error occurred. See console and error message above.");
            }
            finally
            {
                if (chatUI != null) chatUI.SetProcessingState(false);
            }
        }

        private async Task ApplyPlannedSystemAsync()
        {
            if (pendingPlan == null || chatUI == null) return;

            chatUI.SetProcessingState(true);
            errorMessage = null;
            awaitingApproval = false;
            try
            {
                bool success = await systemGenerator.ApplyGameSystemAsync(pendingPlan);
                chatUI.AddResponse(success ? "Changes applied!" : "Some changes failed. See console.");
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"Bifrost ApplyPlannedSystemAsync Error: {ex.Message}\n{ex.StackTrace}");
                errorMessage = $"Error applying changes: {ex.Message}";
                chatUI.AddResponse("Error applying changes. See console.");
            }
            finally
            {
                pendingPlan = null;
                if (chatUI != null) chatUI.SetProcessingState(false);
            }
        }

        private void DrawOnboardingPanel()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Welcome to The Bifrost!", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Bifrost lets you use natural language to develop Unity games.\n\n" +
                                     "- Use the chat to ask for scripts, assets, or scene changes.\n" +
                                     "- Configure your LLM provider in Settings.\n" +
                                     "- Customize Modes and browse Prompts.\n" +
                                     "- Use the Steam Guide for release tips.\n\n" +
                                     "Close this message to begin.", EditorStyles.wordWrappedLabel);
            if (GUILayout.Button("Got it! Close Onboarding"))
            {
                showOnboarding = false;
            }
            EditorGUILayout.EndVertical();
        }

        private void DrawPromptLibraryTab()
        {
            if (promptLibraryUI == null) return;

            promptLibraryUI.Draw();
            if (promptLibraryUI.SelectedTemplateContent != null)
            {
                if (GUILayout.Button("Insert into Chat Input"))
                {
                    if (chatUI != null) chatUI.SetInput(promptLibraryUI.SelectedTemplateContent);
                    currentTab = Tab.Chat;
                }
            }
        }

        private void DrawSteamGuidePanel()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("How to Get a Steam-Ready Game", EditorStyles.boldLabel);
            EditorGUILayout.HelpBox("1. Prepare game for build (scenes, assets, settings).\n" +
                                  "2. Integrate Steamworks.NET or Facepunch.Steamworks.\n" +
                                  "3. Set up Steam app on Steamworks dashboard.\n" +
                                  "4. Build and upload using SteamPipe.\n" +
                                  "5. Test with Steam client.\n" +
                                  "6. Follow Steam's release checklist.", MessageType.Info);
            if (GUILayout.Button("Back to Chat"))
            {
                currentTab = Tab.Chat;
            }
            EditorGUILayout.EndVertical();
        }
    }
}