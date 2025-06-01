using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEditor;
using Bifrost.Editor.AI.MCP;
using Bifrost.Editor.AI.Unity;
using Bifrost.Editor.Settings;

namespace Bifrost.Editor.UI
{
    public class BifrostMCPWindow : EditorWindow
    {
        private MCPServerEnhanced mcpServer;
        private UnityStateSynchronizer stateSynchronizer;
        private BifrostSettings settings;
        
        private Vector2 scrollPosition;
        private int selectedTab = 0;
        private readonly string[] tabNames = { "MCP Server", "Tools", "Resources", "Agent", "Debug" };
        
        // MCP Server Tab
        private int serverPort = 8090;
        private List<string> connectedClients = new List<string>();
        private List<string> serverLogs = new List<string>();
        private Vector2 logsScrollPosition;
        
        // Tools Tab
        private Vector2 toolsScrollPosition;
        private string toolSearchFilter = "";
        
        // Resources Tab
        private Vector2 resourcesScrollPosition;
        
        // Agent Tab
        private string agentGoal = "";
        private List<string> agentActivities = new List<string>();
        
        // Debug Tab
        private bool showDetailedLogs = false;
        private int mainThreadQueueCount = 0;
        private int backgroundQueueCount = 0;

        [MenuItem("Window/Bifrost/MCP Control Panel")]
        public static void ShowWindow()
        {
            var window = GetWindow<BifrostMCPWindow>("Bifrost MCP");
            window.minSize = new Vector2(600, 400);
        }

        private void OnEnable()
        {
            settings = BifrostSettings.LoadOrCreateSettings();
            serverPort = EditorPrefs.GetInt("BifrostMCP_ServerPort", 8090);
        }

        private void OnDisable()
        {
            if (mcpServer != null && mcpServer.IsRunning)
            {
                StopServer();
            }
        }

        private void OnGUI()
        {
            DrawHeader();
            DrawTabs();
            
            switch (selectedTab)
            {
                case 0: DrawMCPServerTab(); break;
                case 1: DrawToolsTab(); break;
                case 2: DrawResourcesTab(); break;
                case 3: DrawAgentTab(); break;
                case 4: DrawDebugTab(); break;
            }
        }

        private void DrawHeader()
        {
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            GUILayout.Label("Bifrost MCP Control Panel", EditorStyles.boldLabel);
            GUILayout.FlexibleSpace();
            
            if (mcpServer != null && mcpServer.IsRunning)
            {
                GUILayout.Label("● Server Running", new GUIStyle(EditorStyles.label) { normal = { textColor = Color.green } });
            }
            else
            {
                GUILayout.Label("● Server Stopped", new GUIStyle(EditorStyles.label) { normal = { textColor = Color.red } });
            }
            
            EditorGUILayout.EndHorizontal();
        }

        private void DrawTabs()
        {
            selectedTab = GUILayout.Toolbar(selectedTab, tabNames);
            EditorGUILayout.Space();
        }

