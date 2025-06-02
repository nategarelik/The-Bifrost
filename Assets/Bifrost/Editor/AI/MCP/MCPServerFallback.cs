using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace Bifrost.Editor.AI.MCP
{
    /// <summary>
    /// Fallback TCP server for when WebSocketSharp isn't available
    /// </summary>
    public class MCPServerFallback
    {
        private TcpListener tcpListener;
        private Thread listenerThread;
        private bool isRunning;
        private readonly int port;
        
        public event Action<string> OnLog;
        public event Action<string> OnError;
        
        public MCPServerFallback(int port)
        {
            this.port = port;
        }
        
        public void Start()
        {
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
                
                OnLog?.Invoke($"[Fallback] TCP Server started on port {port}");
                OnLog?.Invoke("[Fallback] Note: This is a limited TCP fallback. For full MCP support, install websocket-sharp.dll");
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Failed to start fallback server: {ex.Message}");
            }
        }
        
        public void Stop()
        {
            isRunning = false;
            tcpListener?.Stop();
            listenerThread?.Join(1000);
            OnLog?.Invoke("[Fallback] TCP Server stopped");
        }
        
        private void ListenForClients()
        {
            while (isRunning)
            {
                try
                {
                    var client = tcpListener.AcceptTcpClient();
                    var clientThread = new Thread(() => HandleClient(client))
                    {
                        IsBackground = true
                    };
                    clientThread.Start();
                }
                catch
                {
                    // Expected when stopping
                }
            }
        }
        
        private void HandleClient(TcpClient client)
        {
            try
            {
                var stream = client.GetStream();
                var buffer = new byte[4096];
                
                // Send a message explaining the limitation
                var message = JObject.FromObject(new
                {
                    jsonrpc = "2.0",
                    error = new
                    {
                        code = -32601,
                        message = "WebSocket support not available. Please install websocket-sharp.dll in Assets/Plugins/"
                    }
                }).ToString();
                
                var bytes = Encoding.UTF8.GetBytes(message);
                stream.Write(bytes, 0, bytes.Length);
            }
            catch (Exception ex)
            {
                OnError?.Invoke($"Client error: {ex.Message}");
            }
            finally
            {
                client.Close();
            }
        }
    }
}