using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
#if UNITY_EDITOR
using WebSocketSharp;
using WebSocketSharp.Server;
#endif

namespace Bifrost.Editor.AI.MCP
{
    public class MCPServer
    {
#if UNITY_EDITOR
        private WebSocketServer webSocketServer;
#endif
        private bool isRunning = false;
        private int port;
        
        private readonly MCPToolRegistry toolRegistry;
        private readonly MCPResourceRegistry resourceRegistry;
        private readonly IMCPProtocol protocol;

        public event Action<string> OnClientConnected;
        public event Action<string> OnClientDisconnected;
        public event Action<string> OnError;
        public event Action<string> OnLog;

        public bool IsRunning => isRunning;
        public int Port => port;
        public MCPToolRegistry ToolRegistry => toolRegistry;
        public MCPResourceRegistry ResourceRegistry => resourceRegistry;

        public MCPServer(int port = 8090)
        {
            this.port = port;
            this.toolRegistry = new MCPToolRegistry();
            this.resourceRegistry = new MCPResourceRegistry();
            this.protocol = new MCPProtocol(toolRegistry, resourceRegistry);
        }

        public void Start()
        {
            if (isRunning) return;

#if UNITY_EDITOR
            try
            {
                webSocketServer = new WebSocketServer(port);
                webSocketServer.AddWebSocketService<MCPBehavior>("/", behavior =>
                {
                    behavior.Initialize(protocol, this);
                });

                webSocketServer.Start();
                isRunning = true;

                OnLog?.Invoke($"Enhanced MCP Server started on ws://localhost:{port}");
                OnLog?.Invoke($"Loaded {toolRegistry.GetAllTools().Count()} tools");
                OnLog?.Invoke($"Loaded {resourceRegistry.GetAllResources().Count()} resources");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Failed to start MCP server: {ex.Message}");
                isRunning = false;
            }
#else
            OnError?.Invoke("WebSocket support not available. Please ensure websocket-sharp.dll is in Assets/Plugins/");
#endif
        }

        public void Stop()
        {
            if (!isRunning) return;

#if UNITY_EDITOR
            try
            {
                webSocketServer?.Stop();
                webSocketServer = null;
                isRunning = false;
#else
            isRunning = false;
#endif
                OnLog?.Invoke("Enhanced MCP Server stopped");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error stopping MCP server: {ex.Message}");
            }
        }

        internal void NotifyClientConnected(string clientId)
        {
            OnClientConnected?.Invoke(clientId);
        }

        internal void NotifyClientDisconnected(string clientId)
        {
            OnClientDisconnected?.Invoke(clientId);
        }

        internal void NotifyError(string error)
        {
            OnError?.Invoke(error);
        }

        internal void NotifyLog(string message)
        {
            OnLog?.Invoke(message);
        }
    }

#if UNITY_EDITOR
    public class MCPBehavior : WebSocketBehavior
    {
        private IMCPProtocol protocol;
        private MCPServer server;
        private Dictionary<string, Func<JObject, Task<object>>> methodHandlers;

        public void Initialize(IMCPProtocol protocol, MCPServer server)
        {
            this.protocol = protocol;
            this.server = server;
            
            // Initialize method handlers
            methodHandlers = new Dictionary<string, Func<JObject, Task<object>>>
            {
                ["initialize"] = HandleInitialize,
                ["tools/list"] = HandleToolsList,
                ["tools/call"] = HandleToolCall,
                ["resources/list"] = HandleResourcesList,
                ["resources/read"] = HandleResourceRead,
                ["resources/subscribe"] = HandleResourceSubscribe,
                ["resources/unsubscribe"] = HandleResourceUnsubscribe,
                ["ping"] = HandlePing
            };
        }

        protected override void OnOpen()
        {
            server.NotifyClientConnected(ID);
            server.NotifyLog($"MCP Client connected: {ID}");
        }

        protected override void OnClose(CloseEventArgs e)
        {
            server.NotifyClientDisconnected(ID);
            server.NotifyLog($"MCP Client disconnected: {ID} - {e.Reason}");
        }

        protected override void OnError(ErrorEventArgs e)
        {
            server.NotifyError($"WebSocket error with client {ID}: {e.Message}");
        }

        protected override async void OnMessage(MessageEventArgs e)
        {
            JObject response = null;
            try
            {
                var request = JObject.Parse(e.Data);
                var method = request["method"]?.ToString();
                var id = request["id"];

                server.NotifyLog($"[{ID}] → {method}");

                if (methodHandlers.TryGetValue(method, out var handler))
                {
                    var result = await handler(request);
                    response = CreateSuccessResponse(id, result);
                }
                else
                {
                    response = CreateErrorResponse(id, -32601, $"Method not found: {method}");
                }
            }
            catch (JsonException ex)
            {
                response = CreateErrorResponse(null, -32700, $"Parse error: {ex.Message}");
            }
            catch (Exception ex)
            {
                server.NotifyError($"Error processing message from {ID}: {ex.Message}");
                response = CreateErrorResponse(null, -32603, $"Internal error: {ex.Message}");
            }

            if (response != null)
            {
                var responseJson = response.ToString(Formatting.None);
                Send(responseJson);
                server.NotifyLog($"[{ID}] ← Response sent");
            }
        }

        private async Task<object> HandleInitialize(JObject request)
        {
            var parameters = request["params"]?.ToObject<MCPInitializeParams>() ?? new MCPInitializeParams();
            return await protocol.Initialize(parameters);
        }

        private async Task<object> HandleToolsList(JObject request)
        {
            return await protocol.ListTools();
        }

        private async Task<object> HandleToolCall(JObject request)
        {
            var parameters = request["params"]?.ToObject<MCPToolCallParams>();
            if (parameters == null)
            {
                throw new ArgumentException("Invalid tool call parameters");
            }
            return await protocol.CallTool(parameters);
        }

        private async Task<object> HandleResourcesList(JObject request)
        {
            return await protocol.ListResources();
        }

        private async Task<object> HandleResourceRead(JObject request)
        {
            var parameters = request["params"]?.ToObject<MCPResourceReadParams>();
            if (parameters == null)
            {
                throw new ArgumentException("Invalid resource read parameters");
            }
            return await protocol.ReadResource(parameters);
        }

        private Task<object> HandleResourceSubscribe(JObject request)
        {
            var uri = request["params"]?["uri"]?.ToString();
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException("Resource URI is required");
            }

            // TODO: Implement resource subscription
            return Task.FromResult<object>(new { success = true });
        }

        private Task<object> HandleResourceUnsubscribe(JObject request)
        {
            var uri = request["params"]?["uri"]?.ToString();
            if (string.IsNullOrEmpty(uri))
            {
                throw new ArgumentException("Resource URI is required");
            }

            // TODO: Implement resource unsubscription
            return Task.FromResult<object>(new { success = true });
        }

        private Task<object> HandlePing(JObject request)
        {
            return Task.FromResult<object>(new { pong = true });
        }

        private JObject CreateSuccessResponse(JToken id, object result)
        {
            return new JObject
            {
                ["jsonrpc"] = "2.0",
                ["id"] = id,
                ["result"] = JToken.FromObject(result)
            };
        }

        private JObject CreateErrorResponse(JToken id, int code, string message)
        {
            return new JObject
            {
                ["jsonrpc"] = "2.0",
                ["id"] = id,
                ["error"] = new JObject
                {
                    ["code"] = code,
                    ["message"] = message
                }
            };
        }
    }
#endif
}