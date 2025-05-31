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
        // Static instance to ensure only one window exists
        private static BifrostEditorWindow _instance;

        private enum Tab { Chat, Settings, Modes, PromptLibrary, SteamGuide, Debug }
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

        // Debug info fields
        private string lastLLMRequest = null;
        private string lastLLMResponse = null;
        private string lastLLMParseResult = null;

        [MenuItem("Window/Bifrost AI Assistant")]
        public static void ShowWindow()
        {
            // If we already have an instance, focus it instead of creating a new one
            if (_instance != null)
            {
                _instance.Focus();
                return;
            }

            var window = GetWindow<BifrostEditorWindow>(false, "Bifrost AI Assistant", true);
            window.minSize = new Vector2(500, 600);
            _instance = window;
        }

        private async void OnEnable()
        {
            _instance = this;

            Bifrost.Editor.UI.BifrostSettingsUI.EnsureBifrostResourcesFolder();

            InitializeComponents();
            RegisterEventHandlers();

            if (!EditorPrefs.GetBool(ONBOARDING_SHOWN_KEY, false))
            {
                showOnboarding = true;
                EditorPrefs.SetBool(ONBOARDING_SHOWN_KEY, true);
                currentTab = Tab.Chat;
            }

            await promptManager.LoadTemplatesAsync();
        }

        private void InitializeComponents()
        {
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
        }

        private void RegisterEventHandlers()
        {
            if (chatUI != null)
            {
                chatUI.OnMessageSent += OnUserMessage;
                chatUI.OnClearChat += () => { errorMessage = null; };
            }
        }

        private void OnDisable()
        {
            if (chatUI != null)
            {
                chatUI.OnMessageSent -= OnUserMessage;
            }

            // Clear the instance reference if this is the current instance
            if (_instance == this)
            {
                _instance = null;
            }
        }

        private void OnGUI()
        {
            // Ensure components are initialized
            if (chatUI == null || settingsUI == null || modeEditor == null ||
                promptManager == null || promptLibraryUI == null)
            {
                InitializeComponents();
                RegisterEventHandlers();
            }

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
                case Tab.Debug:
                    DrawDebugTab();
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
            if (GUILayout.Toggle(currentTab == Tab.Debug, new GUIContent("Debug", "LLM request/response log"), EditorStyles.toolbarButton)) currentTab = Tab.Debug;
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
                // Make sure all required base directories exist
                EnsureBifrostRuntimeDirectories();

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

        private void EnsureBifrostRuntimeDirectories()
        {
            // Ensure all base directories for game system generation exist
            string[] directories = {
                "Assets/Bifrost",
                "Assets/Bifrost/Runtime",
                "Assets/Bifrost/Runtime/Scripts",
                "Assets/Bifrost/Runtime/Prefabs",
                "Assets/Bifrost/Runtime/UI",
                "Assets/Bifrost/Resources"
            };

            foreach (string dir in directories)
            {
                if (!AssetDatabase.IsValidFolder(dir))
                {
                    // Get parent directory and folder name
                    string[] parts = dir.Split('/');
                    string parentDir = string.Join("/", parts, 0, parts.Length - 1);
                    string folderName = parts[parts.Length - 1];

                    // Create folder if parent exists
                    if (AssetDatabase.IsValidFolder(parentDir))
                    {
                        AssetDatabase.CreateFolder(parentDir, folderName);
                    }
                    else
                    {
                        Debug.LogError($"Cannot create directory {dir} because parent {parentDir} doesn't exist");
                    }
                }
            }

            // Make sure all changes are committed to the AssetDatabase
            AssetDatabase.Refresh();
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
                    lastLLMRequest = message;
                    var (llmPlan, rawResponse) = await systemGenerator.PlanGameSystemAsync(message);
                    lastLLMResponse = rawResponse;
                    var plan = ConvertToGameSystemPlan(llmPlan);
                    if (plan != null && (plan.Scripts.Any() || plan.Prefabs.Any() || plan.UIs.Any()))
                    {
                        pendingPlan = plan;
                        awaitingApproval = true;
                        chatUI.AddResponse("Plan ready for review. Approve to apply changes.");
                    }
                    else
                    {
                        chatUI.AddResponse("AI could not generate a valid plan.");
                    }
                    lastLLMParseResult = (plan != null ? "Success" : "Failed to parse");
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

        private void DrawInfoPanel(string title, string content, string buttonText = null, System.Action buttonAction = null)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField(title, EditorStyles.boldLabel);
            EditorGUILayout.LabelField(content, EditorStyles.wordWrappedLabel);

            if (!string.IsNullOrEmpty(buttonText) && buttonAction != null)
            {
                if (GUILayout.Button(buttonText))
                {
                    buttonAction.Invoke();
                }
            }

            EditorGUILayout.EndVertical();
        }

        private void DrawOnboardingPanel()
        {
            DrawInfoPanel(
                "Welcome to The Bifrost!",
                "Bifrost lets you use natural language to develop Unity games.\n\n" +
                "- Use the chat to ask for scripts, assets, or scene changes.\n" +
                "- Configure your LLM provider in Settings.\n" +
                "- Customize Modes and browse Prompts.\n" +
                "- Use the Steam Guide for release tips.\n\n" +
                "Close this message to begin.",
                "Got it! Close Onboarding",
                () => { showOnboarding = false; }
            );
        }

        private void DrawSteamGuidePanel()
        {
            DrawInfoPanel(
                "How to Get a Steam-Ready Game",
                "1. Prepare game for build (scenes, assets, settings).\n" +
                "2. Integrate Steamworks.NET or Facepunch.Steamworks.\n" +
                "3. Set up Steam app on Steamworks dashboard.\n" +
                "4. Build and upload using SteamPipe.\n" +
                "5. Test with Steam client.\n" +
                "6. Follow Steam's release checklist.",
                "Back to Chat",
                () => { currentTab = Tab.Chat; }
            );
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

        private void DrawDebugTab()
        {
            EditorGUILayout.LabelField("LLM Debug Log", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Last LLM Request:", EditorStyles.miniBoldLabel);
            EditorGUILayout.TextArea(lastLLMRequest ?? "<none>", GUILayout.Height(60));
            EditorGUILayout.LabelField("Last LLM Response:", EditorStyles.miniBoldLabel);
            EditorGUILayout.TextArea(lastLLMResponse ?? "<none>", GUILayout.Height(100));
            EditorGUILayout.LabelField("Last Parse Result:", EditorStyles.miniBoldLabel);
            EditorGUILayout.TextArea(lastLLMParseResult ?? "<none>", GUILayout.Height(40));
        }
    }
}