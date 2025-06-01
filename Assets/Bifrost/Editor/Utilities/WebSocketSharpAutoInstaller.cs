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

    private static void TryInstall()
    {
        EditorApplication.update -= TryInstall;
        if (File.Exists(DllPath)) return;

        Debug.Log("[Bifrost] Downloading websocket-sharp from NuGet...");
        try
        {
            Directory.CreateDirectory("Assets/Plugins");
            using (var client = new WebClient())
            {
                client.DownloadFile(NugetUrl, TempZip);
            }

            using (var archive = ZipFile.OpenRead(TempZip))
            {
                foreach (var entry in archive.Entries)
                {
                    if (entry.FullName.EndsWith("lib/net20/websocket-sharp.dll"))
                    {
                        entry.ExtractToFile(DllPath, true);
                        Debug.Log("[Bifrost] websocket-sharp.dll installed to Assets/Plugins/");
                        AssetDatabase.Refresh();
                        break;
                    }
                }
            }
            File.Delete(TempZip);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("[Bifrost] Failed to auto-install websocket-sharp: " + ex.Message);
        }
    }
}
#endif
