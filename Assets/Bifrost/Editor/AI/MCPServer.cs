using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebSocketSharp;
using WebSocketSharp.Server;

namespace Bifrost.Editor.AI
{
    public class MCPServer
    {
        private WebSocketServer webSocketServer;
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

        public void Start()
        {
            if (isRunning) return;

            try
            {
                webSocketServer = new WebSocketServer(port);

                // Add the MCP behavior service
                webSocketServer.AddWebSocketService<MCPBehavior>("/", behavior =>
                {
                    behavior.OnClientConnected = OnClientConnected;
                    behavior.OnClientDisconnected = OnClientDisconnected;
                    behavior.OnError = OnError;
                    behavior.OnLog = OnLog;
                });

                webSocketServer.Start();
                isRunning = true;

                OnLog?.Invoke($"WebSocket-Sharp MCP Server started on ws://localhost:{port}");
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
                webSocketServer?.Stop();
                webSocketServer = null;
                isRunning = false;
                OnLog?.Invoke("WebSocket-Sharp MCP Server stopped");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error stopping MCP server: {ex.Message}");
            }
        }
    }

    public class MCPBehavior : WebSocketBehavior
    {
        public Action<string> OnClientConnected;
        public Action<string> OnClientDisconnected;
        public Action<string> OnError;
        public Action<string> OnLog;

        protected override void OnOpen()
        {
            OnClientConnected?.Invoke(ID);
            OnLog?.Invoke($"MCP Client connected: {ID}");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            OnClientDisconnected?.Invoke(ID);
            OnLog?.Invoke($"MCP Client disconnected: {ID} - {e.Reason}");
        }

        protected override void OnError(ErrorEventArgs e)
        {
            OnError?.Invoke($"WebSocket error with client {ID}: {e.Message}");
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            try
            {
                OnLog?.Invoke($"Received from {ID}: {e.Data}");

                JObject request = JObject.Parse(e.Data);
                string method = request["method"]?.ToString();
                JToken paramsObj = request["params"];
                string id = request["id"]?.ToString();

                JObject response = new JObject();
                response["jsonrpc"] = "2.0";
                response["id"] = id;

                switch (method)
                {
                    case "initialize":
                        response["result"] = HandleInitialize(paramsObj);
                        break;

                    case "tools/list":
                        response["result"] = HandleToolsList();
                        break;

                    case "tools/call":
                        response["result"] = HandleToolCall(paramsObj);
                        break;

                    case "resources/list":
                        response["result"] = HandleResourcesList();
                        break;

                    case "resources/read":
                        response["result"] = HandleResourceRead(paramsObj);
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
                Send(responseJson);
                OnLog?.Invoke($"Sent to {ID}: {responseJson}");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error processing message from {ID}: {ex.Message}");

                // Send error response
                JObject errorResponse = new JObject
                {
                    ["jsonrpc"] = "2.0",
                    ["id"] = null,
                    ["error"] = new JObject
                    {
                        ["code"] = -32603,
                        ["message"] = $"Internal error: {ex.Message}"
                    }
                };
                Send(errorResponse.ToString());
            }
        }

        private JObject HandleInitialize(JToken paramsObj)
        {
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

        private JObject HandleToolsList()
        {
            var tools = new JArray();

            // Execute menu items
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

            // Select GameObjects
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

            // Create GameObjects
            tools.Add(new JObject
            {
                ["name"] = "create_gameobject",
                ["description"] = "Create new GameObjects in the scene",
                ["inputSchema"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["name"] = new JObject
                        {
                            ["type"] = "string",
                            ["description"] = "Name of the GameObject"
                        },
                        ["primitiveType"] = new JObject
                        {
                            ["type"] = "string",
                            ["enum"] = new JArray { "Cube", "Sphere", "Capsule", "Cylinder", "Plane", "Quad" },
                            ["description"] = "Type of primitive to create"
                        },
                        ["position"] = new JObject
                        {
                            ["type"] = "object",
                            ["properties"] = new JObject
                            {
                                ["x"] = new JObject { ["type"] = "number" },
                                ["y"] = new JObject { ["type"] = "number" },
                                ["z"] = new JObject { ["type"] = "number" }
                            }
                        }
                    },
                    ["required"] = new JArray { "name" }
                }
            });

            // Console logging
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

        private JObject HandleToolCall(JToken paramsObj)
        {
            string toolName = paramsObj["name"]?.ToString();
            JObject arguments = paramsObj["arguments"] as JObject;

            try
            {
                switch (toolName)
                {
                    case "execute_menu_item":
                        return ExecuteMenuItem(arguments);

                    case "select_gameobject":
                        return SelectGameObject(arguments);

                    case "create_gameobject":
                        return CreateGameObject(arguments);

                    case "send_console_log":
                        return SendConsoleLog(arguments);

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

        private JObject ExecuteMenuItem(JObject arguments)
        {
            string menuPath = arguments["menuPath"]?.ToString();

            if (string.IsNullOrEmpty(menuPath))
            {
                throw new ArgumentException("menuPath is required");
            }

            bool success = false;
            string result = "";

            EditorApplication.delayCall += () =>
            {
                try
                {
                    EditorApplication.ExecuteMenuItem(menuPath);
                    success = true;
                    result = $"Successfully executed menu item: {menuPath}";
                }
                catch (Exception ex)
                {
                    result = $"Failed to execute menu item '{menuPath}': {ex.Message}";
                    Debug.LogError(result);
                }
            };

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Queued menu item execution: {menuPath}"
                    }
                }
            };
        }

        private JObject SelectGameObject(JObject arguments)
        {
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
                        ["text"] = $"Queued selection of GameObject: {objectPath}"
                    }
                }
            };
        }

