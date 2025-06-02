using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine.SceneManagement;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace Bifrost.Editor.AI.Unity
{
    public class UnityStateSynchronizer
    {
        private readonly MCP.MCPServer server;
        private readonly List<StateChangeSubscriber> subscribers = new List<StateChangeSubscriber>();
        private bool isInitialized = false;

        public UnityStateSynchronizer(MCP.MCPServer server)
        {
            this.server = server;
        }

        public void Initialize()
        {
            if (isInitialized) return;

            // Subscribe to Unity Editor events
            EditorApplication.hierarchyChanged += OnHierarchyChanged;
            EditorApplication.projectChanged += OnProjectChanged;
            EditorSceneManager.sceneOpened += OnSceneOpened;
            EditorSceneManager.sceneClosed += OnSceneClosed;
            EditorSceneManager.sceneLoaded += OnSceneLoaded;
            EditorSceneManager.sceneSaved += OnSceneSaved;
            Selection.selectionChanged += OnSelectionChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
            Undo.undoRedoPerformed += OnUndoRedoPerformed;

            isInitialized = true;
            Debug.Log("Unity State Synchronizer initialized");
        }

        public void Shutdown()
        {
            if (!isInitialized) return;

            // Unsubscribe from Unity Editor events
            EditorApplication.hierarchyChanged -= OnHierarchyChanged;
            EditorApplication.projectChanged -= OnProjectChanged;
            EditorSceneManager.sceneOpened -= OnSceneOpened;
            EditorSceneManager.sceneClosed -= OnSceneClosed;
            EditorSceneManager.sceneLoaded -= OnSceneLoaded;
            EditorSceneManager.sceneSaved -= OnSceneSaved;
            Selection.selectionChanged -= OnSelectionChanged;
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            Undo.undoRedoPerformed -= OnUndoRedoPerformed;

            isInitialized = false;
            Debug.Log("Unity State Synchronizer shutdown");
        }

        public void Subscribe(StateChangeSubscriber subscriber)
        {
            if (!subscribers.Contains(subscriber))
            {
                subscribers.Add(subscriber);
            }
        }

        public void Unsubscribe(StateChangeSubscriber subscriber)
        {
            subscribers.Remove(subscriber);
        }

        private void BroadcastStateChange(StateChangeEvent change)
        {
            // Log the state change
            server?.NotifyLog($"State change: {change.Type} - {change.Description}");

            // Notify all subscribers
            foreach (var subscriber in subscribers)
            {
                try
                {
                    subscriber.OnStateChanged(change);
                }
                catch (Exception ex)
                {
                    Debug.LogError($"Error notifying subscriber: {ex.Message}");
                }
            }

            // If MCP server is running, broadcast to connected clients
            if (server != null && server.IsRunning)
            {
                // This would send updates to connected MCP clients
                // Implementation depends on how we want to handle real-time updates
            }
        }

        private void OnHierarchyChanged()
        {
            BroadcastStateChange(new StateChangeEvent
            {
                Type = StateChangeType.HierarchyChanged,
                Description = "Scene hierarchy has changed",
                Timestamp = DateTime.Now,
                Data = GetSceneHierarchySummary()
            });
        }

        private void OnProjectChanged()
        {
            BroadcastStateChange(new StateChangeEvent
            {
                Type = StateChangeType.ProjectChanged,
                Description = "Project assets have changed",
                Timestamp = DateTime.Now
            });
        }

        private void OnSceneOpened(Scene scene, OpenSceneMode mode)
        {
            BroadcastStateChange(new StateChangeEvent
            {
                Type = StateChangeType.SceneOpened,
                Description = $"Scene opened: {scene.name} ({mode})",
                Timestamp = DateTime.Now,
                Data = new JObject
                {
                    ["sceneName"] = scene.name,
                    ["scenePath"] = scene.path,
                    ["mode"] = mode.ToString()
                }
            });
        }

        private void OnSceneClosed(Scene scene)
        {
            BroadcastStateChange(new StateChangeEvent
            {
                Type = StateChangeType.SceneClosed,
                Description = $"Scene closed: {scene.name}",
                Timestamp = DateTime.Now,
                Data = new JObject
                {
                    ["sceneName"] = scene.name,
                    ["scenePath"] = scene.path
                }
            });
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            BroadcastStateChange(new StateChangeEvent
            {
                Type = StateChangeType.SceneLoaded,
                Description = $"Scene loaded: {scene.name} ({mode})",
                Timestamp = DateTime.Now,
                Data = new JObject
                {
                    ["sceneName"] = scene.name,
                    ["scenePath"] = scene.path,
                    ["mode"] = mode.ToString()
                }
            });
        }

        private void OnSceneSaved(Scene scene)
        {
            BroadcastStateChange(new StateChangeEvent
            {
                Type = StateChangeType.SceneSaved,
                Description = $"Scene saved: {scene.name}",
                Timestamp = DateTime.Now,
                Data = new JObject
                {
                    ["sceneName"] = scene.name,
                    ["scenePath"] = scene.path
                }
            });
        }

        private void OnSelectionChanged()
        {
            BroadcastStateChange(new StateChangeEvent
            {
                Type = StateChangeType.SelectionChanged,
                Description = "Selection has changed",
                Timestamp = DateTime.Now,
                Data = GetSelectionSummary()
            });
        }

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            BroadcastStateChange(new StateChangeEvent
            {
                Type = StateChangeType.PlayModeChanged,
                Description = $"Play mode state changed: {state}",
                Timestamp = DateTime.Now,
                Data = new JObject
                {
                    ["state"] = state.ToString(),
                    ["isPlaying"] = EditorApplication.isPlaying,
                    ["isPaused"] = EditorApplication.isPaused
                }
            });
        }

        private void OnUndoRedoPerformed()
        {
            BroadcastStateChange(new StateChangeEvent
            {
                Type = StateChangeType.UndoRedoPerformed,
                Description = "Undo/Redo operation performed",
                Timestamp = DateTime.Now,
                Data = new JObject
                {
                    ["undoName"] = Undo.GetCurrentUndoName()
                }
            });
        }

        private JObject GetSceneHierarchySummary()
        {
            var scene = SceneManager.GetActiveScene();
            return new JObject
            {
                ["sceneName"] = scene.name,
                ["rootCount"] = scene.rootCount,
                ["isDirty"] = scene.isDirty,
                ["gameObjectCount"] = GameObject.FindObjectsOfType<GameObject>().Length
            };
        }

        private JObject GetSelectionSummary()
        {
            return new JObject
            {
                ["selectedCount"] = Selection.objects.Length,
                ["activeObjectName"] = Selection.activeObject?.name,
                ["activeGameObjectName"] = Selection.activeGameObject?.name,
                ["selectionType"] = Selection.objects.Length > 0 ? Selection.objects[0].GetType().Name : "None"
            };
        }
    }

    public class StateChangeEvent
    {
        public StateChangeType Type { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public JObject Data { get; set; }
    }

    public enum StateChangeType
    {
        HierarchyChanged,
        ProjectChanged,
        SceneOpened,
        SceneClosed,
        SceneLoaded,
        SceneSaved,
        SelectionChanged,
        PlayModeChanged,
        UndoRedoPerformed,
        AssetImported,
        AssetDeleted,
        AssetModified
    }

    public interface StateChangeSubscriber
    {
        void OnStateChanged(StateChangeEvent change);
    }
}