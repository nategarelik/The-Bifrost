using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using UnityEditor;
using Unity.Plastic.Newtonsoft.Json.Linq;

namespace Bifrost.Editor.AI.Agent
{
    public class AgentContext
    {
        public string UserPrompt { get; set; }
        public string SystemPrompt { get; set; }
        public JObject ProjectState { get; set; }
        public JObject SceneState { get; set; }
        public List<string> RecentActions { get; set; }
        public Dictionary<string, object> Memory { get; set; }
        public int EstimatedTokens { get; set; }
    }

    public class AgentMemory
    {
        private readonly Dictionary<string, MemoryEntry> shortTermMemory = new Dictionary<string, MemoryEntry>();
        private readonly List<string> longTermMemory = new List<string>();
        private const int MaxShortTermEntries = 100;
        private const int MaxLongTermEntries = 1000;

        public void AddShortTerm(string key, object value, string category = "general")
        {
            shortTermMemory[key] = new MemoryEntry
            {
                Key = key,
                Value = value,
                Category = category,
                Timestamp = DateTime.Now,
                AccessCount = 0
            };

            // Prune old entries if needed
            if (shortTermMemory.Count > MaxShortTermEntries)
            {
                var oldestKey = shortTermMemory.OrderBy(kvp => kvp.Value.Timestamp).First().Key;
                shortTermMemory.Remove(oldestKey);
            }
        }

        public object GetShortTerm(string key)
        {
            if (shortTermMemory.TryGetValue(key, out var entry))
            {
                entry.AccessCount++;
                entry.LastAccessed = DateTime.Now;
                return entry.Value;
            }
            return null;
        }

        public void AddLongTerm(string summary)
        {
            longTermMemory.Add($"[{DateTime.Now:yyyy-MM-dd HH:mm:ss}] {summary}");
            
            if (longTermMemory.Count > MaxLongTermEntries)
            {
                longTermMemory.RemoveAt(0);
            }
        }

        public List<string> GetRecentLongTermMemories(int count = 10)
        {
            return longTermMemory.TakeLast(count).ToList();
        }

        public Dictionary<string, object> GetContextRelevantMemory(string context)
        {
            var relevant = new Dictionary<string, object>();
            
            // Get frequently accessed short-term memories
            var frequentMemories = shortTermMemory
                .Where(kvp => kvp.Value.AccessCount > 2)
                .OrderByDescending(kvp => kvp.Value.AccessCount)
                .Take(20);

            foreach (var kvp in frequentMemories)
            {
                relevant[kvp.Key] = kvp.Value.Value;
            }

            return relevant;
        }

        private class MemoryEntry
        {
            public string Key { get; set; }
            public object Value { get; set; }
            public string Category { get; set; }
            public DateTime Timestamp { get; set; }
            public DateTime LastAccessed { get; set; }
            public int AccessCount { get; set; }
        }
    }

    public class CostTracker
    {
        private int totalTokensUsed = 0;
        private float totalCost = 0f;
        private readonly Dictionary<string, ModelCost> modelCosts = new Dictionary<string, ModelCost>
        {
            ["gpt-4"] = new ModelCost { InputCostPer1k = 0.03f, OutputCostPer1k = 0.06f },
            ["gpt-3.5-turbo"] = new ModelCost { InputCostPer1k = 0.0005f, OutputCostPer1k = 0.0015f },
            ["claude-3-opus"] = new ModelCost { InputCostPer1k = 0.015f, OutputCostPer1k = 0.075f },
            ["claude-3-sonnet"] = new ModelCost { InputCostPer1k = 0.003f, OutputCostPer1k = 0.015f },
            ["llama3.2"] = new ModelCost { InputCostPer1k = 0f, OutputCostPer1k = 0f } // Local model
        };

        public void TrackUsage(string model, int inputTokens, int outputTokens)
        {
            totalTokensUsed += inputTokens + outputTokens;
            
            if (modelCosts.TryGetValue(model, out var cost))
            {
                totalCost += (inputTokens / 1000f) * cost.InputCostPer1k;
                totalCost += (outputTokens / 1000f) * cost.OutputCostPer1k;
            }
        }

        public float GetTotalCost() => totalCost;
        public int GetTotalTokens() => totalTokensUsed;

        private class ModelCost
        {
            public float InputCostPer1k { get; set; }
            public float OutputCostPer1k { get; set; }
        }
    }

