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
            // Filter scenes to only include those within the Assets folder
            var allScenePaths = GetAllAssetsOfType(".unity");
            var projectScenePaths = allScenePaths.Where(path => path.StartsWith("Assets/")).ToList();

            var prefabs = GetAllAssetsOfType(".prefab").Where(path => path.StartsWith("Assets/")).ToList();
            var materials = GetAllAssetsOfType(".mat").Where(path => path.StartsWith("Assets/")).ToList();
            var scriptableObjects = GetAllAssetsOfType(".asset").Where(path => path.StartsWith("Assets/")).ToList();
            var folders = GetAllFolders(); // Assuming this correctly targets Assets

            var originalActiveScenePath = EditorSceneManager.GetActiveScene().path;
            SceneInfo activeSceneInfo = null;
            if (!string.IsNullOrEmpty(originalActiveScenePath) && originalActiveScenePath.StartsWith("Assets/"))
            {
                activeSceneInfo = GetSceneInfo(EditorSceneManager.GetActiveScene());
            }

            var allSceneInfos = new List<SceneInfo>();
            foreach (var path in projectScenePaths)
            {
                // Avoid re-processing the already open active scene if it's in the list
                // or handle it carefully if GetSceneInfo(string) closes scenes.
                // For simplicity, GetSceneInfo(string) will handle additive loading.
                if (path == originalActiveScenePath && activeSceneInfo != null)
                {
                    allSceneInfos.Add(activeSceneInfo); // Use already processed active scene info
                }
                else
                {
                    SceneInfo info = GetSceneInfo(path);
                    if (info != null) allSceneInfos.Add(info);
                }
            }
            // Ensure the original active scene is restored if it was closed by GetSceneInfo,
            // though additive loading should prevent this.
            if (!string.IsNullOrEmpty(originalActiveScenePath) && EditorSceneManager.GetActiveScene().path != originalActiveScenePath)
            {
                EditorSceneManager.OpenScene(originalActiveScenePath, OpenSceneMode.Single);
            }


            var prefabInfos = prefabs.Select(path => GetPrefabInfo(path)).Where(p => p != null).ToList();

            var context = new ProjectContext
            {
                ScriptPaths = scripts.Where(path => path.StartsWith("Assets/")).ToList(), // Also filter scripts
                ScenePaths = projectScenePaths,
                PrefabPaths = prefabs,
                MaterialPaths = materials,
                ScriptableObjectPaths = scriptableObjects,
                Folders = folders,
                ActiveScene = activeSceneInfo,
                AllScenes = allSceneInfos,
                Prefabs = prefabInfos
            };
            await Task.CompletedTask;
            return context;
        }

        public List<string> GetAllAssetsOfType(string extension)
        {
            var guids = AssetDatabase.FindAssets($"t:Object"); // More general search, then filter by extension
            var assets = new List<string>();
            foreach (var guid in guids)
            {
                var path = AssetDatabase.GUIDToAssetPath(guid);
                // Ensure we only consider assets and not directories, and filter by extension.
                if (!string.IsNullOrEmpty(path) && System.IO.Path.HasExtension(path) && path.EndsWith(extension))
                {
                    assets.Add(path);
                }
            }
            return assets;
        }

        public List<string> GetAllFolders()
        {
            var folders = new HashSet<string>();
            // More robust way to get all folders within Assets
            string[] assetFolder = { "Assets" };
            var allFolderGuids = AssetDatabase.FindAssets("t:Folder", assetFolder);
            foreach (var guid in allFolderGuids)
            {
                folders.Add(AssetDatabase.GUIDToAssetPath(guid));
            }

            // Get all top-level directories in Assets
            foreach (var dir in System.IO.Directory.GetDirectories(Application.dataPath))
            {
                string relativePath = "Assets/" + System.IO.Path.GetFileName(dir);
                folders.Add(relativePath); // Add top-level
                // Recursively get subdirectories
                GetAllSubFolders(relativePath, folders);
            }
            return folders.ToList();
        }

        private void GetAllSubFolders(string path, HashSet<string> folders)
        {
            try
            {
                foreach (var dir in System.IO.Directory.GetDirectories(path))
                {
                    folders.Add(dir.Replace("\\\\", "/"));
                    GetAllSubFolders(dir, folders);
                }
            }
            catch (System.Exception ex)
            {
                Debug.LogWarning($"Could not fully scan folder {path}: {ex.Message}");
            }
        }


        public SceneInfo GetSceneInfo(string scenePath)
        {
            if (string.IsNullOrEmpty(scenePath) || !scenePath.StartsWith("Assets/"))
            {
                Debug.LogWarning($"Skipping scene analysis for non-asset or invalid path: {scenePath}");
                return null;
            }

            UnityEngine.SceneManagement.Scene sceneToAnalyze;
            bool sceneWasAlreadyOpen = false;
            string originalActiveScene = EditorSceneManager.GetActiveScene().path;

            // Check if the scene is already open
            sceneToAnalyze = EditorSceneManager.GetSceneByPath(scenePath);
            if (!sceneToAnalyze.IsValid() || !sceneToAnalyze.isLoaded)
            {
                // Scene is not open or not valid, try to open it additively
                try
                {
                    sceneToAnalyze = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
                }
                catch (System.Exception ex)
                {
                    Debug.LogError($"Failed to open scene {scenePath} for analysis: {ex.Message}");
                    return null;
                }
            }
            else
            {
                sceneWasAlreadyOpen = true;
            }

            if (!sceneToAnalyze.IsValid()) return null;


            var info = new SceneInfo { Path = scenePath, GameObjects = new List<GameObjectInfo>() };
            foreach (var go in sceneToAnalyze.GetRootGameObjects())
            {
                info.GameObjects.Add(GetGameObjectInfo(go));
            }

            // Only close the scene if we opened it additively and it's not the main scene that was active.
            if (!sceneWasAlreadyOpen && sceneToAnalyze.path != originalActiveScene && EditorSceneManager.sceneCount > 1)
            {
                EditorSceneManager.CloseScene(sceneToAnalyze, true);
            }
            return info;
        }

        public SceneInfo GetSceneInfo(UnityEngine.SceneManagement.Scene scene)
        {
            if (!scene.IsValid() || !scene.isLoaded) return null; // Ensure scene is valid and loaded

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
            if (string.IsNullOrEmpty(prefabPath) || !prefabPath.StartsWith("Assets/")) return null;
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