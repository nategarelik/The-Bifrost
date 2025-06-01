using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;
using Newtonsoft.Json.Linq;

namespace Bifrost.Editor.AI.Agent
{
    public class AgentPlan
    {
        public string Goal { get; set; }
        public List<PlanStep> Steps { get; set; } = new List<PlanStep>();
        public float EstimatedCost { get; set; }
        public int EstimatedTokens { get; set; }
        public TimeSpan EstimatedDuration { get; set; }
        public float ConfidenceScore { get; set; }
        public List<string> RequiredTools { get; set; } = new List<string>();
        public List<string> Risks { get; set; } = new List<string>();
    }

    public class PlanStep
    {
        public int StepNumber { get; set; }
        public string Description { get; set; }
        public string ToolName { get; set; }
        public JObject ToolArguments { get; set; }
        public List<string> Dependencies { get; set; } = new List<string>();
        public PlanStepStatus Status { get; set; } = PlanStepStatus.Pending;
        public string Result { get; set; }
        public Exception Error { get; set; }
    }

    public enum PlanStepStatus
    {
        Pending,
        InProgress,
        Completed,
        Failed,
        Skipped
    }

    public class ExecutionError
    {
        public PlanStep FailedStep { get; set; }
        public Exception Exception { get; set; }
        public string Context { get; set; }
        public DateTime Timestamp { get; set; }
    }

    public class AgentPlanner
    {
        private readonly MCP.MCPToolRegistry toolRegistry;
        private readonly IBifrostLLMProvider llmProvider;
        private readonly Dictionary<string, PlanTemplate> planTemplates;

        public AgentPlanner(MCP.MCPToolRegistry toolRegistry, IBifrostLLMProvider llmProvider)
        {
            this.toolRegistry = toolRegistry;
            this.llmProvider = llmProvider;
            this.planTemplates = InitializePlanTemplates();
        }

        public async Task<AgentPlan> CreatePlan(string goal, AgentContext context)
        {
            try
            {
                // Check if we have a template for this type of goal
                var template = FindMatchingTemplate(goal);
                if (template != null)
                {
                    return await CreatePlanFromTemplate(goal, template, context);
                }

                // Otherwise, use LLM to generate a plan
                return await GeneratePlanWithLLM(goal, context);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to create plan: {ex.Message}");
                throw;
            }
        }

        private Dictionary<string, PlanTemplate> InitializePlanTemplates()
        {
            return new Dictionary<string, PlanTemplate>
            {
                ["create_basic_scene"] = new PlanTemplate
                {
                    Name = "Create Basic Scene",
                    Pattern = "create.*scene|new.*scene|setup.*scene",
                    Steps = new List<PlanTemplateStep>
                    {
                        new PlanTemplateStep { ToolName = "create_scene", Description = "Create new scene" },
                        new PlanTemplateStep { ToolName = "create_gameobject_advanced", Description = "Add camera" },
                        new PlanTemplateStep { ToolName = "create_gameobject_advanced", Description = "Add lighting" },
                        new PlanTemplateStep { ToolName = "save_scene", Description = "Save the scene" }
                    }
                },
                ["create_player_controller"] = new PlanTemplate
                {
                    Name = "Create Player Controller",
                    Pattern = "player.*controller|character.*controller|movement.*script",
                    Steps = new List<PlanTemplateStep>
                    {
                        new PlanTemplateStep { ToolName = "create_gameobject_advanced", Description = "Create player GameObject" },
                        new PlanTemplateStep { ToolName = "create_script", Description = "Create movement script" },
                        new PlanTemplateStep { ToolName = "attach_script", Description = "Attach script to player" },
                        new PlanTemplateStep { ToolName = "add_component", Description = "Add required components" }
                    }
                }
            };
        }

        private PlanTemplate FindMatchingTemplate(string goal)
        {
            var lowerGoal = goal.ToLower();
            return planTemplates.Values.FirstOrDefault(t => 
                System.Text.RegularExpressions.Regex.IsMatch(lowerGoal, t.Pattern));
        }

        private async Task<AgentPlan> CreatePlanFromTemplate(string goal, PlanTemplate template, AgentContext context)
        {
            var plan = new AgentPlan
            {
                Goal = goal,
                ConfidenceScore = 0.9f,
                EstimatedDuration = TimeSpan.FromMinutes(5)
            };

            int stepNumber = 1;
            foreach (var templateStep in template.Steps)
            {
                var tool = toolRegistry.GetTool(templateStep.ToolName);
                if (tool != null)
                {
                    plan.Steps.Add(new PlanStep
                    {
                        StepNumber = stepNumber++,
                        Description = templateStep.Description,
                        ToolName = templateStep.ToolName,
                        ToolArguments = templateStep.DefaultArguments ?? new JObject()
                    });
                    
                    if (!plan.RequiredTools.Contains(templateStep.ToolName))
                    {
                        plan.RequiredTools.Add(templateStep.ToolName);
                    }
                }
            }

            plan.EstimatedTokens = EstimateTokenUsage(plan);
            plan.EstimatedCost = EstimateCost(plan);

            return plan;
        }

