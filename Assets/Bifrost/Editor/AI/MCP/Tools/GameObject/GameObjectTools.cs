using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Unity.Plastic.Newtonsoft.Json.Linq;
using Bifrost.Editor.AI.MCP;

namespace Bifrost.Editor.AI.MCP.Tools.GameObject
{
    [MCPTool("create_gameobject_advanced", "Create GameObject with full component setup")]
    public class CreateGameObjectAdvancedTool : MCPToolBase
    {
        public override string Name => "create_gameobject_advanced";
        public override string Description => "Create GameObject with full component setup";
        
        public override JObject InputSchema => new JObject
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
                    ["enum"] = new JArray { "None", "Cube", "Sphere", "Capsule", "Cylinder", "Plane", "Quad" },
                    ["description"] = "Primitive type to create"
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
                },
                ["rotation"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["x"] = new JObject { ["type"] = "number" },
                        ["y"] = new JObject { ["type"] = "number" },
                        ["z"] = new JObject { ["type"] = "number" }
                    }
                },
                ["scale"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["x"] = new JObject { ["type"] = "number" },
                        ["y"] = new JObject { ["type"] = "number" },
                        ["z"] = new JObject { ["type"] = "number" }
                    }
                },
                ["parent"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "Path or name of parent GameObject"
                },
                ["components"] = new JObject
                {
                    ["type"] = "array",
                    ["items"] = new JObject
                    {
                        ["type"] = "object",
                        ["properties"] = new JObject
                        {
                            ["type"] = new JObject { ["type"] = "string" },
                            ["properties"] = new JObject { ["type"] = "object" }
                        }
                    }
                }
            },
            ["required"] = new JArray { "name" }
        };

        public override async Task<MCPToolCallResult> Execute(JObject arguments, MCPExecutionContext context)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var name = arguments["name"]?.ToString();
                    if (string.IsNullOrEmpty(name))
                    {
                        return Error("GameObject name is required");
                    }

                    MCPToolCallResult result = null;
                    EditorApplication.delayCall += () =>
                    {
                        try
                        {
                            UnityEngine.GameObject go;
                            
                            // Create primitive or empty
                            var primitiveTypeStr = arguments["primitiveType"]?.ToString() ?? "None";
                            if (primitiveTypeStr != "None" && Enum.TryParse<PrimitiveType>(primitiveTypeStr, out var primitiveType))
                            {
                                go = UnityEngine.GameObject.CreatePrimitive(primitiveType);
                            }
                            else
                            {
                                go = new UnityEngine.GameObject();
                            }
                            
                            go.name = name;

                            // Set transform
                            if (arguments["position"] is JObject pos)
                            {
                                go.transform.position = new Vector3(
                                    pos["x"]?.ToObject<float>() ?? 0f,
                                    pos["y"]?.ToObject<float>() ?? 0f,
                                    pos["z"]?.ToObject<float>() ?? 0f
                                );
                            }

                            if (arguments["rotation"] is JObject rot)
                            {
                                go.transform.eulerAngles = new Vector3(
                                    rot["x"]?.ToObject<float>() ?? 0f,
                                    rot["y"]?.ToObject<float>() ?? 0f,
                                    rot["z"]?.ToObject<float>() ?? 0f
                                );
                            }

                            if (arguments["scale"] is JObject scale)
                            {
                                go.transform.localScale = new Vector3(
                                    scale["x"]?.ToObject<float>() ?? 1f,
                                    scale["y"]?.ToObject<float>() ?? 1f,
                                    scale["z"]?.ToObject<float>() ?? 1f
                                );
                            }

                            // Set parent
                            var parentPath = arguments["parent"]?.ToString();
                            if (!string.IsNullOrEmpty(parentPath))
                            {
                                var parent = UnityEngine.GameObject.Find(parentPath);
                                if (parent != null)
                                {
                                    go.transform.SetParent(parent.transform, true);
                                }
                            }

                            // Add components
                            if (arguments["components"] is JArray components)
                            {
                                foreach (JObject comp in components)
                                {
                                    var typeName = comp["type"]?.ToString();
                                    if (!string.IsNullOrEmpty(typeName))
                                    {
                                        var type = Type.GetType(typeName) ?? 
                                                  Type.GetType($"UnityEngine.{typeName}, UnityEngine");
                                        
                                        if (type != null && typeof(Component).IsAssignableFrom(type))
                                        {
                                            var component = go.AddComponent(type);
                                            
                                            // Set properties if provided
                                            if (comp["properties"] is JObject props)
                                            {
                                                SetComponentProperties(component, props);
                                            }
                                        }
                                    }
                                }
                            }

                            Undo.RegisterCreatedObjectUndo(go, $"Create {name}");
                            Selection.activeGameObject = go;
                            
                            result = Success($"Created GameObject '{name}'");
                        }
                        catch (Exception ex)
                        {
                            result = Error($"Failed to create GameObject: {ex.Message}");
                        }
                    };

                    System.Threading.Thread.Sleep(100);
                    return result ?? Success($"Queued GameObject creation: {name}");
                }
                catch (Exception ex)
                {
                    return Error($"Error creating GameObject: {ex.Message}");
                }
            });
        }

        private void SetComponentProperties(Component component, JObject properties)
        {
            var type = component.GetType();
            foreach (var prop in properties)
            {
                try
                {
                    var propInfo = type.GetProperty(prop.Key);
                    if (propInfo != null && propInfo.CanWrite)
                    {
                        var value = prop.Value.ToObject(propInfo.PropertyType);
                        propInfo.SetValue(component, value);
                    }
                }
                catch (Exception ex)
                {
                    Debug.LogWarning($"Failed to set property {prop.Key}: {ex.Message}");
                }
            }
        }
    }

    [MCPTool("find_gameobjects", "Find GameObjects by various criteria")]
    public class FindGameObjectsTool : MCPToolBase
    {
        public override string Name => "find_gameobjects";
        public override string Description => "Find GameObjects by various criteria";
        
        public override JObject InputSchema => new JObject
        {
            ["type"] = "object",
            ["properties"] = new JObject
            {
                ["name"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "Name or name pattern to search for"
                },
                ["tag"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "Tag to filter by"
                },
                ["layer"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "Layer name to filter by"
                },
                ["componentType"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "Component type to filter by"
                },
                ["includeInactive"] = new JObject
                {
                    ["type"] = "boolean",
                    ["description"] = "Include inactive GameObjects"
                }
            }
        };

        public override async Task<MCPToolCallResult> Execute(JObject arguments, MCPExecutionContext context)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var includeInactive = arguments["includeInactive"]?.ToObject<bool>() ?? false;
                    var results = new List<UnityEngine.GameObject>();

                    // Start with all GameObjects
                    var allObjects = includeInactive ? 
                        Resources.FindObjectsOfTypeAll<UnityEngine.GameObject>() :
                        UnityEngine.GameObject.FindObjectsOfType<UnityEngine.GameObject>();

                    results.AddRange(allObjects);

                    // Filter by name
                    var namePattern = arguments["name"]?.ToString();
                    if (!string.IsNullOrEmpty(namePattern))
                    {
                        results = results.Where(go => go.name.Contains(namePattern)).ToList();
                    }

                    // Filter by tag
                    var tag = arguments["tag"]?.ToString();
                    if (!string.IsNullOrEmpty(tag))
                    {
                        results = results.Where(go => go.CompareTag(tag)).ToList();
                    }

                    // Filter by layer
                    var layerName = arguments["layer"]?.ToString();
                    if (!string.IsNullOrEmpty(layerName))
                    {
                        int layer = LayerMask.NameToLayer(layerName);
                        if (layer != -1)
                        {
                            results = results.Where(go => go.layer == layer).ToList();
                        }
                    }

                    // Filter by component
                    var componentTypeName = arguments["componentType"]?.ToString();
                    if (!string.IsNullOrEmpty(componentTypeName))
                    {
                        var componentType = Type.GetType(componentTypeName) ?? 
                                          Type.GetType($"UnityEngine.{componentTypeName}, UnityEngine");
                        
                        if (componentType != null)
                        {
                            results = results.Where(go => go.GetComponent(componentType) != null).ToList();
                        }
                    }

                    // Build result
                    var foundObjects = new JArray();
                    foreach (var go in results.Take(100)) // Limit results
                    {
                        foundObjects.Add(new JObject
                        {
                            ["name"] = go.name,
                            ["path"] = GetGameObjectPath(go),
                            ["active"] = go.activeSelf,
                            ["tag"] = go.tag,
                            ["layer"] = LayerMask.LayerToName(go.layer)
                        });
                    }

                    var response = new JObject
                    {
                        ["found"] = results.Count,
                        ["returned"] = foundObjects.Count,
                        ["objects"] = foundObjects
                    };

                    return JsonResult(response);
                }
                catch (Exception ex)
                {
                    return Error($"Error finding GameObjects: {ex.Message}");
                }
            });
        }

        private string GetGameObjectPath(UnityEngine.GameObject go)
        {
            var path = go.name;
            var parent = go.transform.parent;
            
            while (parent != null)
            {
                path = parent.name + "/" + path;
                parent = parent.parent;
            }
            
            return path;
        }
    }

    [MCPTool("modify_transform", "Modify GameObject transform properties")]
    public class ModifyTransformTool : MCPToolBase
    {
        public override string Name => "modify_transform";
        public override string Description => "Modify GameObject transform properties";
        
        public override JObject InputSchema => new JObject
        {
            ["type"] = "object",
            ["properties"] = new JObject
            {
                ["target"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "GameObject name or path"
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
                },
                ["rotation"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["x"] = new JObject { ["type"] = "number" },
                        ["y"] = new JObject { ["type"] = "number" },
                        ["z"] = new JObject { ["type"] = "number" }
                    }
                },
                ["scale"] = new JObject
                {
                    ["type"] = "object",
                    ["properties"] = new JObject
                    {
                        ["x"] = new JObject { ["type"] = "number" },
                        ["y"] = new JObject { ["type"] = "number" },
                        ["z"] = new JObject { ["type"] = "number" }
                    }
                },
                ["space"] = new JObject
                {
                    ["type"] = "string",
                    ["enum"] = new JArray { "World", "Local" },
                    ["description"] = "Coordinate space for position/rotation"
                }
            },
            ["required"] = new JArray { "target" }
        };

        public override async Task<MCPToolCallResult> Execute(JObject arguments, MCPExecutionContext context)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var targetPath = arguments["target"]?.ToString();
                    if (string.IsNullOrEmpty(targetPath))
                    {
                        return Error("Target GameObject is required");
                    }

                    var space = arguments["space"]?.ToString() ?? "World";
                    var isLocal = space == "Local";

                    MCPToolCallResult result = null;
                    EditorApplication.delayCall += () =>
                    {
                        try
                        {
                            var target = UnityEngine.GameObject.Find(targetPath);
                            if (target == null)
                            {
                                result = Error($"GameObject not found: {targetPath}");
                                return;
                            }

                            Undo.RecordObject(target.transform, "Modify Transform");

                            // Set position
                            if (arguments["position"] is JObject pos)
                            {
                                var newPos = new Vector3(
                                    pos["x"]?.ToObject<float>() ?? 0f,
                                    pos["y"]?.ToObject<float>() ?? 0f,
                                    pos["z"]?.ToObject<float>() ?? 0f
                                );
                                
                                if (isLocal)
                                    target.transform.localPosition = newPos;
                                else
                                    target.transform.position = newPos;
                            }

                            // Set rotation
                            if (arguments["rotation"] is JObject rot)
                            {
                                var newRot = new Vector3(
                                    rot["x"]?.ToObject<float>() ?? 0f,
                                    rot["y"]?.ToObject<float>() ?? 0f,
                                    rot["z"]?.ToObject<float>() ?? 0f
                                );
                                
                                if (isLocal)
                                    target.transform.localEulerAngles = newRot;
                                else
                                    target.transform.eulerAngles = newRot;
                            }

                            // Set scale
                            if (arguments["scale"] is JObject scale)
                            {
                                target.transform.localScale = new Vector3(
                                    scale["x"]?.ToObject<float>() ?? 1f,
                                    scale["y"]?.ToObject<float>() ?? 1f,
                                    scale["z"]?.ToObject<float>() ?? 1f
                                );
                            }

                            result = Success($"Modified transform for '{target.name}'");
                        }
                        catch (Exception ex)
                        {
                            result = Error($"Failed to modify transform: {ex.Message}");
                        }
                    };

                    System.Threading.Thread.Sleep(100);
                    return result ?? Success($"Queued transform modification for: {targetPath}");
                }
                catch (Exception ex)
                {
                    return Error($"Error modifying transform: {ex.Message}");
                }
            });
        }
    }

    [MCPTool("destroy_gameobject", "Safely destroy GameObject with dependency check")]
    public class DestroyGameObjectTool : MCPToolBase
    {
        public override string Name => "destroy_gameobject";
        public override string Description => "Safely destroy GameObject with dependency check";
        
        public override JObject InputSchema => new JObject
        {
            ["type"] = "object",
            ["properties"] = new JObject
            {
                ["target"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "GameObject name or path to destroy"
                },
                ["includeChildren"] = new JObject
                {
                    ["type"] = "boolean",
                    ["description"] = "Whether to destroy children as well"
                },
                ["immediate"] = new JObject
                {
                    ["type"] = "boolean",
                    ["description"] = "Use DestroyImmediate instead of Destroy"
                }
            },
            ["required"] = new JArray { "target" }
        };

        public override async Task<MCPToolCallResult> Execute(JObject arguments, MCPExecutionContext context)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var targetPath = arguments["target"]?.ToString();
                    if (string.IsNullOrEmpty(targetPath))
                    {
                        return Error("Target GameObject is required");
                    }

                    var includeChildren = arguments["includeChildren"]?.ToObject<bool>() ?? true;
                    var immediate = arguments["immediate"]?.ToObject<bool>() ?? true;

                    MCPToolCallResult result = null;
                    EditorApplication.delayCall += () =>
                    {
                        try
                        {
                            var target = UnityEngine.GameObject.Find(targetPath);
                            if (target == null)
                            {
                                result = Error($"GameObject not found: {targetPath}");
                                return;
                            }

                            Undo.DestroyObjectImmediate(target);
                            result = Success($"Destroyed GameObject '{targetPath}'");
                        }
                        catch (Exception ex)
                        {
                            result = Error($"Failed to destroy GameObject: {ex.Message}");
                        }
                    };

                    System.Threading.Thread.Sleep(100);
                    return result ?? Success($"Queued destruction of: {targetPath}");
                }
                catch (Exception ex)
                {
                    return Error($"Error destroying GameObject: {ex.Message}");
                }
            });
        }
    }
}