        private void DrawMCPServerTab()
        {
            EditorGUILayout.BeginVertical("box");
            
            // Server Controls
            EditorGUILayout.LabelField("Server Configuration", EditorStyles.boldLabel);
            
            EditorGUI.BeginDisabledGroup(mcpServer != null && mcpServer.IsRunning);
            serverPort = EditorGUILayout.IntField("Port", serverPort);
            EditorGUI.EndDisabledGroup();
            
            EditorGUILayout.BeginHorizontal();
            if (mcpServer == null || !mcpServer.IsRunning)
            {
                if (GUILayout.Button("Start Server", GUILayout.Height(30)))
                {
                    StartServer();
                }
            }
            else
            {
                if (GUILayout.Button("Stop Server", GUILayout.Height(30)))
                {
                    StopServer();
                }
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();
            
            // Connected Clients
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField($"Connected Clients ({connectedClients.Count})", EditorStyles.boldLabel);
            
            if (connectedClients.Count > 0)
            {
                foreach (var client in connectedClients)
                {
                    EditorGUILayout.BeginHorizontal();
                    GUILayout.Label("• " + client);
                    if (GUILayout.Button("Disconnect", GUILayout.Width(80)))
                    {
                        // TODO: Implement client disconnect
                    }
                    EditorGUILayout.EndHorizontal();
                }
            }
            else
            {
                EditorGUILayout.HelpBox("No clients connected", MessageType.Info);
            }
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();
            
            // Server Logs
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Server Logs", EditorStyles.boldLabel);
            if (GUILayout.Button("Clear", GUILayout.Width(50)))
            {
                serverLogs.Clear();
            }
            EditorGUILayout.EndHorizontal();
            
            logsScrollPosition = EditorGUILayout.BeginScrollView(logsScrollPosition, GUILayout.Height(150));
            foreach (var log in serverLogs.TakeLast(50))
            {
                GUILayout.Label(log, EditorStyles.miniLabel);
            }
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }

        private void DrawToolsTab()
        {
            if (mcpServer == null)
            {
                EditorGUILayout.HelpBox("Start the MCP server to view available tools", MessageType.Warning);
                return;
            }
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Search:", GUILayout.Width(50));
            toolSearchFilter = EditorGUILayout.TextField(toolSearchFilter);
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.Space();
            
            toolsScrollPosition = EditorGUILayout.BeginScrollView(toolsScrollPosition);
            
            var tools = mcpServer.ToolRegistry.GetAllTools();
            var filteredTools = string.IsNullOrEmpty(toolSearchFilter) ? tools :
                tools.Where(t => t.Name.Contains(toolSearchFilter, StringComparison.OrdinalIgnoreCase) ||
                                t.Description.Contains(toolSearchFilter, StringComparison.OrdinalIgnoreCase));
            
            foreach (var tool in filteredTools)
            {
                EditorGUILayout.BeginVertical("box");
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(tool.Name, EditorStyles.boldLabel);
                if (GUILayout.Button("Test", GUILayout.Width(50)))
                {
                    TestTool(tool.Name);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.LabelField(tool.Description, EditorStyles.wordWrappedLabel);
                
                if (tool.InputSchema != null)
                {
                    EditorGUI.indentLevel++;
                    EditorGUILayout.LabelField("Parameters:", EditorStyles.miniLabel);
                    EditorGUILayout.TextArea(tool.InputSchema.ToString(Newtonsoft.Json.Formatting.Indented), 
                        GUILayout.MaxHeight(100));
                    EditorGUI.indentLevel--;
                }
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            
            EditorGUILayout.EndScrollView();
        }

        private void DrawResourcesTab()
        {
            if (mcpServer == null)
            {
                EditorGUILayout.HelpBox("Start the MCP server to view available resources", MessageType.Warning);
                return;
            }
            
            resourcesScrollPosition = EditorGUILayout.BeginScrollView(resourcesScrollPosition);
            
            var resources = mcpServer.ResourceRegistry.GetAllResources();
            
            foreach (var resource in resources)
            {
                EditorGUILayout.BeginVertical("box");
                
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(resource.Name, EditorStyles.boldLabel);
                if (GUILayout.Button("Read", GUILayout.Width(50)))
                {
                    ReadResource(resource.Uri);
                }
                EditorGUILayout.EndHorizontal();
                
                EditorGUILayout.LabelField($"URI: {resource.Uri}", EditorStyles.miniLabel);
                EditorGUILayout.LabelField(resource.Description, EditorStyles.wordWrappedLabel);
                
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }
            
            EditorGUILayout.EndScrollView();
        }

        private void DrawAgentTab()
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Autonomous Agent", EditorStyles.boldLabel);
            
            EditorGUILayout.LabelField("Goal:");
            agentGoal = EditorGUILayout.TextArea(agentGoal, GUILayout.Height(60));
            
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Execute Goal", GUILayout.Height(30)))
            {
                ExecuteAgentGoal();
            }
            if (GUILayout.Button("Stop Agent", GUILayout.Height(30)))
            {
                // TODO: Implement agent stop
            }
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();
            
            // Agent Activity
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Agent Activity", EditorStyles.boldLabel);
            
            var activityScroll = EditorGUILayout.BeginScrollView(Vector2.zero, GUILayout.Height(200));
            foreach (var activity in agentActivities.TakeLast(20))
            {
                GUILayout.Label(activity, EditorStyles.miniLabel);
            }
            EditorGUILayout.EndScrollView();
            
            EditorGUILayout.EndVertical();
        }

        private void DrawDebugTab()
        {
            // Operation Queue Status
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.LabelField("Operation Queue Status", EditorStyles.boldLabel);
            
            var queue = UnityOperationQueue.Instance;
            EditorGUILayout.LabelField($"Main Thread Queue: {queue.MainThreadQueueCount} operations");
            EditorGUILayout.LabelField($"Background Queue: {queue.BackgroundQueueCount} operations");
            
            if (GUILayout.Button("Clear All Queues"))
            {
                queue.ClearQueues();
            }
            
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.Space();
            
            // State Synchronizer Status
            if (stateSynchronizer != null)
            {
                EditorGUILayout.BeginVertical("box");
                EditorGUILayout.LabelField("State Synchronizer", EditorStyles.boldLabel);
                EditorGUILayout.LabelField("Status: Active", EditorStyles.miniLabel);
                EditorGUILayout.EndVertical();
            }
            
            // Detailed Logs Toggle
            showDetailedLogs = EditorGUILayout.Toggle("Show Detailed Logs", showDetailedLogs);
        }

        private void StartServer()
        {
            EditorPrefs.SetInt("BifrostMCP_ServerPort", serverPort);
            
            mcpServer = new MCPServerEnhanced(serverPort);
            mcpServer.OnClientConnected += OnClientConnected;
            mcpServer.OnClientDisconnected += OnClientDisconnected;
            mcpServer.OnLog += OnServerLog;
            mcpServer.OnError += OnServerError;
            
            mcpServer.Start();
            
            // Initialize state synchronizer
            stateSynchronizer = new UnityStateSynchronizer(mcpServer);
            stateSynchronizer.Initialize();
            
            // Register built-in resources
            RegisterBuiltInResources();
        }

        private void StopServer()
        {
            if (stateSynchronizer != null)
            {
                stateSynchronizer.Shutdown();
                stateSynchronizer = null;
            }
            
            if (mcpServer != null)
            {
                mcpServer.Stop();
                mcpServer = null;
            }
            
            connectedClients.Clear();
        }

        private void RegisterBuiltInResources()
        {
            // Resources are automatically registered in the ResourceRegistry constructor
            OnServerLog($"Registered {mcpServer.ResourceRegistry.GetAllResources().Count()} resources");
        }

        private void OnClientConnected(string clientId)
        {
            connectedClients.Add(clientId);
            Repaint();
        }

        private void OnClientDisconnected(string clientId)
        {
            connectedClients.Remove(clientId);
            Repaint();
        }

        private void OnServerLog(string message)
        {
            serverLogs.Add($"[{DateTime.Now:HH:mm:ss}] {message}");
            Repaint();
        }

        private void OnServerError(string error)
        {
            serverLogs.Add($"[{DateTime.Now:HH:mm:ss}] ERROR: {error}");
            Debug.LogError($"MCP Server Error: {error}");
            Repaint();
        }

        private async void TestTool(string toolName)
        {
            try
            {
                var tool = mcpServer.ToolRegistry.GetTool(toolName);
                if (tool != null)
                {
                    var testArgs = new Newtonsoft.Json.Linq.JObject();
                    var context = new MCPExecutionContext
                    {
                        ClientId = "test-client",
                        RequestTime = DateTime.Now
                    };
                    
                    var result = await tool.Execute(testArgs, context);
                    Debug.Log($"Tool test result: {result.Content[0].Text}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Tool test failed: {ex.Message}");
            }
        }

        private async void ReadResource(string uri)
        {
            try
            {
                var resource = mcpServer.ResourceRegistry.GetResource(uri);
                if (resource != null)
                {
                    var result = await resource.Read(new MCPResourceReadParams { Uri = uri });
                    Debug.Log($"Resource content: {result.Contents[0].Text}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Resource read failed: {ex.Message}");
            }
        }

        private void ExecuteAgentGoal()
        {
            if (string.IsNullOrEmpty(agentGoal))
            {
                EditorUtility.DisplayDialog("Error", "Please enter a goal for the agent", "OK");
                return;
            }
            
            agentActivities.Add($"[{DateTime.Now:HH:mm:ss}] Starting goal: {agentGoal}");
            // TODO: Implement agent execution
            Debug.Log($"Executing agent goal: {agentGoal}");
        }
    }
}