        private async Task<AgentPlan> GeneratePlanWithLLM(string goal, AgentContext context)
        {
            var availableTools = toolRegistry.GetAllTools();
            var toolsDescription = string.Join("\n", availableTools.Select(t => 
                $"- {t.Name}: {t.Description}"));

            var planPrompt = $@"Create a step-by-step plan to achieve this goal: {goal}

Available tools:
{toolsDescription}

Current context:
- Scene: {context.SceneState["sceneName"]}
- Project: {context.ProjectState["projectName"]}

Respond with a JSON plan in this format:
{{
  ""steps"": [
    {{
      ""stepNumber"": 1,
      ""description"": ""Step description"",
      ""toolName"": ""tool_name"",
      ""toolArguments"": {{ /* tool specific arguments */ }},
      ""dependencies"": []
    }}
  ],
  ""estimatedDuration"": ""5 minutes"",
  ""risks"": [""Potential risk 1"", ""Potential risk 2""]
}}";

            try
            {
                var response = await llmProvider.CompleteAsync(
                    planPrompt,
                    "gpt-4", // Use a capable model for planning
                    "",
                    "",
                    new LLMRequestOptions { maxTokens = 2048, temperature = 0.7f }
                );

                var planJson = JObject.Parse(response);
                return ParsePlanFromJson(goal, planJson);
            }
            catch (Exception ex)
            {
                Debug.LogError($"Failed to generate plan with LLM: {ex.Message}");
                // Fallback to a simple single-step plan
                return CreateFallbackPlan(goal);
            }
        }

        private AgentPlan ParsePlanFromJson(string goal, JObject planJson)
        {
            var plan = new AgentPlan { Goal = goal };

            if (planJson["steps"] is JArray steps)
            {
                foreach (JObject stepJson in steps)
                {
                    var step = new PlanStep
                    {
                        StepNumber = stepJson["stepNumber"]?.ToObject<int>() ?? plan.Steps.Count + 1,
                        Description = stepJson["description"]?.ToString() ?? "Execute step",
                        ToolName = stepJson["toolName"]?.ToString() ?? "",
                        ToolArguments = stepJson["toolArguments"] as JObject ?? new JObject()
                    };

                    if (stepJson["dependencies"] is JArray deps)
                    {
                        step.Dependencies = deps.Select(d => d.ToString()).ToList();
                    }

                    plan.Steps.Add(step);
                    
                    if (!string.IsNullOrEmpty(step.ToolName) && !plan.RequiredTools.Contains(step.ToolName))
                    {
                        plan.RequiredTools.Add(step.ToolName);
                    }
                }
            }

            if (planJson["risks"] is JArray risks)
            {
                plan.Risks = risks.Select(r => r.ToString()).ToList();
            }

            plan.EstimatedTokens = EstimateTokenUsage(plan);
            plan.EstimatedCost = EstimateCost(plan);
            plan.ConfidenceScore = 0.7f; // LLM-generated plans have moderate confidence

            return plan;
        }

        private AgentPlan CreateFallbackPlan(string goal)
        {
            return new AgentPlan
            {
                Goal = goal,
                Steps = new List<PlanStep>
                {
                    new PlanStep
                    {
                        StepNumber = 1,
                        Description = "Attempt to complete the goal directly",
                        ToolName = "send_console_log",
                        ToolArguments = new JObject
                        {
                            ["message"] = $"Attempting to complete goal: {goal}",
                            ["logType"] = "Log"
                        }
                    }
                },
                ConfidenceScore = 0.3f,
                EstimatedDuration = TimeSpan.FromMinutes(1),
                Risks = new List<string> { "This is a fallback plan with limited capabilities" }
            };
        }

        public async Task<AgentPlan> AdaptPlan(AgentPlan original, ExecutionError error)
        {
            // Analyze the error and create an adapted plan
            var adaptedPlan = new AgentPlan
            {
                Goal = original.Goal + " (adapted after error)",
                ConfidenceScore = original.ConfidenceScore * 0.8f
            };

            // Skip the failed step and any dependencies
            var failedStepNumber = error.FailedStep.StepNumber;
            var stepsToSkip = new HashSet<int> { failedStepNumber };
            
            // Find all steps that depend on the failed step
            foreach (var step in original.Steps)
            {
                if (step.Dependencies.Contains(failedStepNumber.ToString()))
                {
                    stepsToSkip.Add(step.StepNumber);
                }
            }

            // Copy non-failed steps
            foreach (var step in original.Steps)
            {
                if (!stepsToSkip.Contains(step.StepNumber))
                {
                    adaptedPlan.Steps.Add(new PlanStep
                    {
                        StepNumber = adaptedPlan.Steps.Count + 1,
                        Description = step.Description,
                        ToolName = step.ToolName,
                        ToolArguments = step.ToolArguments,
                        Dependencies = step.Dependencies
                    });
                }
            }

            // Add recovery steps if needed
            if (error.Exception is UnauthorizedAccessException)
            {
                adaptedPlan.Steps.Insert(0, new PlanStep
                {
                    StepNumber = 1,
                    Description = "Request necessary permissions",
                    ToolName = "send_console_log",
                    ToolArguments = new JObject
                    {
                        ["message"] = "Permission required for operation",
                        ["logType"] = "Warning"
                    }
                });
            }

            adaptedPlan.EstimatedTokens = EstimateTokenUsage(adaptedPlan);
            adaptedPlan.EstimatedCost = EstimateCost(adaptedPlan);

            return adaptedPlan;
        }

        private int EstimateTokenUsage(AgentPlan plan)
        {
            // Rough estimation based on plan complexity
            return 100 + (plan.Steps.Count * 50);
        }

        private float EstimateCost(AgentPlan plan)
        {
            // Rough cost estimation (in USD)
            var tokensPerStep = 50;
            var totalTokens = plan.Steps.Count * tokensPerStep;
            var costPer1kTokens = 0.002f; // Average cost
            return (totalTokens / 1000f) * costPer1kTokens;
        }

        private class PlanTemplate
        {
            public string Name { get; set; }
            public string Pattern { get; set; }
            public List<PlanTemplateStep> Steps { get; set; }
        }

        private class PlanTemplateStep
        {
            public string ToolName { get; set; }
            public string Description { get; set; }
            public JObject DefaultArguments { get; set; }
        }
    }
}