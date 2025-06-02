using System;
using System.Threading.Tasks;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace Bifrost.Editor.AI.MCP
{
    public class MCPInitializeParams
    {
        public string ProtocolVersion { get; set; }
        public MCPClientInfo ClientInfo { get; set; }
    }

    public class MCPClientInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public class MCPInitializeResult
    {
        public string ProtocolVersion { get; set; }
        public MCPServerInfo ServerInfo { get; set; }
        public MCPCapabilities Capabilities { get; set; }
    }

    public class MCPServerInfo
    {
        public string Name { get; set; }
        public string Version { get; set; }
    }

    public class MCPCapabilities
    {
        public MCPToolCapabilities Tools { get; set; }
        public MCPResourceCapabilities Resources { get; set; }
        public MCPPromptCapabilities Prompts { get; set; }
    }

    public class MCPToolCapabilities
    {
        public bool ListChanged { get; set; }
    }

    public class MCPResourceCapabilities
    {
        public bool Subscribe { get; set; }
        public bool ListChanged { get; set; }
    }

    public class MCPPromptCapabilities
    {
        public bool ListChanged { get; set; }
    }

    public class MCPToolListResult
    {
        public MCPTool[] Tools { get; set; }
    }

    public class MCPTool
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public JObject InputSchema { get; set; }
    }

    public class MCPResourceListResult
    {
        public MCPResource[] Resources { get; set; }
    }

    public class MCPResource
    {
        public string Uri { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string MimeType { get; set; }
    }

    public class MCPToolCallParams
    {
        public string Name { get; set; }
        public JObject Arguments { get; set; }
    }

    public class MCPToolCallResult
    {
        public MCPContent[] Content { get; set; }
        public bool IsError { get; set; }
    }

    public class MCPContent
    {
        public string Type { get; set; }
        public string Text { get; set; }
        public string MimeType { get; set; }
        public string Data { get; set; }
    }

    public class MCPResourceReadParams
    {
        public string Uri { get; set; }
    }

    public class MCPResourceReadResult
    {
        public MCPContent[] Contents { get; set; }
    }

    public class MCPExecutionContext
    {
        public string ClientId { get; set; }
        public DateTime RequestTime { get; set; }
        public MCPServerContext ServerContext { get; set; }
    }

    public class MCPServerContext
    {
        public string CurrentScene { get; set; }
        public string ProjectPath { get; set; }
        public string UnityVersion { get; set; }
    }

    public interface IMCPProtocol
    {
        string ProtocolVersion { get; }
        Task<MCPInitializeResult> Initialize(MCPInitializeParams parameters);
        Task<MCPToolListResult> ListTools();
        Task<MCPResourceListResult> ListResources();
        Task<MCPToolCallResult> CallTool(MCPToolCallParams parameters);
        Task<MCPResourceReadResult> ReadResource(MCPResourceReadParams parameters);
    }

    public class MCPProtocol : IMCPProtocol
    {
        private readonly MCPToolRegistry toolRegistry;
        private readonly MCPResourceRegistry resourceRegistry;
        
        public string ProtocolVersion => "2024-11-05";

        public MCPProtocol(MCPToolRegistry toolRegistry, MCPResourceRegistry resourceRegistry)
        {
            this.toolRegistry = toolRegistry;
            this.resourceRegistry = resourceRegistry;
        }

        public async Task<MCPInitializeResult> Initialize(MCPInitializeParams parameters)
        {
            return await Task.FromResult(new MCPInitializeResult
            {
                ProtocolVersion = ProtocolVersion,
                ServerInfo = new MCPServerInfo
                {
                    Name = "Bifrost Unity MCP Server",
                    Version = "2.0.0"
                },
                Capabilities = new MCPCapabilities
                {
                    Tools = new MCPToolCapabilities { ListChanged = true },
                    Resources = new MCPResourceCapabilities { Subscribe = true, ListChanged = true },
                    Prompts = new MCPPromptCapabilities { ListChanged = true }
                }
            });
        }

        public async Task<MCPToolListResult> ListTools()
        {
            var tools = toolRegistry.GetAllTools();
            return await Task.FromResult(new MCPToolListResult
            {
                Tools = tools.ToArray()
            });
        }

        public async Task<MCPResourceListResult> ListResources()
        {
            var resources = resourceRegistry.GetAllResources();
            return await Task.FromResult(new MCPResourceListResult
            {
                Resources = resources.ToArray()
            });
        }

        public async Task<MCPToolCallResult> CallTool(MCPToolCallParams parameters)
        {
            var tool = toolRegistry.GetTool(parameters.Name);
            if (tool == null)
            {
                return new MCPToolCallResult
                {
                    IsError = true,
                    Content = new[]
                    {
                        new MCPContent
                        {
                            Type = "text",
                            Text = $"Tool not found: {parameters.Name}"
                        }
                    }
                };
            }

            var context = new MCPExecutionContext
            {
                ClientId = "mcp-client",
                RequestTime = DateTime.Now,
                ServerContext = new MCPServerContext
                {
                    CurrentScene = UnityEngine.SceneManagement.SceneManager.GetActiveScene().name,
                    ProjectPath = UnityEngine.Application.dataPath,
                    UnityVersion = UnityEngine.Application.unityVersion
                }
            };

            return await tool.Execute(parameters.Arguments, context);
        }

        public async Task<MCPResourceReadResult> ReadResource(MCPResourceReadParams parameters)
        {
            var resource = resourceRegistry.GetResource(parameters.Uri);
            if (resource == null)
            {
                throw new ArgumentException($"Resource not found: {parameters.Uri}");
            }

            return await resource.Read(parameters);
        }
    }
}