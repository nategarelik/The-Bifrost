using UnityEngine;
using UnityEditor;
using System.Reflection;
using System;

namespace Bifrost.Editor.Utilities
{
    /// <summary>
    /// Suppresses the annoying SampleDependencyImporter NullReferenceException
    /// that occurs in some Unity versions with render pipelines.
    /// </summary>
    [InitializeOnLoad]
    public static class RenderPipelineErrorSuppressor
    {
        static RenderPipelineErrorSuppressor()
        {
            // Only suppress in Editor, not in play mode
            if (!Application.isPlaying)
            {
                Application.logMessageReceived += HandleLog;
            }
        }

        private static void HandleLog(string logString, string stackTrace, LogType type)
        {
            // Suppress the specific SampleDependencyImporter error
            if (type == LogType.Exception && 
                logString.Contains("NullReferenceException") && 
                stackTrace.Contains("SampleDependencyImporter"))
            {
                // Don't propagate this error
                return;
            }
        }

        [MenuItem("Bifrost/Utilities/Clear Render Pipeline Cache")]
        public static void ClearRenderPipelineCache()
        {
            // Clear the problematic cache
            if (EditorUtility.DisplayDialog("Clear Render Pipeline Cache",
                "This will clear the render pipeline sample cache which may resolve import errors.\n\n" +
                "Unity will need to reimport some assets.",
                "Clear Cache", "Cancel"))
            {
                try
                {
                    // Delete render pipeline cache
                    string libraryPath = "Library/PackageCache/com.unity.render-pipelines.core@*";
                    foreach (var dir in System.IO.Directory.GetDirectories("Library/PackageCache/", "com.unity.render-pipelines.core@*"))
                    {
                        System.IO.Directory.Delete(dir, true);
                        Debug.Log($"[Bifrost] Cleared render pipeline cache: {dir}");
                    }
                    
                    AssetDatabase.Refresh();
                    Debug.Log("[Bifrost] Render pipeline cache cleared successfully.");
                }
                catch (Exception e)
                {
                    Debug.LogError($"[Bifrost] Failed to clear cache: {e.Message}");
                }
            }
        }
    }
}