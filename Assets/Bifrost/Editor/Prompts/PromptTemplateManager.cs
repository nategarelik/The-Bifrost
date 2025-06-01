using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Bifrost.Editor.AI.Prompts;

namespace Bifrost.Editor
{
    /// <summary>
    /// Loads and manages AI prompt templates from Resources/Prompts.
    /// </summary>
    public class PromptTemplateManager
    {
        private const string PROMPT_FOLDER = "Prompts";
        [System.Serializable]
        public class PromptTemplate
        {
            public string Name;
            public string Content;
            public PromptTemplateCategory Category;
        }
        private Dictionary<string, PromptTemplate> templates = new Dictionary<string, PromptTemplate>();
        private bool isLoaded = false;

        public async Task LoadTemplatesAsync()
        {
            if (isLoaded) return;
            templates.Clear();
            // Load all text assets in Resources/Prompts
            var promptAssets = Resources.LoadAll<TextAsset>(PROMPT_FOLDER);
            foreach (var asset in promptAssets)
            {
                // Parse category from filename: e.g., "2D_BasicMovement.txt" => TwoD
                var split = asset.name.Split('_');
                PromptTemplateCategory cat = PromptTemplateCategory.General;
                if (split.Length > 1 && System.Enum.TryParse<PromptTemplateCategory>(split[0], true, out var parsedCat))
                    cat = parsedCat;
                templates[asset.name] = new PromptTemplate
                {
                    Name = asset.name,
                    Content = asset.text,
                    Category = cat
                };
            }
            isLoaded = true;
            await Task.CompletedTask;
        }

        public PromptTemplate GetTemplate(string templateName)
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

        public List<string> GetAllTemplateNames(PromptTemplateCategory? category = null)
        {
            if (!isLoaded)
                return new List<string>();
            if (category.HasValue)
                return new List<string>(templates.Values.Where(t => t.Category == category.Value).Select(t => t.Name));
            return new List<string>(templates.Keys);
        }

        public void ReloadTemplates()
        {
            isLoaded = false;
            templates.Clear();
        }

        public void AddOrUpdateTemplate(PromptTemplate template)
        {
            templates[template.Name] = template;
            // TODO: Save to disk if needed
        }

        public void RemoveTemplate(string templateName)
        {
            if (templates.ContainsKey(templateName))
                templates.Remove(templateName);
            // TODO: Remove from disk if needed
        }
    }
}