using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;

namespace Bifrost.Editor
{
    /// <summary>
    /// Loads and manages AI prompt templates from Resources/Prompts.
    /// </summary>
    public class PromptTemplateManager
    {
        private const string PROMPT_FOLDER = "Prompts";
        private Dictionary<string, string> templates = new Dictionary<string, string>();
        private bool isLoaded = false;

        public async Task LoadTemplatesAsync()
        {
            if (isLoaded) return;
            templates.Clear();
            // Load all text assets in Resources/Prompts
            var promptAssets = Resources.LoadAll<TextAsset>(PROMPT_FOLDER);
            foreach (var asset in promptAssets)
            {
                templates[asset.name] = asset.text;
            }
            isLoaded = true;
            await Task.CompletedTask;
        }

        public string GetTemplate(string templateName)
        {
            if (!isLoaded)
            {
                Debug.LogWarning($"PromptTemplateManager: Templates not loaded. Call LoadTemplatesAsync() first.");
                return null;
            }
            if (templates.TryGetValue(templateName, out var template))
                return template;
            Debug.LogWarning($"PromptTemplateManager: Template '{templateName}' not found.");
            return null;
        }

        public List<string> GetAllTemplateNames()
        {
            if (!isLoaded)
                return new List<string>();
            return new List<string>(templates.Keys);
        }

        public void ReloadTemplates()
        {
            isLoaded = false;
            templates.Clear();
        }
    }
} 