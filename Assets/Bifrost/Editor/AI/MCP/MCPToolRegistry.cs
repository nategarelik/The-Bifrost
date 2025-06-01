using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using UnityEngine;

namespace Bifrost.Editor.AI.MCP
{
    public interface IMCPTool
    {
        string Name { get; }
        string Description { get; }
        JObject InputSchema { get; }
        Task<MCPToolCallResult> Execute(JObject arguments, MCPExecutionContext context);
    }

    [AttributeUsage(AttributeTargets.Class)]
    public class MCPToolAttribute : Attribute
    {
        public string Name { get; set; }
        public string Description { get; set; }
        
        public MCPToolAttribute(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }

    public abstract class MCPToolBase : IMCPTool
    {
        public abstract string Name { get; }
        public abstract string Description { get; }
        public abstract JObject InputSchema { get; }

        public abstract Task<MCPToolCallResult> Execute(JObject arguments, MCPExecutionContext context);

        protected MCPToolCallResult Success(string message)
        {
            return new MCPToolCallResult
            {
                IsError = false,
                Content = new[]
                {
                    new MCPContent
                    {
                        Type = "text",
                        Text = message
                    }
                }
            };
        }

        protected MCPToolCallResult Error(string message)
        {
            return new MCPToolCallResult
            {
                IsError = true,
                Content = new[]
                {
                    new MCPContent
                    {
                        Type = "text",
                        Text = message
                    }
                }
            };
        }

        protected MCPToolCallResult JsonResult(object data)
        {
            return new MCPToolCallResult
            {
                IsError = false,
                Content = new[]
                {
                    new MCPContent
                    {
                        Type = "text",
                        Text = JObject.FromObject(data).ToString()
                    }
                }
            };
        }
    }

    public class MCPToolRegistry
    {
        private readonly Dictionary<string, IMCPTool> tools = new Dictionary<string, IMCPTool>();
        private readonly List<MCPTool> toolDescriptions = new List<MCPTool>();

        public MCPToolRegistry()
        {
            RegisterToolsFromAssembly();
        }

        public void RegisterTool(IMCPTool tool)
        {
            if (tools.ContainsKey(tool.Name))
            {
                Debug.LogWarning($"Tool {tool.Name} is already registered. Overwriting.");
            }

            tools[tool.Name] = tool;
            
            toolDescriptions.RemoveAll(t => t.Name == tool.Name);
            toolDescriptions.Add(new MCPTool
            {
                Name = tool.Name,
                Description = tool.Description,
                InputSchema = tool.InputSchema
            });

            Debug.Log($"Registered MCP tool: {tool.Name}");
        }

        public void RegisterToolsFromAssembly()
        {
            var assembly = Assembly.GetExecutingAssembly();
            var toolTypes = assembly.GetTypes()
                .Where(t => typeof(IMCPTool).IsAssignableFrom(t) && !t.IsAbstract && !t.IsInterface)
                .ToList();

            foreach (var toolType in toolTypes)
            {
                try
                {
                    var tool = Activator.CreateInstance(toolType) as IMCPTool;
                    if (tool != null)
                    {
                        RegisterTool(tool);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Failed to register tool {toolType.Name}: {ex.Message}");
                }
            }

            Debug.Log($"Registered {tools.Count} MCP tools from assembly");
        }

        public IMCPTool GetTool(string name)
        {
            return tools.TryGetValue(name, out var tool) ? tool : null;
        }

        public IEnumerable<MCPTool> GetAllTools()
        {
            return toolDescriptions.AsReadOnly();
        }

        public bool HasTool(string name)
        {
            return tools.ContainsKey(name);
        }

        public void UnregisterTool(string name)
        {
            if (tools.Remove(name))
            {
                toolDescriptions.RemoveAll(t => t.Name == name);
                Debug.Log($"Unregistered MCP tool: {name}");
            }
        }

        public void Clear()
        {
            tools.Clear();
            toolDescriptions.Clear();
        }
    }
}