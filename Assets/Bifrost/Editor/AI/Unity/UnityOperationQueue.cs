using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;

namespace Bifrost.Editor.AI.Unity
{
    public interface IUnityOperation
    {
        string Description { get; }
        void Execute();
        bool IsCompleted { get; }
        Exception Error { get; }
    }

    public class UnityOperation<T> : IUnityOperation
    {
        private readonly Func<T> operation;
        private readonly TaskCompletionSource<T> completionSource;
        private bool isCompleted;
        private Exception error;

        public string Description { get; }
        public bool IsCompleted => isCompleted;
        public Exception Error => error;
        public Task<T> Task => completionSource.Task;

        public UnityOperation(Func<T> operation, string description = null)
        {
            this.operation = operation;
            this.Description = description ?? "Unity Operation";
            this.completionSource = new TaskCompletionSource<T>();
        }

        public void Execute()
        {
            try
            {
                var result = operation();
                completionSource.SetResult(result);
                isCompleted = true;
            }
            catch (Exception ex)
            {
                error = ex;
                completionSource.SetException(ex);
                isCompleted = true;
                Debug.LogError($"Unity operation failed: {Description} - {ex.Message}");
            }
        }
    }

    public class UnityOperationQueue
    {
        private readonly Queue<IUnityOperation> mainThreadQueue = new Queue<IUnityOperation>();
        private readonly Queue<IUnityOperation> backgroundQueue = new Queue<IUnityOperation>();
        private readonly object mainThreadLock = new object();
        private readonly object backgroundLock = new object();
        
        private static UnityOperationQueue instance;
        private bool isProcessing;
        private Thread backgroundThread;
        private CancellationTokenSource cancellationTokenSource;

        public static UnityOperationQueue Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new UnityOperationQueue();
                    instance.Initialize();
                }
                return instance;
            }
        }

        private UnityOperationQueue() { }

        private void Initialize()
        {
            EditorApplication.update += ProcessMainThreadQueue;
            
            cancellationTokenSource = new CancellationTokenSource();
            backgroundThread = new Thread(ProcessBackgroundQueue)
            {
                Name = "Bifrost Background Operations",
                IsBackground = true
            };
            backgroundThread.Start();
            
            isProcessing = true;
            Debug.Log("Unity Operation Queue initialized");
        }

        public void Shutdown()
        {
            EditorApplication.update -= ProcessMainThreadQueue;
            
            cancellationTokenSource?.Cancel();
            backgroundThread?.Join(1000);
            
            isProcessing = false;
            instance = null;
            Debug.Log("Unity Operation Queue shutdown");
        }

        public Task<T> EnqueueMainThread<T>(Func<T> operation, string description = null)
        {
            if (!isProcessing)
            {
                throw new InvalidOperationException("Operation queue is not initialized");
            }

            var unityOp = new UnityOperation<T>(operation, description);
            
            lock (mainThreadLock)
            {
                mainThreadQueue.Enqueue(unityOp);
            }
            
            return unityOp.Task;
        }

        public Task EnqueueMainThread(Action operation, string description = null)
        {
            return EnqueueMainThread(() =>
            {
                operation();
                return true;
            }, description);
        }

        public Task<T> EnqueueBackground<T>(Func<T> operation, string description = null)
        {
            if (!isProcessing)
            {
                throw new InvalidOperationException("Operation queue is not initialized");
            }

            var unityOp = new UnityOperation<T>(operation, description);
            
            lock (backgroundLock)
            {
                backgroundQueue.Enqueue(unityOp);
            }
            
            return unityOp.Task;
        }

        public Task EnqueueBackground(Action operation, string description = null)
        {
            return EnqueueBackground(() =>
            {
                operation();
                return true;
            }, description);
        }

        private void ProcessMainThreadQueue()
        {
            if (!isProcessing) return;

            List<IUnityOperation> operations = null;
            
            lock (mainThreadLock)
            {
                if (mainThreadQueue.Count > 0)
                {
                    operations = new List<IUnityOperation>();
                    int count = Math.Min(mainThreadQueue.Count, 10); // Process up to 10 operations per frame
                    
                    for (int i = 0; i < count; i++)
                    {
                        operations.Add(mainThreadQueue.Dequeue());
                    }
                }
            }
            
            if (operations != null)
            {
                foreach (var op in operations)
                {
                    try
                    {
                        op.Execute();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to execute main thread operation: {op.Description} - {ex.Message}");
                    }
                }
            }
        }

        private void ProcessBackgroundQueue()
        {
            var token = cancellationTokenSource.Token;
            
            while (!token.IsCancellationRequested)
            {
                IUnityOperation operation = null;
                
                lock (backgroundLock)
                {
                    if (backgroundQueue.Count > 0)
                    {
                        operation = backgroundQueue.Dequeue();
                    }
                }
                
                if (operation != null)
                {
                    try
                    {
                        operation.Execute();
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Failed to execute background operation: {operation.Description} - {ex.Message}");
                    }
                }
                else
                {
                    Thread.Sleep(10); // Small delay when queue is empty
                }
            }
        }

        public int MainThreadQueueCount
        {
            get
            {
                lock (mainThreadLock)
                {
                    return mainThreadQueue.Count;
                }
            }
        }

        public int BackgroundQueueCount
        {
            get
            {
                lock (backgroundLock)
                {
                    return backgroundQueue.Count;
                }
            }
        }

        public void ClearQueues()
        {
            lock (mainThreadLock)
            {
                mainThreadQueue.Clear();
            }
            
            lock (backgroundLock)
            {
                backgroundQueue.Clear();
            }
            
            Debug.Log("Operation queues cleared");
        }
    }

    // Helper class for common Unity operations
    public static class UnityOperations
    {
        public static Task<GameObject> CreateGameObjectAsync(string name, PrimitiveType? primitiveType = null)
        {
            return UnityOperationQueue.Instance.EnqueueMainThread(() =>
            {
                GameObject go;
                if (primitiveType.HasValue)
                {
                    go = GameObject.CreatePrimitive(primitiveType.Value);
                    go.name = name;
                }
                else
                {
                    go = new GameObject(name);
                }
                
                Undo.RegisterCreatedObjectUndo(go, $"Create {name}");
                return go;
            }, $"Create GameObject: {name}");
        }

        public static Task<T> AddComponentAsync<T>(GameObject target) where T : Component
        {
            return UnityOperationQueue.Instance.EnqueueMainThread(() =>
            {
                return target.AddComponent<T>();
            }, $"Add Component: {typeof(T).Name}");
        }

        public static Task SaveSceneAsync(Scene scene)
        {
            return UnityOperationQueue.Instance.EnqueueMainThread(() =>
            {
                EditorSceneManager.SaveScene(scene);
            }, $"Save Scene: {scene.name}");
        }

        public static Task<Scene> LoadSceneAsync(string scenePath, LoadSceneMode mode = LoadSceneMode.Single)
        {
            return UnityOperationQueue.Instance.EnqueueMainThread(() =>
            {
                return mode == LoadSceneMode.Single
                    ? EditorSceneManager.OpenScene(scenePath)
                    : EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Additive);
            }, $"Load Scene: {scenePath}");
        }

        public static Task RefreshAssetDatabaseAsync()
        {
            return UnityOperationQueue.Instance.EnqueueMainThread(() =>
            {
                AssetDatabase.Refresh();
            }, "Refresh Asset Database");
        }
    }
}