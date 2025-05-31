using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Bifrost.Editor.UI
{
    /// <summary>
    /// ScriptableObject for storing Bifrost AI mode definitions
    /// </summary>
    public class BifrostModeLibrary : ScriptableObject
    {
        private const string LIBRARY_PATH = "Assets/Bifrost/Resources/BifrostModeLibrary.asset";

        [SerializeField]
        private List<BifrostMode> modes = new List<BifrostMode>
        {
            new BifrostMode { name = "Code", description = "Code generation and editing mode.", promptTemplate = "Prompts/CodePrompt" },
            new BifrostMode { name = "Architect", description = "System and architecture design mode.", promptTemplate = "Prompts/ArchitectPrompt" },
            new BifrostMode { name = "Designer", description = "Game and level design mode.", promptTemplate = "Prompts/DesignerPrompt" },
            new BifrostMode { name = "GameDev", description = "General Unity game development mode.", promptTemplate = "Prompts/GameDevPrompt" }
        };

        public List<BifrostMode> Modes => modes;

        public static BifrostModeLibrary GetOrCreateLibrary()
        {
            Bifrost.Editor.UI.BifrostSettingsUI.EnsureBifrostResourcesFolder();
            var library = AssetDatabase.LoadAssetAtPath<BifrostModeLibrary>(LIBRARY_PATH);
            if (library == null)
            {
                library = ScriptableObject.CreateInstance<BifrostModeLibrary>();
                AssetDatabase.CreateAsset(library, LIBRARY_PATH);
                AssetDatabase.SaveAssets();
            }
            return library;
        }
    }
}