        private JObject CreateGameObject(JObject arguments)
        {
            string name = arguments["name"]?.ToString();
            string primitiveType = arguments["primitiveType"]?.ToString() ?? "Cube";

            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("name is required");
            }

            EditorApplication.delayCall += () =>
            {
                try
                {
                    GameObject go;
                    if (Enum.TryParse<PrimitiveType>(primitiveType, out PrimitiveType primitive))
                    {
                        go = GameObject.CreatePrimitive(primitive);
                    }
                    else
                    {
                        go = new GameObject();
                    }

                    go.name = name;

                    // Set position if provided
                    if (arguments["position"] is JObject posObj)
                    {
                        float x = posObj["x"]?.ToObject<float>() ?? 0f;
                        float y = posObj["y"]?.ToObject<float>() ?? 0f;
                        float z = posObj["z"]?.ToObject<float>() ?? 0f;
                        go.transform.position = new Vector3(x, y, z);
                    }

                    Undo.RegisterCreatedObjectUndo(go, $"Create {name}");
                    Selection.activeGameObject = go;
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to create GameObject '{name}': {ex.Message}");
                }
            };

            return new JObject
            {
                ["content"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Queued creation of GameObject: {name} ({primitiveType})"
                    }
                }
            };
        }

        private JObject SendConsoleLog(JObject arguments)
        {
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

        private JObject HandleResourcesList()
        {
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

            resources.Add(new JObject
            {
                ["uri"] = "unity://selection",
                ["name"] = "Current Selection",
                ["description"] = "Currently selected objects in Unity"
            });

            return new JObject
            {
                ["resources"] = resources
            };
        }

        private JObject HandleResourceRead(JToken paramsObj)
        {
            string uri = paramsObj["uri"]?.ToString();

            switch (uri)
            {
                case "unity://scene-hierarchy":
                    return GetSceneHierarchy();

                case "unity://console-logs":
                    return GetConsoleLogs();

                case "unity://selection":
                    return GetCurrentSelection();

                default:
                    throw new ArgumentException($"Unknown resource URI: {uri}");
            }
        }

        private JObject GetSceneHierarchy()
        {
            var hierarchy = new JArray();

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
                        ["text"] = $"Unity Scene Hierarchy:\n{hierarchy.ToString(Formatting.Indented)}"
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
                ["layer"] = obj.layer,
                ["position"] = new JObject
                {
                    ["x"] = obj.transform.position.x,
                    ["y"] = obj.transform.position.y,
                    ["z"] = obj.transform.position.z
                }
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
                        ["text"] = "Unity Console logs: Access via Unity Console window. Real-time log capture requires additional implementation."
                    }
                }
            };
        }

        private JObject GetCurrentSelection()
        {
            var selection = new JArray();

            foreach (var obj in Selection.gameObjects)
            {
                selection.Add(GetGameObjectData(obj));
            }

            return new JObject
            {
                ["contents"] = new JArray
                {
                    new JObject
                    {
                        ["type"] = "text",
                        ["text"] = $"Current Unity Selection:\n{selection.ToString(Formatting.Indented)}"
                    }
                }
            };
        }
    }
}