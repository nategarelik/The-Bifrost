using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Bifrost.Editor
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
        public async Task<GameSystemPlan> PlanGameSystemAsync(string description)
        {
            // Ask the AI agent to plan the system (stubbed for now)
            var plan = await bifrostAgent.PlanGameSystemAsync(description);
            return plan;
        }

        /// <summary>
        /// Applies the approved game system plan: creates scripts, prefabs, UIs, etc.
        /// </summary>
        public async Task<bool> ApplyGameSystemAsync(GameSystemPlan plan)
        {
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
                bool created = projectManager.CreatePrefab(prefab.Path, prefab.Template);
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
            AssetDatabase.Refresh();
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
} 