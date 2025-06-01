#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using System.IO;
using System.Net;
using System.IO.Compression;

[InitializeOnLoad]
public static class WebSocketSharpAutoInstaller
{
    private const string DllPath = "Assets/Plugins/websocket-sharp.dll";
    private const string NugetUrl = "https://www.nuget.org/api/v2/package/WebSocketSharp/1.0.3-rc11";
    private const string TempZip = "Temp/websocket-sharp.nupkg";

    static WebSocketSharpAutoInstaller()
    {
        if (!File.Exists(DllPath))
        {
            EditorApplication.update += TryInstall;
        }
    }

    [MenuItem("Tools/Bifrost/Install WebSocket Sharp")]
    public static void ForceInstall()
    {
        if (File.Exists(DllPath))
        {
            Debug.Log("[Bifrost] websocket-sharp.dll already exists at: " + DllPath);
            return;
        }
        TryInstall();
    }

    private static void TryInstall()
    {
        EditorApplication.update -= TryInstall;
        if (File.Exists(DllPath)) return;

        Debug.Log("[Bifrost] Downloading websocket-sharp from NuGet...");
        try
        {
            Directory.CreateDirectory("Assets/Plugins");
            Directory.CreateDirectory("Temp");
            
            Debug.Log("[Bifrost] Downloading from: " + NugetUrl);
            using (var client = new WebClient())
            {
                client.DownloadFile(NugetUrl, TempZip);
            }
            Debug.Log("[Bifrost] Download complete. Extracting...");

            using (var archive = ZipFile.OpenRead(TempZip))
            {
                foreach (var entry in archive.Entries)
                {
                    Debug.Log("[Bifrost] Found entry: " + entry.FullName);
                    if (entry.FullName.EndsWith("lib/net20/websocket-sharp.dll") || 
                        entry.FullName.EndsWith("lib/net35/websocket-sharp.dll") ||
                        entry.FullName.Contains("websocket-sharp.dll"))
                    {
                        entry.ExtractToFile(DllPath, true);
                        Debug.Log("[Bifrost] ✅ websocket-sharp.dll installed to Assets/Plugins/");
                        AssetDatabase.Refresh();
                        break;
                    }
                }
            }
            File.Delete(TempZip);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[Bifrost] ❌ Failed to auto-install websocket-sharp: " + ex.Message);
            Debug.LogError("[Bifrost] Please manually download websocket-sharp.dll from: https://github.com/sta/websocket-sharp/releases");
        }
    }
}
#endif
