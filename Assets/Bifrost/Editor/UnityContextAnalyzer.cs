using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace Bifrost.Editor
{
    /// <summary>
    /// Analyzes the Unity project state and provides structured context for the AI agent.
    /// </summary>
    public class UnityContextAnalyzer
    {
        public async Task<ProjectContext> AnalyzeProjectAsync()
        {
            var scripts = GetAllAssetsOfType(".cs");
            var scenes = GetAllAssetsOfType(".unity");
            var prefabs = GetAllAssetsOfType(".prefab");
            var materials = GetAllAssetsOfType(".mat");
            var scriptableObjects = GetAllAssetsOfType(".asset");
            var folders = GetAllFolders();
            var activeScene = EditorSceneManager.GetActiveScene();
            var sceneInfo = GetSceneInfo(activeScene);
            var allSceneInfos = scenes.Select(path => GetSceneInfo(path)).ToList();
            var prefabInfos = prefabs.Select(path => GetPrefabInfo(path)).ToList();

            var context = new ProjectContext
            {
                ScriptPaths = scripts,
                ScenePaths = scenes,
                PrefabPaths = prefabs,
                MaterialPaths = materials,
                ScriptableObjectPaths = scriptableObjects,
                Folders = folders,
                ActiveScene = sceneInfo,
                AllScenes = allSceneInfos,
                Prefabs = prefabInfos
            };
            await Task.CompletedTask;
            return context;
        }

        public List<string> GetAllAssetsOfType(string extension)
        {
            var guids = AssetDatabase.FindAssets("");
            var assets = new List<string>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (path.EndsWith(extension))
                {
                    assets.Add(path);
                }
            }
            return assets;
        }

        public List<string> GetAllFolders()
        {
            var guids = AssetDatabase.FindAssets("", new[] { "Assets" });
            var folders = new HashSet<string>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                if (AssetDatabase.IsValidFolder(path))
                    folders.Add(path);
            }
            return folders.ToList();
        }

        public SceneInfo GetSceneInfo(string scenePath)
        {
            var info = new SceneInfo { Path = scenePath, GameObjects = new List<GameObjectInfo>() };
            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            foreach (var go in scene.GetRootGameObjects())
            {
                info.GameObjects.Add(GetGameObjectInfo(go));
            }
            EditorSceneManager.CloseScene(scene, true);
            return info;
        }

        public SceneInfo GetSceneInfo(UnityEngine.SceneManagement.Scene scene)
        {
            var info = new SceneInfo { Path = scene.path, GameObjects = new List<GameObjectInfo>() };
            foreach (var go in scene.GetRootGameObjects())
            {
                info.GameObjects.Add(GetGameObjectInfo(go));
            }
            return info;
        }

        public GameObjectInfo GetGameObjectInfo(GameObject go)
        {
            var info = new GameObjectInfo
            {
                Name = go.name,
                Tag = go.tag,
                Layer = go.layer,
                Components = go.GetComponents<Component>().Select(c => c.GetType().Name).ToList(),
                Children = new List<GameObjectInfo>()
            };
            foreach (Transform child in go.transform)
            {
                info.Children.Add(GetGameObjectInfo(child.gameObject));
            }
            return info;
        }

        public PrefabInfo GetPrefabInfo(string prefabPath)
        {
            var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);
            if (prefab == null) return null;
            return new PrefabInfo
            {
                Path = prefabPath,
                Name = prefab.name,
                Components = prefab.GetComponents<Component>().Select(c => c.GetType().Name).ToList(),
                ChildCount = prefab.transform.childCount
            };
        }
    }

    /// <summary>
    /// Represents a summary of the Unity project state for AI context
    /// </summary>
    public class ProjectContext
    {
        public List<string> ScriptPaths;
        public List<string> ScenePaths;
        public List<string> PrefabPaths;
        public List<string> MaterialPaths;
        public List<string> ScriptableObjectPaths;
        public List<string> Folders;
        public SceneInfo ActiveScene;
        public List<SceneInfo> AllScenes;
        public List<PrefabInfo> Prefabs;
    }

    public class SceneInfo
    {
        public string Path;
        public List<GameObjectInfo> GameObjects;
    }

    public class GameObjectInfo
    {
        public string Name;
        public string Tag;
        public int Layer;
        public List<string> Components;
        public List<GameObjectInfo> Children;
    }

    public class PrefabInfo
    {
        public string Path;
        public string Name;
        public List<string> Components;
        public int ChildCount;
    }
} 