    public class AgentContextManager
    {
        private readonly Context.UnityContextAnalyzer contextAnalyzer;
        private readonly AgentMemory memory;
        private readonly CostTracker costTracker;

        public AgentContextManager()
        {
            this.contextAnalyzer = new Context.UnityContextAnalyzer();
            this.memory = new AgentMemory();
            this.costTracker = new CostTracker();
        }

        public async Task<AgentContext> BuildContext(string userPrompt)
        {
            var context = new AgentContext
            {
                UserPrompt = userPrompt,
                Memory = new Dictionary<string, object>(),
                RecentActions = new List<string>()
            };

            // Build system prompt with project awareness
            context.SystemPrompt = BuildSystemPrompt();

            // Gather project state
            context.ProjectState = await GatherProjectState();

            // Gather scene state
            context.SceneState = GatherSceneState();

            // Add relevant memories
            context.Memory = memory.GetContextRelevantMemory(userPrompt);

            // Add recent actions
            context.RecentActions = memory.GetRecentLongTermMemories(5);

            // Estimate token usage
            context.EstimatedTokens = EstimateTokens(context);

            return context;
        }

        private string BuildSystemPrompt()
        {
            return @"You are an AI-powered Unity game development assistant with full control over the Unity Editor through MCP tools.

Your capabilities include:
- Creating and modifying scenes, GameObjects, and components
- Generating scripts and managing code
- Importing and creating assets
- Configuring physics, lighting, and audio
- Building complete game systems

Current Unity version: " + Application.unityVersion + @"
Project: " + Application.productName + @"

Be proactive in using tools to accomplish tasks. You can execute multiple operations to achieve complex goals.";
        }

        private async Task<JObject> GatherProjectState()
        {
            return await Task.Run(() =>
            {
                var state = new JObject();
                
                // Basic project info
                state["projectName"] = Application.productName;
                state["unityVersion"] = Application.unityVersion;
                state["targetPlatform"] = EditorUserBuildSettings.activeBuildTarget.ToString();
                
                // Asset summary
                state["assetCounts"] = new JObject
                {
                    ["scripts"] = AssetDatabase.FindAssets("t:Script").Length,
                    ["prefabs"] = AssetDatabase.FindAssets("t:Prefab").Length,
                    ["materials"] = AssetDatabase.FindAssets("t:Material").Length,
                    ["scenes"] = EditorBuildSettings.scenes.Length
                };
                
                // Recent files
                var recentScripts = AssetDatabase.FindAssets("t:Script")
                    .Take(10)
                    .Select(guid => AssetDatabase.GUIDToAssetPath(guid))
                    .ToList();
                state["recentScripts"] = new JArray(recentScripts);
                
                return state;
            });
        }

        private JObject GatherSceneState()
        {
            var scene = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            var state = new JObject
            {
                ["sceneName"] = scene.name,
                ["scenePath"] = scene.path,
                ["rootObjectCount"] = scene.rootCount,
                ["totalGameObjects"] = GameObject.FindObjectsOfType<GameObject>().Length
            };

            // Key object types in scene
            state["objectCounts"] = new JObject
            {
                ["cameras"] = GameObject.FindObjectsOfType<Camera>().Length,
                ["lights"] = GameObject.FindObjectsOfType<Light>().Length,
                ["audioSources"] = GameObject.FindObjectsOfType<AudioSource>().Length,
                ["uiCanvases"] = GameObject.FindObjectsOfType<Canvas>().Length
            };

            return state;
        }

        public async Task<string> SummarizeForMemory(string interaction)
        {
            // Simple summarization - in production, this could use an LLM
            var lines = interaction.Split('\n');
            if (lines.Length <= 3)
            {
                return interaction;
            }

            var summary = string.Join(" ", lines.Take(2)) + "...";
            return summary.Length > 200 ? summary.Substring(0, 197) + "..." : summary;
        }

        private int EstimateTokens(AgentContext context)
        {
            // Rough estimation: 1 token ≈ 4 characters
            var totalChars = context.SystemPrompt.Length +
                           context.UserPrompt.Length +
                           context.ProjectState.ToString().Length +
                           context.SceneState.ToString().Length +
                           string.Join("", context.RecentActions).Length;

            return totalChars / 4;
        }

        public void RecordAction(string action)
        {
            memory.AddLongTerm(action);
        }

        public void UpdateMemory(string key, object value, string category = "general")
        {
            memory.AddShortTerm(key, value, category);
        }

        public CostTracker GetCostTracker() => costTracker;
        public AgentMemory GetMemory() => memory;
    }
}