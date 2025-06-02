using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Bifrost.Editor.Utilities
{
    public class BifrostDiagnostics : EditorWindow
    {
        private Vector2 scrollPosition;
        private bool websocketFound = false;
        private bool newtonsoftFound = false;
        private bool assemblyValid = false;
        private string diagnosticResults = "";

        [MenuItem("Bifrost/Utilities/Run Diagnostics")]
        public static void ShowWindow()
        {
            var window = GetWindow<BifrostDiagnostics>("Bifrost Diagnostics");
            window.RunDiagnostics();
        }

        private void OnGUI()
        {
            EditorGUILayout.LabelField("Bifrost Diagnostics", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            // Status indicators
            DrawStatusLine("WebSocketSharp DLL", websocketFound);
            DrawStatusLine("Newtonsoft.Json", newtonsoftFound);
            DrawStatusLine("Assembly Definition", assemblyValid);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Diagnostic Results:", EditorStyles.boldLabel);
            
            scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, GUILayout.Height(300));
            EditorGUILayout.TextArea(diagnosticResults, EditorStyles.textArea, GUILayout.ExpandHeight(true));
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();

            if (GUILayout.Button("Refresh Diagnostics", GUILayout.Height(30)))
            {
                RunDiagnostics();
            }

            if (GUILayout.Button("Auto-Fix Issues", GUILayout.Height(30)))
            {
                AutoFixIssues();
            }

            if (GUILayout.Button("Clear All Caches", GUILayout.Height(30)))
            {
                ClearAllCaches();
            }
        }

        private void DrawStatusLine(string label, bool status)
        {
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(label, GUILayout.Width(150));
            
            var oldColor = GUI.color;
            GUI.color = status ? Color.green : Color.red;
            EditorGUILayout.LabelField(status ? "✓ OK" : "✗ Missing", GUILayout.Width(100));
            GUI.color = oldColor;
            
            EditorGUILayout.EndHorizontal();
        }

        private void RunDiagnostics()
        {
            diagnosticResults = "";
            
            // Check WebSocketSharp
            diagnosticResults += "=== WebSocketSharp Check ===\n";
            string[] possiblePaths = {
                "Assets/Plugins/websocket-sharp.dll",
                "Assets/Bifrost/Editor/Plugins/websocket-sharp.dll"
            };
            
            websocketFound = false;
            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                {
                    websocketFound = true;
                    diagnosticResults += $"✓ Found at: {path}\n";
                    
                    // Check import settings
                    var importer = AssetImporter.GetAtPath(path) as PluginImporter;
                    if (importer != null)
                    {
                        diagnosticResults += $"  Editor Compatible: {importer.GetCompatibleWithEditor()}\n";
                    }
                }
            }
            
            if (!websocketFound)
            {
                diagnosticResults += "✗ WebSocketSharp.dll not found!\n";
            }

            // Check Newtonsoft.Json
            diagnosticResults += "\n=== Newtonsoft.Json Check ===\n";
            try
            {
                var assembly = Assembly.Load("Unity.Plastic.Newtonsoft.Json");
                if (assembly != null)
                {
                    newtonsoftFound = true;
                    diagnosticResults += "✓ Unity.Plastic.Newtonsoft.Json loaded\n";
                    diagnosticResults += $"  Version: {assembly.GetName().Version}\n";
                }
            }
            catch
            {
                newtonsoftFound = false;
                diagnosticResults += "✗ Unity.Plastic.Newtonsoft.Json not found!\n";
            }

            // Check Assembly Definition
            diagnosticResults += "\n=== Assembly Definition Check ===\n";
            string asmdefPath = "Assets/Bifrost/Editor/Bifrost.Editor.asmdef";
            if (File.Exists(asmdefPath))
            {
                assemblyValid = true;
                diagnosticResults += "✓ Assembly definition found\n";
                
                string content = File.ReadAllText(asmdefPath);
                if (content.Contains("Unity.Plastic.Newtonsoft.Json"))
                {
                    diagnosticResults += "  ✓ References Newtonsoft.Json\n";
                }
                else
                {
                    diagnosticResults += "  ✗ Missing Newtonsoft.Json reference\n";
                    assemblyValid = false;
                }
                
                if (content.Contains("websocket-sharp.dll"))
                {
                    diagnosticResults += "  ✓ References websocket-sharp.dll\n";
                }
                else
                {
                    diagnosticResults += "  ✗ Missing websocket-sharp.dll reference\n";
                    assemblyValid = false;
                }
            }
            else
            {
                assemblyValid = false;
                diagnosticResults += "✗ Assembly definition not found!\n";
            }

            // Package version
            diagnosticResults += "\n=== Package Info ===\n";
            string packagePath = "Assets/Bifrost/package.json";
            if (File.Exists(packagePath))
            {
                var packageContent = File.ReadAllText(packagePath);
                var lines = packageContent.Split('\n');
                foreach (var line in lines)
                {
                    if (line.Contains("\"version\""))
                    {
                        diagnosticResults += $"Package {line.Trim()}\n";
                        break;
                    }
                }
            }

            diagnosticResults += "\n=== Unity Info ===\n";
            diagnosticResults += $"Unity Version: {Application.unityVersion}\n";
            diagnosticResults += $"Platform: {Application.platform}\n";
            
            Repaint();
        }

        private void AutoFixIssues()
        {
            bool fixedSomething = false;

            // Fix WebSocketSharp
            if (!websocketFound)
            {
                string sourcePath = "Assets/Plugins/websocket-sharp.dll";
                string destPath = "Assets/Bifrost/Editor/Plugins/websocket-sharp.dll";
                
                if (File.Exists(sourcePath) && !File.Exists(destPath))
                {
                    Directory.CreateDirectory(Path.GetDirectoryName(destPath));
                    File.Copy(sourcePath, destPath);
                    fixedSomething = true;
                    Debug.Log("[Bifrost] Copied websocket-sharp.dll to package");
                }
            }

            if (fixedSomething)
            {
                AssetDatabase.Refresh();
                RunDiagnostics();
                EditorUtility.DisplayDialog("Auto-Fix Complete", 
                    "Some issues were fixed. Please check the diagnostics again.", 
                    "OK");
            }
            else
            {
                EditorUtility.DisplayDialog("No Issues Found", 
                    "No automatic fixes were needed.", 
                    "OK");
            }
        }

        private void ClearAllCaches()
        {
            if (EditorUtility.DisplayDialog("Clear All Caches",
                "This will clear:\n" +
                "• Package Cache\n" +
                "• Script Assemblies\n" +
                "• Asset Database\n\n" +
                "Unity will restart after clearing.",
                "Clear", "Cancel"))
            {
                // Clear caches
                try
                {
                    if (Directory.Exists("Library/PackageCache"))
                    {
                        Directory.Delete("Library/PackageCache", true);
                    }
                    
                    if (Directory.Exists("Library/ScriptAssemblies"))
                    {
                        Directory.Delete("Library/ScriptAssemblies", true);
                    }

                    EditorUtility.DisplayDialog("Cache Cleared",
                        "Caches cleared. Unity will now refresh.",
                        "OK");
                    
                    AssetDatabase.Refresh();
                }
                catch (System.Exception e)
                {
                    Debug.LogError($"[Bifrost] Error clearing cache: {e.Message}");
                }
            }
        }
    }
}