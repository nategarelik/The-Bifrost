using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using System;
#if PROBUILDER_API_EXISTS
using UnityEngine.ProBuilder;
#endif

namespace Bifrost.Editor
{
    public class UnityProjectManager
    {
        // Create a new scene and save it
        public string CreateScene(string sceneName, string folder = "Assets/Bifrost/Runtime/BifrostGameSystems/Scenes")
        {
            try
            {
                if (!AssetDatabase.IsValidFolder(folder))
                    AssetDatabase.CreateFolder("Assets/Bifrost/Runtime/BifrostGameSystems", "Scenes");
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
        public string CreatePrefab(string prefabPath, GameObject go)
        {
            try
            {
                string folder = System.IO.Path.GetDirectoryName(prefabPath);
                if (!AssetDatabase.IsValidFolder(folder))
                    AssetDatabase.CreateFolder(System.IO.Path.GetDirectoryName(folder), System.IO.Path.GetFileName(folder));
                var prefab = PrefabUtility.SaveAsPrefabAsset(go, prefabPath);
                AssetDatabase.Refresh();
                return prefab != null ? prefabPath : null;
            }
            catch (Exception ex)
            {
                Debug.LogError($"UnityProjectManager: Failed to create prefab: {ex.Message}");
                return null;
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
                var newPrefab = UnityEngine.Object.Instantiate(prefab);
                var resultPath = CreatePrefab(destPath, newPrefab);
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
                string directory = System.IO.Path.GetDirectoryName(path);
                if (!System.IO.Directory.Exists(directory))
                    System.IO.Directory.CreateDirectory(directory);
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
                string directory = System.IO.Path.GetDirectoryName(path);
                if (!System.IO.Directory.Exists(directory))
                    System.IO.Directory.CreateDirectory(directory);
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
    }
}