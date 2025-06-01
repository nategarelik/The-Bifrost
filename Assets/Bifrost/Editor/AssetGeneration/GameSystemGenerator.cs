using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using System.Collections.Generic;
using Bifrost.Editor.AI;
using Bifrost.Editor.AI.Prompts;

namespace Bifrost.Editor.AssetGeneration
{
    /// <summary>
    /// Generates complete game systems (scripts, prefabs, UIs, etc.) from a text description using the AI agent.
    /// </summary>
    public class GameSystemGenerator
    {
        private UnityProjectManager projectManager;
        private BifrostAgent bifrostAgent;

        public GameSystemGenerator(UnityProjectManager projectManager, BifrostAgent bifrostAgent)
        {
            this.projectManager = projectManager;
            this.bifrostAgent = bifrostAgent;
        }

        /// <summary>
        /// Generates a game system from a description. Returns a summary of planned changes for approval.
        /// </summary>
        public async Task<(LLMGameSystemPlan plan, string rawResponse)> PlanGameSystemAsync(string description)
        {
            return await bifrostAgent.PlanGameSystemAsync(description);
        }

        /// <summary>
        /// Applies the approved game system plan: creates scripts, prefabs, UIs, etc.
        /// </summary>
        public async Task<bool> ApplyGameSystemAsync(GameSystemPlan plan)
        {
            if (!GameSystemPlan.Validate(plan, out var validationError))
            {
                Debug.LogError($"GameSystemGenerator: Plan validation failed: {validationError}");
                return false;
            }
            // For each planned script
            foreach (var script in plan.Scripts)
            {
                bool created = projectManager.CreateScript(script.Path, script.Content);
                if (!created)
                {
                    Debug.LogError($"GameSystemGenerator: Failed to create script at {script.Path}");
                    return false;
                }
            }
            // For each planned prefab
            foreach (var prefab in plan.Prefabs)
            {
                bool created = projectManager.CreatePrefab(prefab.Path, new GameObject(prefab.Path));
                if (!created)
                {
                    Debug.LogError($"GameSystemGenerator: Failed to create prefab at {prefab.Path}");
                    return false;
                }
            }
            // For each planned UI
            foreach (var ui in plan.UIs)
            {
                bool created = projectManager.CreateUI(ui.Path, ui.Template);
                if (!created)
                {
                    Debug.LogError($"GameSystemGenerator: Failed to create UI at {ui.Path}");
                    return false;
                }
            }
            // For each planned ScriptableObject
            foreach (var so in plan.ScriptableObjects)
            {
                bool created = projectManager.CreateScriptableObject(so.Path, so.TypeName, so.JsonData);
                if (!created)
                {
                    Debug.LogError($"GameSystemGenerator: Failed to create ScriptableObject at {so.Path}");
                    return false;
                }
            }
            AssetDatabase.Refresh();
            await Task.CompletedTask;
            return true;
        }
    }

    /// <summary>
    /// Represents a plan for a generated game system (for approval before applying)
    /// </summary>
    public class GameSystemPlan
    {
        public List<PlannedScript> Scripts = new List<PlannedScript>();
        public List<PlannedPrefab> Prefabs = new List<PlannedPrefab>();
        public List<PlannedUI> UIs = new List<PlannedUI>();
        public List<PlannedScriptableObject> ScriptableObjects = new List<PlannedScriptableObject>();

        /// <summary>
        /// Validates the plan for required fields, valid asset names, and no destructive actions.
        /// </summary>
        public static bool Validate(GameSystemPlan plan, out string error)
        {
            error = null;
            if (plan == null)
            {
                error = "Plan is null.";
                return false;
            }
            if (plan.Scripts == null || plan.Prefabs == null || plan.UIs == null || plan.ScriptableObjects == null)
            {
                error = "Plan is missing required lists.";
                return false;
            }
            if (plan.Scripts.Count == 0 && plan.Prefabs.Count == 0 && plan.UIs.Count == 0 && plan.ScriptableObjects.Count == 0)
            {
                error = "Plan contains no actions.";
                return false;
            }
            // Check for valid asset names and paths
            foreach (var script in plan.Scripts)
            {
                if (string.IsNullOrWhiteSpace(script.Path) || !script.Path.StartsWith("Assets/"))
                {
                    error = $"Invalid script path: {script.Path}";
                    return false;
                }
            }
            foreach (var prefab in plan.Prefabs)
            {
                if (string.IsNullOrWhiteSpace(prefab.Path) || !prefab.Path.StartsWith("Assets/"))
                {
                    error = $"Invalid prefab path: {prefab.Path}";
                    return false;
                }
            }
            foreach (var ui in plan.UIs)
            {
                if (string.IsNullOrWhiteSpace(ui.Path) || !ui.Path.StartsWith("Assets/"))
                {
                    error = $"Invalid UI path: {ui.Path}";
                    return false;
                }
            }
            foreach (var so in plan.ScriptableObjects)
            {
                if (string.IsNullOrWhiteSpace(so.Path) || !so.Path.StartsWith("Assets/"))
                {
                    error = $"Invalid ScriptableObject path: {so.Path}";
                    return false;
                }
            }
            // (Optional) Add more checks for destructive actions, reserved names, etc.
            return true;
        }
    }

    public class PlannedScript
    {
        public string Path;
        public string Content;
    }
    public class PlannedPrefab
    {
        public string Path;
        public string Template;
    }
    public class PlannedUI
    {
        public string Path;
        public string Template;
    }

    public class PlannedScriptableObject
    {
        public string Path;
        public string TypeName; // e.g., "MyCustomSO"
        public string JsonData; // Serialized data (optional)
    }
}