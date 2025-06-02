using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using UnityEngine.SceneManagement;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace Bifrost.Editor.AI.MCP.Resources
{
    public class SceneHierarchyResource : MCPResourceBase
    {
        public override string Uri => "unity://scene/hierarchy";
        public override string Name => "Scene Hierarchy";
        public override string Description => "Current Unity scene hierarchy";
        public override string MimeType => "application/json";

        public override async Task<MCPResourceReadResult> Read(MCPResourceReadParams parameters)
        {
            return await Task.Run(() =>
            {
                var scene = SceneManager.GetActiveScene();
                var hierarchy = new JObject
                {
                    ["sceneName"] = scene.name,
                    ["scenePath"] = scene.path,
                    ["rootObjects"] = new JArray()
                };

                var rootObjects = scene.GetRootGameObjects();
                foreach (var rootObj in rootObjects)
                {
                    ((JArray)hierarchy["rootObjects"]).Add(SerializeGameObject(rootObj, 0, 5));
                }

                return JsonResult(hierarchy);
            });
        }

        private JObject SerializeGameObject(GameObject obj, int depth, int maxDepth)
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
                },
                ["components"] = new JArray()
            };

            // Add component list
            foreach (var component in obj.GetComponents<Component>())
            {
                if (component != null)
                {
                    ((JArray)result["components"]).Add(new JObject
                    {
                        ["type"] = component.GetType().Name,
                        ["enabled"] = component is Behaviour b ? b.enabled : true
                    });
                }
            }

            // Add children if within depth limit
            if (depth < maxDepth && obj.transform.childCount > 0)
            {
                result["children"] = new JArray();
                for (int i = 0; i < obj.transform.childCount; i++)
                {
                    var child = obj.transform.GetChild(i).gameObject;
                    ((JArray)result["children"]).Add(SerializeGameObject(child, depth + 1, maxDepth));
                }
            }

            return result;
        }
    }

    public class ProjectStructureResource : MCPResourceBase
    {
        public override string Uri => "unity://project/structure";
        public override string Name => "Project Structure";
        public override string Description => "Unity project structure and assets";
        public override string MimeType => "application/json";

        public override async Task<MCPResourceReadResult> Read(MCPResourceReadParams parameters)
        {
            return await Task.Run(() =>
            {
                var structure = new JObject
                {
                    ["projectName"] = Application.productName,
                    ["unityVersion"] = Application.unityVersion,
                    ["projectPath"] = Application.dataPath,
                    ["scenes"] = GetSceneList(),
                    ["assets"] = GetAssetsSummary()
                };

                return JsonResult(structure);
            });
        }

        private JArray GetSceneList()
        {
            var scenes = new JArray();
            foreach (var scene in EditorBuildSettings.scenes)
            {
                scenes.Add(new JObject
                {
                    ["path"] = scene.path,
                    ["name"] = System.IO.Path.GetFileNameWithoutExtension(scene.path),
                    ["enabled"] = scene.enabled
                });
            }
            return scenes;
        }

        private JObject GetAssetsSummary()
        {
            return new JObject
            {
                ["scripts"] = AssetDatabase.FindAssets("t:Script").Length,
                ["prefabs"] = AssetDatabase.FindAssets("t:Prefab").Length,
                ["materials"] = AssetDatabase.FindAssets("t:Material").Length,
                ["textures"] = AssetDatabase.FindAssets("t:Texture").Length,
                ["models"] = AssetDatabase.FindAssets("t:Model").Length,
                ["audioClips"] = AssetDatabase.FindAssets("t:AudioClip").Length,
                ["animations"] = AssetDatabase.FindAssets("t:AnimationClip").Length
            };
        }
    }

    public class SelectionResource : MCPResourceBase
    {
        public override string Uri => "unity://selection";
        public override string Name => "Current Selection";
        public override string Description => "Currently selected objects in Unity";
        public override string MimeType => "application/json";

        public override async Task<MCPResourceReadResult> Read(MCPResourceReadParams parameters)
        {
            return await Task.Run(() =>
            {
                var selection = new JObject
                {
                    ["activeObject"] = Selection.activeObject?.name,
                    ["activeGameObject"] = Selection.activeGameObject?.name,
                    ["gameObjects"] = new JArray(),
                    ["objects"] = new JArray()
                };

                foreach (var go in Selection.gameObjects)
                {
                    ((JArray)selection["gameObjects"]).Add(new JObject
                    {
                        ["name"] = go.name,
                        ["path"] = GetGameObjectPath(go),
                        ["type"] = go.GetType().Name
                    });
                }

                foreach (var obj in Selection.objects)
                {
                    ((JArray)selection["objects"]).Add(new JObject
                    {
                        ["name"] = obj.name,
                        ["type"] = obj.GetType().Name,
                        ["assetPath"] = AssetDatabase.GetAssetPath(obj)
                    });
                }

                return JsonResult(selection);
            });
        }

        private string GetGameObjectPath(GameObject go)
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

    public class ConsoleLogsResource : MCPResourceBase
    {
        private static readonly List<LogEntry> logBuffer = new List<LogEntry>();
        private const int MaxLogEntries = 1000;

        static ConsoleLogsResource()
        {
            Application.logMessageReceived += OnLogMessageReceived;
        }

        public override string Uri => "unity://console/logs";
        public override string Name => "Console Logs";
        public override string Description => "Unity console log messages";
        public override string MimeType => "application/json";

        public override async Task<MCPResourceReadResult> Read(MCPResourceReadParams parameters)
        {
            return await Task.Run(() =>
            {
                var logs = new JArray();
                
                lock (logBuffer)
                {
                    foreach (var entry in logBuffer.TakeLast(100))
                    {
                        logs.Add(new JObject
                        {
                            ["timestamp"] = entry.Timestamp.ToString("yyyy-MM-dd HH:mm:ss"),
                            ["type"] = entry.Type.ToString(),
                            ["message"] = entry.Message,
                            ["stackTrace"] = entry.StackTrace
                        });
                    }
                }

                return JsonResult(new JObject
                {
                    ["totalLogs"] = logBuffer.Count,
                    ["returnedLogs"] = logs.Count,
                    ["logs"] = logs
                });
            });
        }

        private static void OnLogMessageReceived(string message, string stackTrace, LogType type)
        {
            lock (logBuffer)
            {
                logBuffer.Add(new LogEntry
                {
                    Timestamp = DateTime.Now,
                    Type = type,
                    Message = message,
                    StackTrace = stackTrace
                });

                if (logBuffer.Count > MaxLogEntries)
                {
                    logBuffer.RemoveAt(0);
                }
            }
        }

        private class LogEntry
        {
            public DateTime Timestamp { get; set; }
            public LogType Type { get; set; }
            public string Message { get; set; }
            public string StackTrace { get; set; }
        }
    }

    public class BuildSettingsResource : MCPResourceBase
    {
        public override string Uri => "unity://build/settings";
        public override string Name => "Build Settings";
        public override string Description => "Current Unity build configuration";
        public override string MimeType => "application/json";

        public override async Task<MCPResourceReadResult> Read(MCPResourceReadParams parameters)
        {
            return await Task.Run(() =>
            {
                var settings = new JObject
                {
                    ["activeBuildTarget"] = EditorUserBuildSettings.activeBuildTarget.ToString(),
                    ["selectedBuildTargetGroup"] = EditorUserBuildSettings.selectedBuildTargetGroup.ToString(),
                    ["developmentBuild"] = EditorUserBuildSettings.development,
                    ["buildSceneList"] = new JArray(),
                    ["playerSettings"] = GetPlayerSettings()
                };

                foreach (var scene in EditorBuildSettings.scenes)
                {
                    ((JArray)settings["buildSceneList"]).Add(new JObject
                    {
                        ["path"] = scene.path,
                        ["enabled"] = scene.enabled,
                        ["name"] = System.IO.Path.GetFileNameWithoutExtension(scene.path)
                    });
                }

                return JsonResult(settings);
            });
        }

        private JObject GetPlayerSettings()
        {
            return new JObject
            {
                ["companyName"] = PlayerSettings.companyName,
                ["productName"] = PlayerSettings.productName,
                ["applicationIdentifier"] = PlayerSettings.applicationIdentifier,
                ["defaultScreenWidth"] = PlayerSettings.defaultScreenWidth,
                ["defaultScreenHeight"] = PlayerSettings.defaultScreenHeight,
                ["fullscreen"] = PlayerSettings.fullScreenMode.ToString()
            };
        }
    }

    public class AssetsListResource : MCPResourceBase
    {
        public override string Uri => "unity://assets/list";
        public override string Name => "Assets List";
        public override string Description => "List of project assets by type";
        public override string MimeType => "application/json";

        public override async Task<MCPResourceReadResult> Read(MCPResourceReadParams parameters)
        {
            return await Task.Run(() =>
            {
                var assetType = parameters.Uri.Contains("?type=") ? 
                    parameters.Uri.Split(new[] { "?type=" }, StringSplitOptions.None)[1] : null;

                var filter = string.IsNullOrEmpty(assetType) ? "" : $"t:{assetType}";
                var guids = AssetDatabase.FindAssets(filter);
                
                var assets = new JArray();
                foreach (var guid in guids.Take(100)) // Limit results
                {
                    var path = AssetDatabase.GUIDToAssetPath(guid);
                    var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(path);
                    
                    if (asset != null)
                    {
                        assets.Add(new JObject
                        {
                            ["name"] = asset.name,
                            ["type"] = asset.GetType().Name,
                            ["path"] = path,
                            ["guid"] = guid
                        });
                    }
                }

                return JsonResult(new JObject
                {
                    ["totalAssets"] = guids.Length,
                    ["returnedAssets"] = assets.Count,
                    ["filterType"] = assetType ?? "all",
                    ["assets"] = assets
                });
            });
        }
    }
}