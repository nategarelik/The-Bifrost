using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using Bifrost.Editor.UI;
using Bifrost.Editor.AI;

namespace Bifrost.Editor
{
    public class BifrostEditorWindow : EditorWindow
    {
        private enum Tab { Chat, Settings, Modes }
        private Tab currentTab = Tab.Chat;

        // UI Components
        private BifrostChatUI chatUI;
        private BifrostSettingsUI settingsUI;
        private BifrostModeEditor modeEditor;

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

        [MenuItem("Window/Bifrost AI Assistant")]
        public static void ShowWindow()
        {
            var window = GetWindow<BifrostEditorWindow>(false, "Bifrost AI Assistant", true);
            window.minSize = new Vector2(500, 600);
        }

        private async void OnEnable()
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

            chatUI.OnMessageSent += OnUserMessage;
            chatUI.OnClearChat += () => { errorMessage = null; };

            await promptManager.LoadTemplatesAsync();
        }

        private void OnDisable()
        {
            chatUI.OnMessageSent -= OnUserMessage;
        }

        private void OnGUI()
        {
            DrawTabs();
            EditorGUILayout.Space();

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
                    settingsUI.Draw();
                    break;
                case Tab.Modes:
                    modeEditor.Draw();
                    break;
            }
        }

        private void DrawTabs()
        {
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Toggle(currentTab == Tab.Chat, "Chat", EditorStyles.toolbarButton)) currentTab = Tab.Chat;
            if (GUILayout.Toggle(currentTab == Tab.Settings, "Settings", EditorStyles.toolbarButton)) currentTab = Tab.Settings;
            if (GUILayout.Toggle(currentTab == Tab.Modes, "Modes", EditorStyles.toolbarButton)) currentTab = Tab.Modes;
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawChatTab()
        {
            // Mode selection dropdown
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Mode:", GUILayout.Width(40));
            var modeLibrary = BifrostModeLibrary.GetOrCreateLibrary();
            var modeNames = modeLibrary.Modes.ConvertAll(m => m.name);
            int selectedIdx = Mathf.Max(0, modeNames.IndexOf(currentMode));
            int newIdx = EditorGUILayout.Popup(selectedIdx, modeNames.ToArray(), GUILayout.Width(120));
            if (newIdx != selectedIdx)
            {
                currentMode = modeNames[newIdx];
            }
            GUILayout.FlexibleSpace();
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();

            // Main chat UI
            Rect chatRect = GUILayoutUtility.GetRect(position.width, position.height - 60);
            chatUI.Draw(chatRect);

            // Approval system
            if (awaitingApproval && pendingPlan != null)
            {
                EditorGUILayout.Space();
                EditorGUILayout.LabelField("AI Planned Actions:", EditorStyles.boldLabel);
                foreach (var script in pendingPlan.Scripts)
                    EditorGUILayout.LabelField($"Script: {script.Path}");
                foreach (var prefab in pendingPlan.Prefabs)
                    EditorGUILayout.LabelField($"Prefab: {prefab.Path}");
                foreach (var ui in pendingPlan.UIs)
                    EditorGUILayout.LabelField($"UI: {ui.Path}");
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
            var plan = new GameSystemGenerator.GameSystemPlan();
            // For now, treat each step as a script, prefab, or UI based on simple keywords (improve as needed)
            if (llmPlan.steps != null)
            {
                foreach (var step in llmPlan.steps)
                {
                    // Naive parsing: look for keywords
                    if (step.ToLower().Contains("script"))
                    {
                        plan.Scripts.Add(new GameSystemGenerator.PlannedScript { Path = $"Assets/Bifrost/Runtime/{llmPlan.systemName}_Script.cs", Content = "// TODO: Generated script content" });
                    }
                    else if (step.ToLower().Contains("prefab"))
                    {
                        plan.Prefabs.Add(new GameSystemGenerator.PlannedPrefab { Path = $"Assets/Bifrost/Runtime/{llmPlan.systemName}_Prefab.prefab", Template = "" });
                    }
                    else if (step.ToLower().Contains("ui"))
                    {
                        plan.UIs.Add(new GameSystemGenerator.PlannedUI { Path = $"Assets/Bifrost/Runtime/{llmPlan.systemName}_UI.uxml", Template = "" });
                    }
                }
            }
            return plan;
        }

        private async void OnUserMessage(string message)
        {
            errorMessage = null;
            chatUI.SetProcessingState(true);
            lastUserMessage = message;
            try
            {
                // Special: 3D model generation from image
                if (message.ToLower().StartsWith("generate a ") && message.ToLower().Contains("from this image"))
                {
                    string imagePath = EditorUtility.OpenFilePanel("Select Reference Image", Application.dataPath, "png,jpg,jpeg");
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        chatUI.AddResponse("Generating 3D model from image...");
                        string prefabPath = await imageTo3DGenerator.Generate3DModelFromImageAsync(imagePath);
                        if (!string.IsNullOrEmpty(prefabPath))
                        {
                            chatUI.AddResponse($"3D model generated and imported as prefab: {prefabPath}");
                        }
                        else
                        {
                            chatUI.AddResponse("Failed to generate 3D model from image.");
                        }
                    }
                    else
                    {
                        chatUI.AddResponse("No image selected. Operation cancelled.");
                    }
                }
                else
                {
                    // General game system generation
                    chatUI.AddResponse("Analyzing request and planning actions...");
                    var llmPlan = await systemGenerator.PlanGameSystemAsync(message);
                    var plan = ConvertToGameSystemPlan(llmPlan);
                    if (plan != null && plan.Scripts.Count + plan.Prefabs.Count + plan.UIs.Count > 0)
                    {
                        pendingPlan = plan;
                        awaitingApproval = true;
                        chatUI.AddResponse("Planned actions ready. Please review and approve to apply changes.");
                    }
                    else
                    {
                        chatUI.AddResponse("AI could not generate a valid plan for your request.");
                    }
                }
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
                chatUI.AddResponse("An error occurred. See error message above.");
            }
            finally
            {
                chatUI.SetProcessingState(false);
            }
        }

        private async Task ApplyPlannedSystemAsync()
        {
            chatUI.SetProcessingState(true);
            errorMessage = null;
            try
            {
                bool success = await systemGenerator.ApplyGameSystemAsync(pendingPlan);
                if (success)
                {
                    chatUI.AddResponse("All planned changes applied successfully!");
                }
                else
                {
                    chatUI.AddResponse("Some changes could not be applied. See error log.");
                }
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Error: {ex.Message}";
                chatUI.AddResponse("An error occurred while applying changes.");
            }
            finally
            {
                awaitingApproval = false;
                pendingPlan = null;
                chatUI.SetProcessingState(false);
            }
        }
    }
}