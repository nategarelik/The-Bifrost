using UnityEngine;
using UnityEditor;
using System.Linq;

namespace Bifrost.Editor.Utilities
{
    /// <summary>
    /// Handles asset importing for Bifrost package
    /// </summary>
    public class BifrostAssetPostprocessor : AssetPostprocessor
    {
        private static bool _isProcessing = false;

        static void OnPostprocessAllAssets(
            string[] importedAssets,
            string[] deletedAssets,
            string[] movedAssets,
            string[] movedFromAssetPaths)
        {
            // Prevent recursive processing
            if (_isProcessing) return;
            
            try
            {
                _isProcessing = true;

                // Check if any Bifrost assets were imported
                bool bifrostAssetsImported = importedAssets.Any(path => 
                    path.Contains("Bifrost") || 
                    path.Contains("websocket-sharp.dll"));

                if (bifrostAssetsImported)
                {
                    // Ensure websocket-sharp.dll has correct import settings
                    foreach (var asset in importedAssets)
                    {
                        if (asset.EndsWith("websocket-sharp.dll"))
                        {
                            var importer = AssetImporter.GetAtPath(asset) as PluginImporter;
                            if (importer != null)
                            {
                                importer.SetCompatibleWithEditor(true);
                                importer.SetCompatibleWithAnyPlatform(false);
                                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows, false);
                                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneWindows64, false);
                                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneOSX, false);
                                importer.SetCompatibleWithPlatform(BuildTarget.StandaloneLinux64, false);
                                importer.SaveAndReimport();
                                Debug.Log("[Bifrost] Configured websocket-sharp.dll for Editor only");
                            }
                        }
                    }

                    // Log successful import
                    Debug.Log($"[Bifrost] Imported {importedAssets.Length} assets successfully");
                }
            }
            catch (System.Exception e)
            {
                Debug.LogError($"[Bifrost] Error in asset postprocessor: {e.Message}");
            }
            finally
            {
                _isProcessing = false;
            }
        }
    }
}