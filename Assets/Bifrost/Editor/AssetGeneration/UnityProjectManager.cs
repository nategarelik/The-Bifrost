using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
using System.IO;
#if PROBUILDER_API_EXISTS
using UnityEngine.ProBuilder;
#endif

namespace Bifrost.Editor.AssetGeneration
{
    public class UnityProjectManager
    {
        // Ensure directories exist in path
        public bool EnsureDirectoryExists(string path)
        {
            try
            {
                string directory = Path.GetDirectoryName(path);
                if (string.IsNullOrEmpty(directory))
                    return false;

                // Handle both absolute and relative paths
                if (Path.IsPathRooted(directory))
                {
                    Directory.CreateDirectory(directory);
                }
                else
                {
                    // For Unity asset paths (e.g. "Assets/Folder/SubFolder")
                    string[] pathParts = directory.Split('/');
                    string currentPath = pathParts[0]; // Usually "Assets"

                    for (int i = 1; i < pathParts.Length; i++)
                    {
                        string nextPath = currentPath + "/" + pathParts[i];
                        if (!AssetDatabase.IsValidFolder(nextPath))
                        {
                            string parentFolder = currentPath;
                            string newFolder = pathParts[i];
                            AssetDatabase.CreateFolder(parentFolder, newFolder);
                        }
                        currentPath = nextPath;
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to create directory structure: {ex.Message}");
                return false;
            }
        }

        // Create a new scene and save it
        public string CreateScene(string sceneName, string folder = "Assets/Bifrost/Runtime/BifrostGameSystems/Scenes")
        {
            try
            {
                EnsureDirectoryExists(folder + "/placeholder.tmp");
                var scene = EditorSceneManager.NewScene(NewSceneSetup.DefaultGameObjects, NewSceneMode.Single);
                string path = $"{folder}/{sceneName}.unity";
                EditorSceneManager.SaveScene(scene, path);
                AssetDatabase.Refresh();
                return path;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to create scene: {ex.Message}");
                return null;
            }
        }

        // Open an existing scene
        public bool OpenScene(string scenePath)
        {
            try
            {
                var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
                return scene.IsValid();
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to open scene: {ex.Message}");
                return false;
            }
        }

        // Save the current scene
        public bool SaveScene()
        {
            try
            {
                var scene = EditorSceneManager.GetActiveScene();
                return EditorSceneManager.SaveScene(scene);
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to save scene: {ex.Message}");
                return false;
            }
        }

        // Create a new GameObject in the current scene
        public GameObject CreateGameObject(string name, Vector3 position, PrimitiveType? primitive = null)
        {
            try
            {
                GameObject go = primitive.HasValue ? GameObject.CreatePrimitive(primitive.Value) : new GameObject(name);
                go.name = name;
                go.transform.position = position;
                Undo.RegisterCreatedObjectUndo(go, "Create GameObject");
                return go;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to create GameObject: {ex.Message}");
                return null;
            }
        }

        // Add a component to a GameObject
        public Component AddComponent(GameObject go, string componentType)
        {
            try
            {
                var type = Type.GetType(componentType) ?? Type.GetType($"UnityEngine.{componentType}, UnityEngine");
                if (type == null)
                {
                    Debug.LogError($"UnityProjectManager: Component type '{componentType}' not found.");
                    return null;
                }
                var comp = go.AddComponent(type);
                Undo.RegisterCreatedObjectUndo(comp, "Add Component");
                return comp;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to add component: {ex.Message}");
                return null;
            }
        }

        // Create a prefab from a GameObject
        public bool CreatePrefab(string prefabPath, GameObject go)
        {
            try
            {
                if (!EnsureDirectoryExists(prefabPath))
                {
                    Debug.LogError($"UnityProjectManager: Failed to create directory structure for prefab: {prefabPath}");
                    return false;
                }

                var prefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
                AssetDatabase.Refresh();
                return prefab != null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to create prefab: {ex.Message}");
                return false;
            }
        }

        // Place an asset (e.g., model, prefab) in the current scene
        public GameObject PlaceAssetInScene(string assetPath, Vector3 position)
        {
            try
            {
                var asset = AssetDatabase.LoadAssetAtPath<GameObject>(assetPath);
                if (asset == null)
                {
                    Debug.LogError($"UnityProjectManager: Asset not found at {assetPath}");
                    return null;
                }
                var go = (GameObject)PrefabUtility.InstantiatePrefab(asset);
                go.transform.position = position;
                Undo.RegisterCreatedObjectUndo(go, "Place Asset In Scene");
                return go;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to place asset in scene: {ex.Message}");
                return null;
            }
        }

        // ProBuilder integration: create a shape if ProBuilder is present
        public void CreateProBuilderShape(string shapeType, Vector3 position)
        {
#if PROBUILDER_API_EXISTS
            // Example: create a cube
            if (shapeType.ToLower() == "cube")
            {
                var pb = ProBuilderMesh.CreateShape(ShapeType.Cube);
                pb.transform.position = position;
                Undo.RegisterCreatedObjectUndo(pb.gameObject, "Create ProBuilder Cube");
            }
            else
            {
                Debug.LogWarning($"UnityProjectManager: Shape type '{shapeType}' not supported by ProBuilder stub.");
            }
#else
            Debug.LogWarning("UnityProjectManager: ProBuilder integration not available. Please install ProBuilder package.");
#endif
        }

        // Utility: Duplicate a prefab
        public string DuplicatePrefab(string sourcePath, string destPath)
        {
            try
            {
                var prefab = AssetDatabase.LoadAssetAtPath<GameObject>(sourcePath);
                if (prefab == null)
                {
                    Debug.LogError($"UnityProjectManager: Source prefab not found at {sourcePath}");
                    return null;
                }

                if (!EnsureDirectoryExists(destPath))
                {
                    Debug.LogError($"UnityProjectManager: Failed to create directory structure for duplicated prefab: {destPath}");
                    return null;
                }

                var newPrefab = UnityEngine.Object.Instantiate(prefab);
                var resultPath = CreatePrefab(destPath, newPrefab) ? destPath : null;
                UnityEngine.Object.DestroyImmediate(newPrefab);
                return resultPath;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to duplicate prefab: {ex.Message}");
                return null;
            }
        }

        public bool CreateScript(string path, string content)
        {
            try
            {
                if (!EnsureDirectoryExists(path))
                {
                    Debug.LogError($"UnityProjectManager: Failed to create directory structure for script: {path}");
                    return false;
                }

                System.IO.File.WriteAllText(path, content);
                AssetDatabase.ImportAsset(path);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to create script: {ex.Message}");
                return false;
            }
        }

        public bool CreateUI(string path, string template)
        {
            try
            {
                if (!EnsureDirectoryExists(path))
                {
                    Debug.LogError($"UnityProjectManager: Failed to create directory structure for UI: {path}");
                    return false;
                }

                System.IO.File.WriteAllText(path, template ?? "");
                AssetDatabase.ImportAsset(path);
                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to create UI: {ex.Message}");
                return false;
            }
        }

        public bool CreateScriptableObject(string path, string typeName, string jsonData)
        {
            try
            {
                if (!EnsureDirectoryExists(path))
                {
                    Debug.LogError($"UnityProjectManager: Failed to create directory structure for ScriptableObject: {path}");
                    return false;
                }
                var type = System.Type.GetType(typeName);
                if (type == null || !typeof(ScriptableObject).IsAssignableFrom(type))
                {
                    Debug.LogError($"UnityProjectManager: Type '{typeName}' is not a valid ScriptableObject type.");
                    return false;
                }
                var so = ScriptableObject.CreateInstance(type);
                if (!string.IsNullOrEmpty(jsonData))
                {
                    JsonUtility.FromJsonOverwrite(jsonData, so);
                }
                AssetDatabase.CreateAsset(so, path);
                AssetDatabase.SaveAssets();
                return true;
            }
            catch (System.Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to create ScriptableObject: {ex.Message}");
                return false;
            }
        }
    }
}