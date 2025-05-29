using System;
using UnityEngine;

namespace Bifrost.Editor.AI
{
    [Serializable]
    public class LLMGameSystemPlan
    {
        public string systemName;
        public string description;
        public string[] requiredAssets;
        public string[] steps;
        public string notes;

        public static bool TryParse(string json, out LLMGameSystemPlan plan)
        {
            plan = null;
            try
            {
                plan = JsonUtility.FromJson<LLMGameSystemPlan>(json);
                // Basic validation: must have a systemName and at least one step
                if (plan == null || string.IsNullOrEmpty(plan.systemName) || plan.steps == null || plan.steps.Length == 0)
                    return false;
                return true;
            }
            catch
            {
                plan = null;
                return false;
            }
        }

        public static LLMGameSystemPlan Fallback(string userPrompt)
        {
            // Fallback: minimal plan with user prompt as description
            return new LLMGameSystemPlan
            {
                systemName = "Unknown System",
                description = userPrompt,
                requiredAssets = new string[0],
                steps = new[] { "Unable to parse AI response. Please try again or refine your prompt." },
                notes = "Fallback plan generated due to malformed AI response."
            };
        }
    }
}