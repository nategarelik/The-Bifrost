using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Newtonsoft.Json.Linq;
using System.Linq;
using Bifrost.Editor.AI.MCP;

namespace Bifrost.Editor.AI.MCP.Tools.Scene
{
    [MCPTool("create_scene", "Create a new scene with optional configuration")]
    public class CreateSceneTool : MCPToolBase
    {
        public override string Name => "create_scene";
        public override string Description => "Create a new scene with optional configuration";
        
        public override JObject InputSchema => new JObject
        {
            ["type"] = "object",
            ["properties"] = new JObject
            {
                ["name"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "Name of the new scene"
                },
                ["addToBuildSettings"] = new JObject
                {
                    ["type"] = "boolean",
                    ["description"] = "Whether to add the scene to build settings"
                },
                ["makeActive"] = new JObject
                {
                    ["type"] = "boolean",
                    ["description"] = "Whether to make this the active scene"
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
                    var sceneName = arguments["name"]?.ToString();
                    if (string.IsNullOrEmpty(sceneName))
                    {
                        return Error("Scene name is required");
                    }

                    var addToBuildSettings = arguments["addToBuildSettings"]?.ToObject<bool>() ?? false;
                    var makeActive = arguments["makeActive"]?.ToObject<bool>() ?? true;

                    // Execute on main thread
                    MCPToolCallResult result = null;
                    EditorApplication.delayCall += () =>
                    {
                        try
                        {
                            // Create new scene
                            var newScene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, 
                                makeActive ? NewSceneMode.Single : NewSceneMode.Additive);
                            
                            // Save the scene
                            var scenePath = $"Assets/Scenes/{sceneName}.unity";
                            
                            // Ensure directory exists
                            var directory = System.IO.Path.GetDirectoryName(scenePath);
                            if (!AssetDatabase.IsValidFolder(directory))
                            {
                                AssetDatabase.CreateFolder("Assets", "Scenes");
                            }
                            
                            EditorSceneManager.SaveScene(newScene, scenePath);

                            // Add to build settings if requested
                            if (addToBuildSettings)
                            {
                                var scenes = EditorBuildSettings.scenes.ToList();
                                scenes.Add(new EditorBuildSettingsScene(scenePath, true));
                                EditorBuildSettings.scenes = scenes.ToArray();
                            }

                            result = Success($"Created scene '{sceneName}' at {scenePath}");
                        }
                        catch (Exception ex)
                        {
                            result = Error($"Failed to create scene: {ex.Message}");
                        }
                    };

                    // Wait for execution (simplified for now)
                    System.Threading.Thread.Sleep(100);
                    return result ?? Success($"Queued scene creation: {sceneName}");
                }
                catch (Exception ex)
                {
                    return Error($"Error creating scene: {ex.Message}");
                }
            });
        }
    }

    [MCPTool("load_scene", "Load a scene by name or path")]
    public class LoadSceneTool : MCPToolBase
    {
        public override string Name => "load_scene";
        public override string Description => "Load a scene by name or path";
        
        public override JObject InputSchema => new JObject
        {
            ["type"] = "object",
            ["properties"] = new JObject
            {
                ["scene"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "Scene name or path"
                },
                ["mode"] = new JObject
                {
                    ["type"] = "string",
                    ["enum"] = new JArray { "Single", "Additive" },
                    ["description"] = "Load mode (Single replaces all scenes, Additive adds to current)"
                }
            },
            ["required"] = new JArray { "scene" }
        };

        public override async Task<MCPToolCallResult> Execute(JObject arguments, MCPExecutionContext context)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var sceneName = arguments["scene"]?.ToString();
                    if (string.IsNullOrEmpty(sceneName))
                    {
                        return Error("Scene name is required");
                    }

                    var mode = arguments["mode"]?.ToString() ?? "Single";
                    var loadMode = mode == "Additive" ? LoadSceneMode.Additive : LoadSceneMode.Single;

                    EditorApplication.delayCall += () =>
                    {
                        try
                        {
                            // Try to find scene in build settings first
                            var scenePath = sceneName;
                            if (!scenePath.EndsWith(".unity"))
                            {
                                var buildScene = EditorBuildSettings.scenes.FirstOrDefault(s => 
                                    System.IO.Path.GetFileNameWithoutExtension(s.path) == sceneName);
                                if (buildScene != null)
                                {
                                    scenePath = buildScene.path;
                                }
                            }

                            if (loadMode == LoadSceneMode.Single)
                            {
                                EditorSceneManager.OpenScene(scenePath);
                            }
                            else
                            {
                                EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Failed to load scene: {ex.Message}");
                        }
                    };

                    return Success($"Queued loading scene: {sceneName} ({mode})");
                }
                catch (Exception ex)
                {
                    return Error($"Error loading scene: {ex.Message}");
                }
            });
        }
    }

    [MCPTool("save_scene", "Save the current or specified scene")]
    public class SaveSceneTool : MCPToolBase
    {
        public override string Name => "save_scene";
        public override string Description => "Save the current or specified scene";
        
        public override JObject InputSchema => new JObject
        {
            ["type"] = "object",
            ["properties"] = new JObject
            {
                ["sceneName"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "Optional scene name to save (defaults to active scene)"
                },
                ["saveAs"] = new JObject
                {
                    ["type"] = "string",
                    ["description"] = "Optional new path to save scene as"
                }
            }
        };

        public override async Task<MCPToolCallResult> Execute(JObject arguments, MCPExecutionContext context)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var sceneName = arguments["sceneName"]?.ToString();
                    var saveAs = arguments["saveAs"]?.ToString();

                    EditorApplication.delayCall += () =>
                    {
                        try
                        {
                            UnityEngine.SceneManagement.Scene sceneToSave;
                            
                            if (!string.IsNullOrEmpty(sceneName))
                            {
                                sceneToSave = SceneManager.GetSceneByName(sceneName);
                                if (!sceneToSave.IsValid())
                                {
                                    sceneToSave = SceneManager.GetSceneByPath(sceneName);
                                }
                            }
                            else
                            {
                                sceneToSave = SceneManager.GetActiveScene();
                            }

                            if (!sceneToSave.IsValid())
                            {
                                Debug.LogError("Could not find scene to save");
                                return;
                            }

                            if (!string.IsNullOrEmpty(saveAs))
                            {
                                EditorSceneManager.SaveScene(sceneToSave, saveAs);
                            }
                            else
                            {
                                EditorSceneManager.SaveScene(sceneToSave);
                            }
                        }
                        catch (Exception ex)
                        {
                            Debug.LogError($"Failed to save scene: {ex.Message}");
                        }
                    };

                    return Success($"Queued saving scene: {sceneName ?? "active scene"}");
                }
                catch (Exception ex)
                {
                    return Error($"Error saving scene: {ex.Message}");
                }
            });
        }
    }

    [MCPTool("analyze_scene", "Get detailed analysis of the current scene")]
    public class AnalyzeSceneTool : MCPToolBase
    {
        public override string Name => "analyze_scene";
        public override string Description => "Get detailed analysis of the current scene";
        
        public override JObject InputSchema => new JObject
        {
            ["type"] = "object",
            ["properties"] = new JObject
            {
                ["includeComponents"] = new JObject
                {
                    ["type"] = "boolean",
                    ["description"] = "Include component details in analysis"
                },
                ["maxDepth"] = new JObject
                {
                    ["type"] = "integer",
                    ["description"] = "Maximum hierarchy depth to analyze"
                }
            }
        };

        public override async Task<MCPToolCallResult> Execute(JObject arguments, MCPExecutionContext context)
        {
            return await Task.Run(() =>
            {
                try
                {
                    var includeComponents = arguments["includeComponents"]?.ToObject<bool>() ?? false;
                    var maxDepth = arguments["maxDepth"]?.ToObject<int>() ?? 10;

                    var scene = SceneManager.GetActiveScene();
                    var analysis = new JObject
                    {
                        ["name"] = scene.name,
                        ["path"] = scene.path,
                        ["isDirty"] = scene.isDirty,
                        ["isLoaded"] = scene.isLoaded,
                        ["rootCount"] = scene.rootCount,
                        ["buildIndex"] = scene.buildIndex
                    };

                    // Analyze root objects
                    var rootObjects = scene.GetRootGameObjects();
                    var hierarchy = new JArray();
                    
                    foreach (var rootObj in rootObjects)
                    {
                        hierarchy.Add(AnalyzeGameObject(rootObj, includeComponents, 0, maxDepth));
                    }
                    
                    analysis["hierarchy"] = hierarchy;

                    // Scene statistics
                    var stats = new JObject
                    {
                        ["totalGameObjects"] = CountGameObjects(rootObjects),
                        ["totalComponents"] = CountComponents(rootObjects),
                        ["cameras"] = UnityEngine.GameObject.FindObjectsOfType<Camera>().Length,
                        ["lights"] = UnityEngine.GameObject.FindObjectsOfType<Light>().Length,
                        ["audioSources"] = UnityEngine.GameObject.FindObjectsOfType<AudioSource>().Length
                    };
                    
                    analysis["statistics"] = stats;

                    return JsonResult(analysis);
                }
                catch (Exception ex)
                {
                    return Error($"Error analyzing scene: {ex.Message}");
                }
            });
        }

        private JObject AnalyzeGameObject(UnityEngine.GameObject obj, bool includeComponents, int currentDepth, int maxDepth)
        {
            var result = new JObject
            {
                ["name"] = obj.name,
                ["active"] = obj.activeSelf,
                ["tag"] = obj.tag,
                ["layer"] = LayerMask.LayerToName(obj.layer),
                ["position"] = new JObject
                {
                    ["x"] = obj.transform.position.x,
                    ["y"] = obj.transform.position.y,
                    ["z"] = obj.transform.position.z
                }
            };

            if (includeComponents)
            {
                var components = new JArray();
                foreach (var component in obj.GetComponents<Component>())
                {
                    if (component != null)
                    {
                        components.Add(new JObject
                        {
                            ["type"] = component.GetType().Name,
                            ["enabled"] = component is Behaviour behaviour ? behaviour.enabled : true
                        });
                    }
                }
                result["components"] = components;
            }

            if (currentDepth < maxDepth && obj.transform.childCount > 0)
            {
                var children = new JArray();
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    children.Add(AnalyzeGameObject(obj.transform.GetChild(i).gameObject, 
                        includeComponents, currentDepth + 1, maxDepth));
                }
                result["children"] = children;
            }

            return result;
        }

        private int CountGameObjects(UnityEngine.GameObject[] rootObjects)
        {
            int count = rootObjects.Length;
            foreach (var obj in rootObjects)
            {
                count += CountChildGameObjects(obj.transform);
            }
            return count;
        }

        private int CountChildGameObjects(Transform parent)
        {
            int count = parent.childCount;
            for (int i = 0; i < parent.childCount; i++)
            {
                count += CountChildGameObjects(parent.GetChild(i));
            }
            return count;
        }

        private int CountComponents(UnityEngine.GameObject[] rootObjects)
        {
            int count = 0;
            foreach (var obj in rootObjects)
            {
                count += CountComponentsInHierarchy(obj.transform);
            }
            return count;
        }

        private int CountComponentsInHierarchy(Transform transform)
        {
            int count = transform.GetComponents<Component>().Length;
            for (int i = 0; i < transform.childCount; i++)
            {
                count += CountComponentsInHierarchy(transform.GetChild(i));
            }
            return count;
        }
    }
}