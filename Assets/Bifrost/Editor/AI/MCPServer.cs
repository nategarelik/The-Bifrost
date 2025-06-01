using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bifrost.Editor.AI
{
    public class MCPServer
    {
        private HttpListener httpListener;
        private CancellationTokenSource cancellationTokenSource;
        private bool isRunning = false;
        private int port;
        private int requestTimeout;

        public event Action<string> OnClientConnected;
        public event Action<string> OnClientDisconnected;
        public event Action<string> OnError;
        public event Action<string> OnLog;

        public bool IsRunning => isRunning;

        public MCPServer(int port = 8090, int timeoutSeconds = 10)
        {
            this.port = port;
            this.requestTimeout = timeoutSeconds;
        }

        public async Task StartAsync()
        {
            if (isRunning) return;

            try
            {
                httpListener = new HttpListener();
                httpListener.Prefixes.Add($"http://localhost:{port}/");
                httpListener.Start();

                cancellationTokenSource = new CancellationTokenSource();
                isRunning = true;

                OnLog?.Invoke($"MCP Server started on http://localhost:{port}");

                // Start listening for WebSocket connections
                _ = Task.Run(() => AcceptWebSocketConnectionsAsync(cancellationTokenSource.Token));
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Failed to start MCP server: {ex.Message}");
                isRunning = false;
            }
        }

        public void Stop()
        {
            if (!isRunning) return;

            try
            {
                cancellationTokenSource?.Cancel();
                httpListener?.Stop();
                isRunning = false;
                OnLog?.Invoke("MCP Server stopped");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error stopping MCP server: {ex.Message}");
            }
        }

        private async Task AcceptWebSocketConnectionsAsync(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested && httpListener.IsListening)
            {
                try
                {
                    HttpListenerContext context = await httpListener.GetContextAsync();

                    if (context.Request.IsWebSocketRequest)
                    {
                        ProcessWebSocketRequest(context, cancellationToken);
                    }
                    else
                    {
                        context.Response.StatusCode = 400;
                        context.Response.Close();
                    }
                }
                catch (Exception ex)
                {
                    if (!cancellationToken.IsCancellationRequested)
                    {
                        OnError?.Invoke($"Error accepting connections: {ex.Message}");
                    }
                }
            }
        }

        private async void ProcessWebSocketRequest(HttpListenerContext context, CancellationToken cancellationToken)
        {
            WebSocketContext webSocketContext = null;
            WebSocket webSocket = null;

            try
            {
                webSocketContext = await context.AcceptWebSocketAsync(null);
                webSocket = webSocketContext.WebSocket;

                string clientId = Guid.NewGuid().ToString();
                OnClientConnected?.Invoke(clientId);
                OnLog?.Invoke($"Client connected: {clientId}");

                await HandleWebSocketConnection(webSocket, clientId, cancellationToken);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"WebSocket error: {ex.Message}");
            }
            finally
            {
                if (webSocket != null && webSocket.State == WebSocketState.Open)
                {
                    await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Connection closed", CancellationToken.None);
                }
            }
        }

        private async Task HandleWebSocketConnection(WebSocket webSocket, string clientId, CancellationToken cancellationToken)
        {
            var buffer = new byte[4096];

            try
            {
                while (webSocket.State == WebSocketState.Open && !cancellationToken.IsCancellationRequested)
                {
                    WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);

                    if (result.MessageType == WebSocketMessageType.Text)
                    {
                        string message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                        await ProcessMCPMessage(webSocket, message, clientId);
                    }
                    else if (result.MessageType == WebSocketMessageType.Close)
                    {
                        OnClientDisconnected?.Invoke(clientId);
                        OnLog?.Invoke($"Client disconnected: {clientId}");
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Connection error with {clientId}: {ex.Message}");
                OnClientDisconnected?.Invoke(clientId);
            }
        }

        private async Task ProcessMCPMessage(WebSocket webSocket, string message, string clientId)
        {
            try
            {
                OnLog?.Invoke($"Received from {clientId}: {message}");

                JObject request = JObject.Parse(message);
                string method = request["method"]?.ToString();
                JToken paramsObj = request["params"];
                string id = request["id"]?.ToString();

                JObject response = new JObject();
                response["jsonrpc"] = "2.0";
                response["id"] = id;

                switch (method)
                {
                    case "initialize":
                        response["result"] = await HandleInitialize(paramsObj);
                        break;

                    case "tools/list":
                        response["result"] = await HandleToolsList();
                        break;

                    case "tools/call":
                        response["result"] = await HandleToolCall(paramsObj);
                        break;

                    case "resources/list":
                        response["result"] = await HandleResourcesList();
                        break;

                    case "resources/read":
                        response["result"] = await HandleResourceRead(paramsObj);
                        break;

                    default:
                        response["error"] = new JObject
                        {
                            ["code"] = -32601,
                            ["message"] = $"Method not found: {method}"
                        };
                        break;
                }

                string responseJson = response.ToString();
                byte[] responseBytes = Encoding.UTF8.GetBytes(responseJson);
                await webSocket.SendAsync(new ArraySegment<byte>(responseBytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error processing message: {ex.Message}");
            }
        }

        private async Task<JObject> HandleInitialize(JToken paramsObj)
        {
            await Task.CompletedTask; // Make it async

            return new JObject
            {
                ["protocolVersion"] = "2024-11-05",
                ["capabilities"] = new JObject
                {
                    ["tools"] = new JObject(),
                    ["resources"] = new JObject()
                },
                ["serverInfo"] = new JObject
                {
                    ["name"] = "Bifrost Unity MCP Server",
                    ["version"] = "1.0.0"
                }
            };
        }

        private async Task<JObject> HandleToolsList()
        {
            await Task.CompletedTask; // Make it async

            var tools = new JArray();

            // Add basic Unity tools
            tools.Add(new JObject
            {
                ["name"] = "execute_menu_item",
                ["description"] = "Execute Unity menu items",
                ["inputSchema"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["menuPath"] = new JObject
                        {
                            ["type"] = "string",
                            ["description"] = "Menu item path (e.g., 'GameObject/Create Empty')"
                        }
                    },
                    ["required"] = new JArray { "menuPath" }
                }
            });

            tools.Add(new JObject
            {
                ["name"] = "select_gameobject",
                ["description"] = "Select GameObjects in the hierarchy",
                ["inputSchema"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["objectPath"] = new JObject
                        {
                            ["type"] = "string",
                            ["description"] = "Path to the GameObject in hierarchy"
                        }
                    },
                    ["required"] = new JArray { "objectPath" }
                }
            });

            tools.Add(new JObject
            {
                ["name"] = "send_console_log",
                ["description"] = "Send a message to Unity console",
                ["inputSchema"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["message"] = new JObject
                        {
                            ["type"] = "string",
                            ["description"] = "Message to log"
                        },
                        ["logType"] = new JObject
                        {
                            ["type"] = "string",
                            ["enum"] = new JArray { "Log", "Warning", "Error" },
                            ["description"] = "Type of log message"
                        }
                    },
                    ["required"] = new JArray { "message" }
                }
            });

            return new JObject
            {
                ["tools"] = tools
            };
        }

        private async Task<JObject> HandleToolCall(JToken paramsObj)
        {
            await Task.CompletedTask; // Make it async

            string toolName = paramsObj["name"]?.ToString();
            JObject arguments = paramsObj["arguments"] as JObject;

            try
            {
                switch (toolName)
                {
                    case "execute_menu_item":
                        return await ExecuteMenuItem(arguments);

                    case "select_gameobject":
                        return await SelectGameObject(arguments);

                    case "send_console_log":
                        return await SendConsoleLog(arguments);

                    default:
                        return new JObject
                        {
                            ["content"] = new JArray
                            {
                                new JObject
                                {
                                    ["type"] = "text",
                                    ["text"] = $"Unknown tool: {toolName}"
                                }
                            }
                        };
                }
            }
            catch (Exception ex)
            {
                return new JObject
                {
                    ["content"] = new JArray
                    {
                        new JObject
                        {
                            ["type"] = "text",
                            ["text"] = $"Error executing {toolName}: {ex.Message}"
                        }
                    }
                };
            }
        }

        private async Task<JObject> ExecuteMenuItem(JObject arguments)
        {
            await Task.CompletedTask; // Make it async

            string menuPath = arguments["menuPath"]?.ToString();

            if (string.IsNullOrEmpty(menuPath))
            {
                throw new ArgumentException("menuPath is required");
            }

            // Execute on main thread
            bool success = false;
            EditorApplication.delayCall += () =>
            {
                try
                {
                    EditorApplication.ExecuteMenuItem(menuPath);
                    success = true;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to execute menu item '{menuPath}': {ex.Message}");
                }
            };

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Executed menu item: {menuPath}"
                    }
                }
            };
        }

        private async Task<JObject> SelectGameObject(JObject arguments)
        {
            await Task.CompletedTask; // Make it async

            string objectPath = arguments["objectPath"]?.ToString();

            if (string.IsNullOrEmpty(objectPath))
            {
                throw new ArgumentException("objectPath is required");
            }

            GameObject found = null;
            EditorApplication.delayCall += () =>
            {
                found = GameObject.Find(objectPath);
                if (found != null)
                {
                    Selection.activeGameObject = found;
                    EditorGUIUtility.PingObject(found);
                }
            };

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = found != null ? $"Selected GameObject: {objectPath}" : $"GameObject not found: {objectPath}"
                    }
                }
            };
        }

        private async Task<JObject> SendConsoleLog(JObject arguments)
        {
            await Task.CompletedTask; // Make it async

            string message = arguments["message"]?.ToString();
            string logType = arguments["logType"]?.ToString() ?? "Log";

            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentException("message is required");
            }

            EditorApplication.delayCall += () =>
            {
                switch (logType.ToLower())
                {
                    case "warning":
                        Debug.LogWarning($"[MCP] {message}");
                        break;
                    case "error":
                        Debug.LogError($"[MCP] {message}");
                        break;
                    default:
                        Debug.Log($"[MCP] {message}");
                        break;
                }
            };

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Logged message: {message}"
                    }
                }
            };
        }

        private async Task<JObject> HandleResourcesList()
        {
            await Task.CompletedTask; // Make it async

            var resources = new JArray();

            resources.Add(new JObject
            {
                ["uri"] = "unity://scene-hierarchy",
                ["name"] = "Scene Hierarchy",
                ["description"] = "Current Unity scene hierarchy"
            });

            resources.Add(new JObject
            {
                ["uri"] = "unity://console-logs",
                ["name"] = "Console Logs",
                ["description"] = "Unity console log messages"
            });

            return new JObject
            {
                ["resources"] = resources
            };
        }

        private async Task<JObject> HandleResourceRead(JToken paramsObj)
        {
            await Task.CompletedTask; // Make it async

            string uri = paramsObj["uri"]?.ToString();

            switch (uri)
            {
                case "unity://scene-hierarchy":
                    return GetSceneHierarchy();

                case "unity://console-logs":
                    return GetConsoleLogs();

                default:
                    throw new ArgumentException($"Unknown resource URI: {uri}");
            }
        }

        private JObject GetSceneHierarchy()
        {
            var hierarchy = new JArray();

            // Get all root GameObjects in the current scene
            var rootObjects = UnityEngine.SceneManagement.SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var rootObj in rootObjects)
            {
                hierarchy.Add(GetGameObjectData(rootObj));
            }

            return new JObject
            {
                ["contents"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Scene Hierarchy:\n{hierarchy.ToString(Formatting.Indented)}"
                    }
                }
            };
        }

        private JObject GetGameObjectData(GameObject obj)
        {
            var data = new JObject
            {
                ["name"] = obj.name,
                ["active"] = obj.activeInHierarchy,
                ["tag"] = obj.tag,
                ["layer"] = obj.layer
            };

            if (obj.transform.childCount > 0)
            {
                var children = new JArray();
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    children.Add(GetGameObjectData(obj.transform.GetChild(i).gameObject));
                }
                data["children"] = children;
            }

            return data;
        }

        private JObject GetConsoleLogs()
        {
            return new JObject
            {
                ["contents"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = "Console logs would be retrieved here. This requires additional implementation to capture Unity console messages."
                    }
                }
            };
        }
    }
}