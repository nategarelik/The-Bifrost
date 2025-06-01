using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Bifrost.Editor.AI.MCP
{
    /// <summary>
    /// Simplified MCP Server implementation using TCP sockets
    /// This avoids external WebSocket dependencies
    /// </summary>
    public class SimpleMCPServer
    {
        private TcpListener tcpListener;
        private bool isRunning = false;
        private int port;
        private int timeout;
        private readonly MCPToolRegistry toolRegistry;
        private readonly MCPResourceRegistry resourceRegistry;
        private readonly IMCPProtocol protocol;
        private readonly List<TcpClient> connectedClients = new List<TcpClient>();
        private Thread listenerThread;

        public event Action<string> OnClientConnected;
        public event Action<string> OnClientDisconnected;
        public event Action<string> OnError;
        public event Action<string> OnLog;

        public bool IsRunning => isRunning;
        public int Port => port;
        public MCPToolRegistry ToolRegistry => toolRegistry;
        public MCPResourceRegistry ResourceRegistry => resourceRegistry;

        public SimpleMCPServer(int port = 8090, int timeout = 10)
        {
            this.port = port;
            this.timeout = timeout;
            this.toolRegistry = new MCPToolRegistry();
            this.resourceRegistry = new MCPResourceRegistry();
            this.protocol = new MCPProtocol(toolRegistry, resourceRegistry);
        }

        public void Start()
        {
            if (isRunning) return;

            try
            {
                tcpListener = new TcpListener(IPAddress.Any, port);
                tcpListener.Start();
                isRunning = true;

                listenerThread = new Thread(ListenForClients)
                {
                    IsBackground = true
                };
                listenerThread.Start();

                OnLog?.Invoke($"Simple MCP Server started on port {port}");
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
                isRunning = false;
                
                foreach (var client in connectedClients)
                {
                    client?.Close();
                }
                connectedClients.Clear();

                tcpListener?.Stop();
                listenerThread?.Join(1000);
                
                OnLog?.Invoke("Simple MCP Server stopped");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Error stopping MCP server: {ex.Message}");
            }
        }

        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    var tcpClient = tcpListener.AcceptTcpClient();
                    var clientId = Guid.NewGuid().ToString();
                    
                    lock (connectedClients)
                    {
                        connectedClients.Add(tcpClient);
                    }

                    EditorApplication.delayCall += () => OnClientConnected?.Invoke(clientId);

                    var clientThread = new Thread(() => HandleClient(tcpClient, clientId))
                    {
                        IsBackground = true
                    };
                    clientThread.Start();
                }
                catch (Exception ex)
                {
                    if (isRunning)
                    {
                        EditorApplication.delayCall += () => OnError?.Invoke($"Error accepting client: {ex.Message}");
                    }
                }
            }
        }

        private async void HandleClient(TcpClient client, string clientId)
        {
            try
            {
                var stream = client.GetStream();
                var buffer = new byte[4096];

                while (client.Connected && isRunning)
                {
                    var bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);
                    if (bytesRead == 0) break;

                    var message = Encoding.UTF8.GetString(buffer, 0, bytesRead);
                    EditorApplication.delayCall += () => OnLog?.Invoke($"[{clientId}] → {message}");

                    try
                    {
                        var request = JObject.Parse(message);
                        var response = await ProcessRequest(request);
                        
                        var responseJson = response.ToString(Formatting.None);
                        var responseBytes = Encoding.UTF8.GetBytes(responseJson + "\n");
                        await stream.WriteAsync(responseBytes, 0, responseBytes.Length);
                        
                        EditorApplication.delayCall += () => OnLog?.Invoke($"[{clientId}] ← Response sent");
                    }
                    catch (Exception ex)
                    {
                        var errorResponse = CreateErrorResponse(null, -32700, $"Parse error: {ex.Message}");
                        var errorBytes = Encoding.UTF8.GetBytes(errorResponse.ToString() + "\n");
                        await stream.WriteAsync(errorBytes, 0, errorBytes.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                EditorApplication.delayCall += () => OnError?.Invoke($"Client error: {ex.Message}");
            }
            finally
            {
                lock (connectedClients)
                {
                    connectedClients.Remove(client);
                }
                client?.Close();
                EditorApplication.delayCall += () => OnClientDisconnected?.Invoke(clientId);
            }
        }

        private async Task<JObject> ProcessRequest(JObject request)
        {
            var method = request["method"]?.ToString();
            var id = request["id"];

            switch (method)
            {
                case "initialize":
                    var initParams = request["params"]?.ToObject<MCPInitializeParams>() ?? new MCPInitializeParams();
                    var initResult = await protocol.Initialize(initParams);
                    return CreateSuccessResponse(id, initResult);

                case "tools/list":
                    var toolsResult = await protocol.ListTools();
                    return CreateSuccessResponse(id, toolsResult);

                case "tools/call":
                    var toolParams = request["params"]?.ToObject<MCPToolCallParams>();
                    if (toolParams == null)
                        return CreateErrorResponse(id, -32602, "Invalid parameters");
                    var toolResult = await protocol.CallTool(toolParams);
                    return CreateSuccessResponse(id, toolResult);

                case "resources/list":
                    var resourcesResult = await protocol.ListResources();
                    return CreateSuccessResponse(id, resourcesResult);

                case "resources/read":
                    var resourceParams = request["params"]?.ToObject<MCPResourceReadParams>();
                    if (resourceParams == null)
                        return CreateErrorResponse(id, -32602, "Invalid parameters");
                    var resourceResult = await protocol.ReadResource(resourceParams);
                    return CreateSuccessResponse(id, resourceResult);

                default:
                    return CreateErrorResponse(id, -32601, $"Method not found: {method}");
            }
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
}