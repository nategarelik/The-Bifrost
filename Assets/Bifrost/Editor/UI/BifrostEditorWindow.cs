using UnityEditor;
using UnityEngine;
using Bifrost.Editor.UI;
using Bifrost.Editor.Settings;
using Bifrost.Editor.Prompts;
using Bifrost.Editor.AI;
using Bifrost.Editor.AI.MCP;
using Bifrost.Editor.Context;
using Bifrost.Editor.AssetGeneration;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Bifrost.Editor.UI
{
    public class BifrostEditorWindow : EditorWindow
    {
        [SerializeField] private bool showOnboarding = false;
        private const string ONBOARDING_SHOWN_KEY = "Bifrost_OnboardingShown";
        private const string CURRENT_PLAN_KEY = "Bifrost_CurrentPlan";
        private const string AWAITING_APPROVAL_KEY = "Bifrost_AwaitingApproval";
        private const string SELECTED_MODE_KEY = "Bifrost_SelectedMode";

        private BifrostPromptLibraryUI promptLibraryUI;
        [SerializeField] private bool showSteamGuide = false;
        private PromptTemplateManager promptManager;
        private enum Tab { MCP, Chat, Settings, Modes, PromptLibrary, Debug }
        [SerializeField] private Tab currentTab = Tab.MCP;
        private string errorMessage = null;
        private BifrostSettingsUI settingsUI;
        private BifrostModeEditor modeEditor;
        private BifrostChatUI chatUI;
        private List<string> logMessages = new List<string>();
        private const int MaxLogMessages = 100;
        private BifrostAgent bifrostAgent;
        private UnityContextAnalyzer contextAnalyzer;
        [SerializeField] private string selectedMode = "GameDev";
        private List<string> availableModes = new List<string> { "Code", "Architect", "Designer", "GameDev" };
        private GameSystemGenerator systemGenerator;
        private UnityProjectManager projectManager;
        private LLMGameSystemPlan currentPlan;
        [SerializeField] private bool awaitingApproval = false;

        // MCP Server fields
        [SerializeField] private bool mcpServerRunning = false;
        [SerializeField] private int mcpServerPort = 8090;
        [SerializeField] private int mcpRequestTimeout = 10;
        private const string MCP_PORT_KEY = "Bifrost_MCP_Port";
        private const string MCP_TIMEOUT_KEY = "Bifrost_MCP_Timeout";
        private MCPServerEnhanced mcpServer;
        [SerializeField] private List<string> connectedClients = new List<string>();

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
            projectManager = new UnityProjectManager();
            systemGenerator = new GameSystemGenerator(projectManager, bifrostAgent);
            chatUI.OnMessageSent += OnUserMessageAsync;
            chatUI.OnClearChat += () => LogToPanel("Chat history cleared.");

            // Restore persistent state after recompilation
            RestoreState();

            if (!EditorPrefs.GetBool(ONBOARDING_SHOWN_KEY, false))
            {
                showOnboarding = true;
                EditorPrefs.SetBool(ONBOARDING_SHOWN_KEY, true);
            }
            LogToPanel("Bifrost Editor Window enabled.");
        }

        private void RestoreState()
        {
            // Restore selected mode
            selectedMode = EditorPrefs.GetString(SELECTED_MODE_KEY, "GameDev");

            // Restore MCP server settings
            mcpServerPort = EditorPrefs.GetInt(MCP_PORT_KEY, 8090);
            mcpRequestTimeout = EditorPrefs.GetInt(MCP_TIMEOUT_KEY, 10);

            // Restore approval state
            awaitingApproval = EditorPrefs.GetBool(AWAITING_APPROVAL_KEY, false);

            // Restore current plan if awaiting approval
            if (awaitingApproval)
            {
                string planJson = EditorPrefs.GetString(CURRENT_PLAN_KEY, "");
                if (!string.IsNullOrEmpty(planJson))
                {
                    try
                    {
                        currentPlan = JsonUtility.FromJson<LLMGameSystemPlan>(planJson);
                        LogToPanel($"Restored plan: {currentPlan?.systemName} (awaiting approval)");

                        // Add a message to chat indicating the plan was restored
                        if (chatUI != null && currentPlan != null)
                        {
                            chatUI.AddResponse($"📋 Plan restored after recompilation: {currentPlan.systemName}\n⚠️ Use the 'Apply Plan' button below to execute.");
                        }
                    }
                    catch (System.Exception ex)
                    {
                        LogToPanel($"Failed to restore plan: {ex.Message}");
                        awaitingApproval = false;
                        EditorPrefs.DeleteKey(CURRENT_PLAN_KEY);
                        EditorPrefs.SetBool(AWAITING_APPROVAL_KEY, false);
                    }
                }
                else
                {
                    awaitingApproval = false;
                    EditorPrefs.SetBool(AWAITING_APPROVAL_KEY, false);
                }
            }
        }

        private void SaveState()
        {
            // Save selected mode
            EditorPrefs.SetString(SELECTED_MODE_KEY, selectedMode);

            // Save approval state and plan
            EditorPrefs.SetBool(AWAITING_APPROVAL_KEY, awaitingApproval);

            if (awaitingApproval && currentPlan != null)
            {
                try
                {
                    string planJson = JsonUtility.ToJson(currentPlan);
                    EditorPrefs.SetString(CURRENT_PLAN_KEY, planJson);
                }
                catch (System.Exception ex)
                {
                    LogToPanel($"Failed to save plan: {ex.Message}");
                }
            }
            else
            {
                EditorPrefs.DeleteKey(CURRENT_PLAN_KEY);
            }
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
            awaitingApproval = false;
            SaveState(); // Save state immediately
            LogToPanel($"User message: {message} (Mode: {selectedMode})");
            try
            {
                string modePrompt = $"[Mode: {selectedMode}]\n{message}";
                var (llmPlan, rawResponse) = await bifrostAgent.PlanGameSystemAsync(modePrompt);
                if (llmPlan != null && llmPlan.steps != null && llmPlan.steps.Length > 0)
                {
                    currentPlan = llmPlan;
                    awaitingApproval = true;
                    SaveState(); // Save state with new plan
                    chatUI.AddResponse($"AI Plan: {llmPlan.systemName}\n{string.Join("\n", llmPlan.steps)}\n\n⚠️ Plan ready for review. Use the 'Apply Plan' button below to execute.");
                    LogToPanel($"AI plan received: {llmPlan.systemName} with {llmPlan.steps.Length} steps");
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

        private async void ApplyCurrentPlan()
        {
            if (currentPlan == null)
            {
                LogToPanel("No plan to apply.");
                return;
            }

            chatUI.SetProcessingState(true);
            LogToPanel($"Applying plan: {currentPlan.systemName}");
            chatUI.AddResponse($"🔨 Applying plan: {currentPlan.systemName}...");

            try
            {
                // Convert LLMGameSystemPlan to GameSystemPlan
                var gameSystemPlan = ConvertToGameSystemPlan(currentPlan);
                bool success = await systemGenerator.ApplyGameSystemAsync(gameSystemPlan);

                if (success)
                {
                    chatUI.AddResponse($"✅ Plan applied successfully! Check your scene and project for the new assets.");
                    LogToPanel($"Plan applied successfully: {currentPlan.systemName}");
                }
                else
                {
                    chatUI.AddResponse($"❌ Failed to apply plan. Check the Debug tab for details.");
                    LogToPanel($"Failed to apply plan: {currentPlan.systemName}");
                }
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Error applying plan: {ex.Message}";
                chatUI.AddResponse($"❌ Error applying plan: {ex.Message}");
                LogToPanel($"Error applying plan: {ex.Message}");
            }
            finally
            {
                chatUI.SetProcessingState(false);
                awaitingApproval = false;
                currentPlan = null;
                SaveState(); // Clear saved plan after applying
            }
        }

        private GameSystemPlan ConvertToGameSystemPlan(LLMGameSystemPlan llmPlan)
        {
            var plan = new GameSystemPlan();
            var createdObjects = new Dictionary<string, GameObject>();
            var createdMaterials = new Dictionary<string, Material>();

            if (llmPlan.steps != null)
            {
                foreach (var step in llmPlan.steps)
                {
                    if (string.IsNullOrWhiteSpace(step)) continue;

                    try
                    {
                        ParseAndExecuteStep(step, createdObjects, createdMaterials, plan);
                    }
                    catch (System.Exception ex)
                    {
                        LogToPanel($"Error parsing step: {step} - {ex.Message}");
                        // Add as a script comment if parsing fails
                        AddScriptStep(plan, llmPlan.systemName, step);
                    }
                }
            }

            return plan;
        }

        private void ParseAndExecuteStep(string step, Dictionary<string, GameObject> createdObjects, Dictionary<string, Material> createdMaterials, GameSystemPlan plan)
        {
            string stepLower = step.ToLower();

            // Parse GameObject creation (Cube, Plane, etc.)
            if (stepLower.Contains("create") && stepLower.Contains("3d object"))
            {
                CreateGameObjectFromStep(step, createdObjects);
            }
            // Parse Material creation
            else if (stepLower.Contains("create") && stepLower.Contains("material"))
            {
                CreateMaterialFromStep(step, createdMaterials);
            }
            // Parse Material application
            else if (stepLower.Contains("apply") && (stepLower.Contains("material") || stepLower.Contains("color")))
            {
                ApplyMaterialFromStep(step, createdObjects, createdMaterials);
            }
            // Parse positioning, scaling, rotation
            else if (stepLower.Contains("position") || stepLower.Contains("scale") || stepLower.Contains("rotate"))
            {
                ModifyGameObjectFromStep(step, createdObjects);
            }
            // Fallback: add as script for manual implementation
            else
            {
                AddScriptStep(plan, "ParsedInstructions", step);
            }
        }

        private void CreateGameObjectFromStep(string step, Dictionary<string, GameObject> createdObjects)
        {
            try
            {
                // Extract object type (Cube, Plane, etc.)
                PrimitiveType primitiveType = PrimitiveType.Cube; // default
                if (step.ToLower().Contains("plane")) primitiveType = PrimitiveType.Plane;
                else if (step.ToLower().Contains("sphere")) primitiveType = PrimitiveType.Sphere;
                else if (step.ToLower().Contains("cylinder")) primitiveType = PrimitiveType.Cylinder;
                else if (step.ToLower().Contains("capsule")) primitiveType = PrimitiveType.Capsule;

                // Extract name
                string objectName = ExtractNameFromStep(step);
                if (string.IsNullOrEmpty(objectName)) objectName = $"Generated_{primitiveType}_{createdObjects.Count}";

                // Create the GameObject
                GameObject go = GameObject.CreatePrimitive(primitiveType);
                go.name = objectName;

                // Extract and apply position - ALWAYS try to extract position
                Vector3 position = ExtractPositionFromStep(step);
                go.transform.position = position; // Apply the position even if it's Vector3.zero

                // Extract and apply scale if specified in this step
                Vector3 scale = ExtractScaleFromStep(step);
                if (scale != Vector3.one)
                {
                    go.transform.localScale = scale;
                }

                // Extract and apply rotation if specified in this step
                Vector3 rotation = ExtractRotationFromStep(step);
                if (rotation != Vector3.zero)
                {
                    go.transform.rotation = Quaternion.Euler(rotation);
                }

                createdObjects[objectName] = go;
                LogToPanel($"Created GameObject: {objectName} ({primitiveType}) at {position} with scale {scale}");

                // Register undo
                Undo.RegisterCreatedObjectUndo(go, $"Create {objectName}");
            }
            catch (System.Exception ex)
            {
                LogToPanel($"Error creating GameObject from step: {ex.Message}");
            }
        }

        private void CreateMaterialFromStep(string step, Dictionary<string, Material> createdMaterials)
        {
            try
            {
                string materialName = ExtractNameFromStep(step);
                if (string.IsNullOrEmpty(materialName)) materialName = $"Generated_Material_{createdMaterials.Count}";

                Material mat = new Material(Shader.Find("Standard"));
                mat.name = materialName;

                // Extract color from step
                Color color = ExtractColorFromStep(step);
                if (color != Color.clear)
                {
                    mat.color = color;
                }

                createdMaterials[materialName] = mat;
                LogToPanel($"Created Material: {materialName} with color {color}");

                // Save material as asset
                string materialPath = $"Assets/Bifrost/Runtime/Generated/Materials/{materialName}.mat";
                System.IO.Directory.CreateDirectory(System.IO.Path.GetDirectoryName(materialPath));
                AssetDatabase.CreateAsset(mat, materialPath);
            }
            catch (System.Exception ex)
            {
                LogToPanel($"Error creating Material from step: {ex.Message}");
            }
        }

        private void ApplyMaterialFromStep(string step, Dictionary<string, GameObject> createdObjects, Dictionary<string, Material> createdMaterials)
        {
            try
            {
                // Find material name or color in step
                Material materialToApply = null;

                // Look for existing material by name
                foreach (var matName in createdMaterials.Keys)
                {
                    if (step.ToLower().Contains(matName.ToLower()))
                    {
                        materialToApply = createdMaterials[matName];
                        break;
                    }
                }

                // If no material found, create one from color description
                if (materialToApply == null)
                {
                    Color color = ExtractColorFromStep(step);
                    if (color != Color.clear)
                    {
                        materialToApply = new Material(Shader.Find("Standard"));
                        materialToApply.color = color;
                        materialToApply.name = $"Auto_{color}";
                    }
                }

                if (materialToApply != null)
                {
                    // Find target object(s)
                    foreach (var objName in createdObjects.Keys)
                    {
                        if (step.ToLower().Contains(objName.ToLower()))
                        {
                            var renderer = createdObjects[objName].GetComponent<Renderer>();
                            if (renderer != null)
                            {
                                renderer.material = materialToApply;
                                LogToPanel($"Applied material {materialToApply.name} to {objName}");
                            }
                        }
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogToPanel($"Error applying material from step: {ex.Message}");
            }
        }

        private void ModifyGameObjectFromStep(string step, Dictionary<string, GameObject> createdObjects)
        {
            try
            {
                // Find target object
                GameObject targetObject = null;
                foreach (var objName in createdObjects.Keys)
                {
                    if (step.ToLower().Contains(objName.ToLower()))
                    {
                        targetObject = createdObjects[objName];
                        break;
                    }
                }

                if (targetObject != null)
                {
                    if (step.ToLower().Contains("position"))
                    {
                        Vector3 position = ExtractPositionFromStep(step);
                        targetObject.transform.position = position;
                        LogToPanel($"Set position of {targetObject.name} to {position}");
                    }

                    if (step.ToLower().Contains("scale"))
                    {
                        Vector3 scale = ExtractScaleFromStep(step);
                        targetObject.transform.localScale = scale;
                        LogToPanel($"Set scale of {targetObject.name} to {scale}");
                    }

                    if (step.ToLower().Contains("rotate"))
                    {
                        Vector3 rotation = ExtractRotationFromStep(step);
                        targetObject.transform.rotation = Quaternion.Euler(rotation);
                        LogToPanel($"Set rotation of {targetObject.name} to {rotation}");
                    }
                }
            }
            catch (System.Exception ex)
            {
                LogToPanel($"Error modifying GameObject from step: {ex.Message}");
            }
        }

        private string ExtractNameFromStep(string step)
        {
            // Look for patterns like "name it 'ObjectName'" or "named 'ObjectName'"
            var namePatterns = new string[] { "name it '", "named '", "call it '", "name it \"", "named \"" };

            foreach (var pattern in namePatterns)
            {
                int startIndex = step.IndexOf(pattern, System.StringComparison.OrdinalIgnoreCase);
                if (startIndex >= 0)
                {
                    startIndex += pattern.Length;
                    char endChar = pattern.Contains("'") ? '\'' : '"';
                    int endIndex = step.IndexOf(endChar, startIndex);
                    if (endIndex > startIndex)
                    {
                        return step.Substring(startIndex, endIndex - startIndex);
                    }
                }
            }

            return "";
        }

        private Vector3 ExtractPositionFromStep(string step)
        {
            // Look for patterns like "Position it at (0, 0, 0)" or "at (x, y, z)" with more flexible spacing and decimal support
            var patterns = new string[]
            {
                @"position\s+it\s+at\s*\(\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*\)",
                @"position.*?at\s*\(\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*\)",
                @"\bat\s*\(\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*\)",
                @"\(\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*\)"
            };

            foreach (var pattern in patterns)
            {
                var match = System.Text.RegularExpressions.Regex.Match(step, pattern, System.Text.RegularExpressions.RegexOptions.IgnoreCase);
                if (match.Success)
                {
                    if (float.TryParse(match.Groups[1].Value, out float x) &&
                        float.TryParse(match.Groups[2].Value, out float y) &&
                        float.TryParse(match.Groups[3].Value, out float z))
                    {
                        Vector3 position = new Vector3(x, y, z);
                        LogToPanel($"Extracted position: {position} from step: {step.Substring(0, Mathf.Min(60, step.Length))}...");
                        return position;
                    }
                }
            }

            LogToPanel($"No position found in step: {step.Substring(0, Mathf.Min(60, step.Length))}...");
            return Vector3.zero;
        }

        private Vector3 ExtractScaleFromStep(string step)
        {
            // Look for patterns like "scale it to (10, 8, 15)"
            var match = System.Text.RegularExpressions.Regex.Match(step, @"scale.*?\(\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*,\s*(-?\d+(?:\.\d+)?)\s*\)", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success)
            {
                if (float.TryParse(match.Groups[1].Value, out float x) &&
                    float.TryParse(match.Groups[2].Value, out float y) &&
                    float.TryParse(match.Groups[3].Value, out float z))
                {
                    return new Vector3(x, y, z);
                }
            }
            return Vector3.one;
        }

        private Vector3 ExtractRotationFromStep(string step)
        {
            // Look for patterns like "Rotate it -15 degrees on the X-axis"
            var match = System.Text.RegularExpressions.Regex.Match(step, @"rotate.*?(-?\d+(?:\.\d+)?)\s*degrees.*?(x|y|z)-axis", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            if (match.Success)
            {
                if (float.TryParse(match.Groups[1].Value, out float degrees))
                {
                    string axis = match.Groups[2].Value.ToLower();
                    switch (axis)
                    {
                        case "x": return new Vector3(degrees, 0, 0);
                        case "y": return new Vector3(0, degrees, 0);
                        case "z": return new Vector3(0, 0, degrees);
                    }
                }
            }
            return Vector3.zero;
        }

        private Color ExtractColorFromStep(string step)
        {
            string stepLower = step.ToLower();

            // Common color mappings
            if (stepLower.Contains("white")) return Color.white;
            if (stepLower.Contains("red")) return Color.red;
            if (stepLower.Contains("green")) return Color.green;
            if (stepLower.Contains("blue")) return Color.blue;
            if (stepLower.Contains("yellow")) return Color.yellow;
            if (stepLower.Contains("black")) return Color.black;
            if (stepLower.Contains("gray") || stepLower.Contains("grey")) return Color.gray;
            if (stepLower.Contains("cyan")) return Color.cyan;
            if (stepLower.Contains("magenta")) return Color.magenta;

            return Color.clear; // No color found
        }

        private void AddScriptStep(GameSystemPlan plan, string systemName, string step)
        {
            string stepFileName = $"{systemName}_ManualStep_{plan.Scripts.Count + 1}.cs";
            string stepPath = $"Assets/Bifrost/Runtime/Generated/{stepFileName}";
            string stepContent = $"// Manual implementation required: {step}\n// TODO: Implement this step manually.";

            plan.Scripts.Add(new PlannedScript
            {
                Path = stepPath,
                Content = stepContent
            });
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

            // Show plan approval UI if awaiting approval
            if (awaitingApproval && currentPlan != null)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField($"Plan Ready: {currentPlan.systemName}", EditorStyles.boldLabel);
                EditorGUILayout.LabelField($"Steps: {currentPlan.steps.Length}");
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("✅ Apply Plan", GUILayout.Height(30)))
                {
                    ApplyCurrentPlan();
                }
                if (GUILayout.Button("❌ Cancel", GUILayout.Height(30)))
                {
                    awaitingApproval = false;
                    currentPlan = null;
                    SaveState(); // Save state when plan is cancelled
                    chatUI.AddResponse("Plan cancelled.");
                    LogToPanel("Plan cancelled by user.");
                }
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            switch (currentTab)
            {
                case Tab.MCP:
                    DrawMCPTab();
                    break;
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
            if (GUILayout.Toggle(currentTab == Tab.MCP, new GUIContent("MCP Server", "Configure external MCP connections"), EditorStyles.toolbarButton)) currentTab = Tab.MCP;
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
                        SaveState(); // Save state when mode changes
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

        private void DrawMCPTab()
        {
            EditorGUILayout.LabelField("🔌 MCP Server Configuration", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Server Status Section
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Server Status", EditorStyles.boldLabel);

            string statusText = mcpServerRunning ? "✅ Running" : "❌ Stopped";
            Color originalColor = GUI.color;
            GUI.color = mcpServerRunning ? Color.green : Color.red;
            EditorGUILayout.LabelField($"Status: {statusText}");
            GUI.color = originalColor;

            if (mcpServerRunning)
            {
                EditorGUILayout.LabelField($"Port: {mcpServerPort}");
                EditorGUILayout.LabelField($"WebSocket URL: ws://localhost:{mcpServerPort}");
                EditorGUILayout.LabelField($"Connected Clients: {connectedClients.Count}");

                if (connectedClients.Count > 0)
                {
                    EditorGUILayout.LabelField("Client IDs:", EditorStyles.boldLabel);
                    foreach (string clientId in connectedClients)
                    {
                        EditorGUILayout.LabelField($"  • {clientId.Substring(0, 8)}...");
                    }
                }
            }
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // Configuration Section
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Server Configuration", EditorStyles.boldLabel);

            // Port Configuration
            EditorGUI.BeginChangeCheck();
            int newPort = EditorGUILayout.IntField("WebSocket Port", mcpServerPort);
            if (EditorGUI.EndChangeCheck())
            {
                mcpServerPort = Mathf.Clamp(newPort, 1024, 65535);
                EditorPrefs.SetInt(MCP_PORT_KEY, mcpServerPort);
                LogToPanel($"MCP Server port changed to: {mcpServerPort}");
            }

            // Timeout Configuration
            EditorGUI.BeginChangeCheck();
            int newTimeout = EditorGUILayout.IntField("Request Timeout (seconds)", mcpRequestTimeout);
            if (EditorGUI.EndChangeCheck())
            {
                mcpRequestTimeout = Mathf.Clamp(newTimeout, 5, 120);
                EditorPrefs.SetInt(MCP_TIMEOUT_KEY, mcpRequestTimeout);
                LogToPanel($"MCP Server timeout changed to: {mcpRequestTimeout}s");
            }

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // Server Control Section
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Server Control", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            if (!mcpServerRunning)
            {
                if (GUILayout.Button("🚀 Start MCP Server", GUILayout.Height(30)))
                {
                    StartMCPServer();
                }
            }
            else
            {
                if (GUILayout.Button("🛑 Stop MCP Server", GUILayout.Height(30)))
                {
                    StopMCPServer();
                }
            }

            if (GUILayout.Button("🔄 Restart Server", GUILayout.Height(30)))
            {
                RestartMCPServer();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // Client Configuration Section
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("🤖 AI Client Configuration", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("Configure your AI tools to connect to Bifrost:", EditorStyles.wordWrappedLabel);
            EditorGUILayout.Space();

            // Claude Desktop Configuration
            if (GUILayout.Button("📋 Copy Claude Desktop Config"))
            {
                CopyClaudeConfig();
            }

            EditorGUILayout.Space();

            // Cursor/Windsurf Configuration  
            if (GUILayout.Button("📋 Copy Cursor/Windsurf Config"))
            {
                CopyCursorConfig();
            }

            EditorGUILayout.Space();

            // Manual Configuration
            EditorGUILayout.LabelField("Manual Configuration:", EditorStyles.boldLabel);
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.SelectableLabel($"WebSocket URL: ws://localhost:{mcpServerPort}", EditorStyles.textField);
            EditorGUILayout.SelectableLabel($"Protocol: MCP over WebSocket", EditorStyles.textField);
            EditorGUILayout.EndVertical();

            EditorGUILayout.EndVertical();

            EditorGUILayout.Space();

            // Available Tools Section
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("🔧 Available MCP Tools", EditorStyles.boldLabel);
            EditorGUILayout.LabelField("External AI tools can use these functions:", EditorStyles.wordWrappedLabel);

            string[] tools = {
                "• execute_menu_item - Execute Unity menu commands",
                "• select_gameobject - Select objects in hierarchy",
                "• update_gameobject - Modify GameObject properties",
                "• update_component - Add/modify components",
                "• add_package - Install Unity packages",
                "• run_tests - Execute Unity tests",
                "• send_console_log - Send messages to Unity console",
                "• add_asset_to_scene - Add prefabs/assets to scene"
            };

            foreach (string tool in tools)
            {
                EditorGUILayout.LabelField(tool, EditorStyles.wordWrappedLabel);
            }
            EditorGUILayout.EndVertical();
        }

        private void StartMCPServer()
        {
            if (mcpServer != null && mcpServer.IsRunning)
            {
                LogToPanel("MCP Server is already running");
                return;
            }

            try
            {
                mcpServer = new MCPServerEnhanced(mcpServerPort);

                // Subscribe to events
                mcpServer.OnLog += LogToPanel;
                mcpServer.OnError += (error) =>
                {
                    LogToPanel($"MCP Error: {error}");
                    errorMessage = error;
                };
                mcpServer.OnClientConnected += (clientId) =>
                {
                    connectedClients.Add(clientId);
                    LogToPanel($"MCP Client connected: {clientId}");
                };
                mcpServer.OnClientDisconnected += (clientId) =>
                {
                    connectedClients.Remove(clientId);
                    LogToPanel($"MCP Client disconnected: {clientId}");
                };

                mcpServer.Start();
                mcpServerRunning = mcpServer.IsRunning;

                if (mcpServerRunning)
                {
                    LogToPanel($"✅ MCP Server started successfully on port {mcpServerPort}");
                    errorMessage = null;
                }
            }
            catch (System.Exception ex)
            {
                errorMessage = $"Failed to start MCP Server: {ex.Message}";
                LogToPanel(errorMessage);
                mcpServerRunning = false;
            }
        }

        private void StopMCPServer()
        {
            if (mcpServer != null)
            {
                try
                {
                    mcpServer.Stop();
                    mcpServerRunning = false;
                    connectedClients.Clear();
                    LogToPanel("🛑 MCP Server stopped");

                    // Unsubscribe from events
                    mcpServer.OnLog -= LogToPanel;
                    mcpServer.OnError -= (error) =>
                    {
                        LogToPanel($"MCP Error: {error}");
                        errorMessage = error;
                    };
                    mcpServer.OnClientConnected -= (clientId) =>
                    {
                        connectedClients.Add(clientId);
                        LogToPanel($"MCP Client connected: {clientId}");
                    };
                    mcpServer.OnClientDisconnected -= (clientId) =>
                    {
                        connectedClients.Remove(clientId);
                        LogToPanel($"MCP Client disconnected: {clientId}");
                    };

                    mcpServer = null;
                }
                catch (System.Exception ex)
                {
                    LogToPanel($"Error stopping MCP Server: {ex.Message}");
                }
            }
            else
            {
                mcpServerRunning = false;
                LogToPanel("MCP Server was not running");
            }
        }

        private void RestartMCPServer()
        {
            StopMCPServer();
            // Give it a moment to fully stop, then start
            EditorApplication.delayCall += () => StartMCPServer();
        }

        private void CopyClaudeConfig()
        {
            string config = $@"{{
  ""mcpServers"": {{
    ""bifrost-unity"": {{
      ""command"": ""ws://localhost:{mcpServerPort}"",
      ""transport"": ""websocket""
    }}
  }}
}}";
            EditorGUIUtility.systemCopyBuffer = config;
            LogToPanel("Claude Desktop configuration copied to clipboard");
            EditorUtility.DisplayDialog("Configuration Copied",
                "Claude Desktop configuration has been copied to your clipboard.\n\n" +
                "Add this to your claude_desktop_config.json file:\n" +
                "• macOS: ~/Library/Application Support/Claude/claude_desktop_config.json\n" +
                "• Windows: %APPDATA%/Claude/claude_desktop_config.json", "OK");
        }

        private void CopyCursorConfig()
        {
            string config = $@"{{
  ""mcp"": {{
    ""servers"": {{
      ""bifrost-unity"": {{
        ""url"": ""ws://localhost:{mcpServerPort}"",
        ""transport"": ""websocket""
      }}
    }}
  }}
}}";
            EditorGUIUtility.systemCopyBuffer = config;
            LogToPanel("Cursor/Windsurf configuration copied to clipboard");
            EditorUtility.DisplayDialog("Configuration Copied",
                "Cursor/Windsurf configuration has been copied to your clipboard.\n\n" +
                "Add this to your MCP settings in your IDE.", "OK");
        }

        private void OnDestroy()
        {
            // Cleanup MCP server when window is destroyed
            if (mcpServer != null && mcpServer.IsRunning)
            {
                LogToPanel("Cleaning up MCP Server on window close");
                StopMCPServer();
            }
        }
    }
}