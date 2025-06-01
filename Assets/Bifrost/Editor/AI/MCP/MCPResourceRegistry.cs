using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

namespace Bifrost.Editor.AI.MCP
{
    public interface IMCPResource
    {
        string Uri { get; }
        string Name { get; }
        string Description { get; }
        string MimeType { get; }
        Task<MCPResourceReadResult> Read(MCPResourceReadParams parameters);
        Task Subscribe(Action<MCPResourceUpdate> callback);
        Task Unsubscribe(Action<MCPResourceUpdate> callback);
    }

    public class MCPResourceUpdate
    {
        public string Uri { get; set; }
        public string UpdateType { get; set; }
        public MCPContent[] Contents { get; set; }
    }

    public abstract class MCPResourceBase : IMCPResource
    {
        public abstract string Uri { get; }
        public abstract string Name { get; }
        public abstract string Description { get; }
        public virtual string MimeType => "text/plain";

        private readonly List<Action<MCPResourceUpdate>> subscribers = new List<Action<MCPResourceUpdate>>();

        public abstract Task<MCPResourceReadResult> Read(MCPResourceReadParams parameters);

        public virtual Task Subscribe(Action<MCPResourceUpdate> callback)
        {
            if (!subscribers.Contains(callback))
            {
                subscribers.Add(callback);
            }
            return Task.CompletedTask;
        }

        public virtual Task Unsubscribe(Action<MCPResourceUpdate> callback)
        {
            subscribers.Remove(callback);
            return Task.CompletedTask;
        }

        protected void NotifySubscribers(MCPResourceUpdate update)
        {
            foreach (var subscriber in subscribers.ToList())
            {
                try
                {
                    subscriber?.Invoke(update);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error notifying subscriber for resource {Uri}: {ex.Message}");
                }
            }
        }

        protected MCPResourceReadResult TextResult(string text)
        {
            return new MCPResourceReadResult
            {
                Contents = new[]
                {
                    new MCPContent
                    {
                        Type = "text",
                        Text = text,
                        MimeType = MimeType
                    }
                }
            };
        }

        protected MCPResourceReadResult JsonResult(object data)
        {
            return new MCPResourceReadResult
            {
                Contents = new[]
                {
                    new MCPContent
                    {
                        Type = "text",
                        Text = Newtonsoft.Json.JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented),
                        MimeType = "application/json"
                    }
                }
            };
        }
    }

    public class MCPResourceRegistry
    {
        private readonly Dictionary<string, IMCPResource> resources = new Dictionary<string, IMCPResource>();
        private readonly List<MCPResource> resourceDescriptions = new List<MCPResource>();

        public MCPResourceRegistry()
        {
            RegisterBuiltInResources();
        }

        public void RegisterResource(IMCPResource resource)
        {
            if (resources.ContainsKey(resource.Uri))
            {
                Debug.LogWarning($"Resource {resource.Uri} is already registered. Overwriting.");
            }

            resources[resource.Uri] = resource;
            
            resourceDescriptions.RemoveAll(r => r.Uri == resource.Uri);
            resourceDescriptions.Add(new MCPResource
            {
                Uri = resource.Uri,
                Name = resource.Name,
                Description = resource.Description,
                MimeType = resource.MimeType
            });

            Debug.Log($"Registered MCP resource: {resource.Uri}");
        }

        private void RegisterBuiltInResources()
        {
            // Register all built-in Unity resources
            RegisterResource(new Resources.SceneHierarchyResource());
            RegisterResource(new Resources.ProjectStructureResource());
            RegisterResource(new Resources.SelectionResource());
            RegisterResource(new Resources.ConsoleLogsResource());
            RegisterResource(new Resources.BuildSettingsResource());
            RegisterResource(new Resources.AssetsListResource());
        }

        public IMCPResource GetResource(string uri)
        {
            return resources.TryGetValue(uri, out var resource) ? resource : null;
        }

        public IEnumerable<MCPResource> GetAllResources()
        {
            return resourceDescriptions.AsReadOnly();
        }

        public bool HasResource(string uri)
        {
            return resources.ContainsKey(uri);
        }

        public void UnregisterResource(string uri)
        {
            if (resources.Remove(uri))
            {
                resourceDescriptions.RemoveAll(r => r.Uri == uri);
                Debug.Log($"Unregistered MCP resource: {uri}");
            }
        }

        public void Clear()
        {
            resources.Clear();
            resourceDescriptions.Clear();
        }

        public async Task SubscribeToResource(string uri, Action<MCPResourceUpdate> callback)
        {
            var resource = GetResource(uri);
            if (resource != null)
            {
                await resource.Subscribe(callback);
            }
            else
            {
                throw new ArgumentException($"Resource not found: {uri}");
            }
        }

        public async Task UnsubscribeFromResource(string uri, Action<MCPResourceUpdate> callback)
        {
            var resource = GetResource(uri);
            if (resource != null)
            {
                await resource.Unsubscribe(callback);
            }
        }
    }
}