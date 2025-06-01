using System;
using UnityEngine;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
                string contentToProcess = json;

                // First, try to see if this is an OpenRouter response
                if (json.Contains("\"choices\"") && (json.Contains("\"message\"") || json.Contains("\"content\"")))
                {
                    Debug.Log("Detected OpenRouter response format, extracting content");
                    try
                    {
                        // Parse as JObject to navigate the JSON structure
                        JObject responseObj = JObject.Parse(json);

                        // Navigate to the content within choices
                        if (responseObj["choices"] is JArray choices && choices.Count > 0)
                        {
                            var firstChoice = choices[0];

                            // Extract the content from the message
                            if (firstChoice["message"] is JObject message && message["content"] != null)
                            {
                                contentToProcess = message["content"].ToString();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        Debug.LogError($"Error parsing OpenRouter response: {ex.Message}");
                        return false;
                    }
                }

                // Handle markdown code blocks (```json...``` or ```...```)
                if (contentToProcess.Contains("```"))
                {
                    // Find the start of the code block
                    int codeBlockStart = contentToProcess.IndexOf("```");
                    if (codeBlockStart >= 0)
                    {
                        // Skip past the opening ```json or ``` part
                        int jsonStart = contentToProcess.IndexOf('\n', codeBlockStart);
                        if (jsonStart < 0) jsonStart = codeBlockStart + 3; // If no newline, assume ```{ format
                        else jsonStart++; // Move past the newline

                        // Find the closing ```
                        int codeBlockEnd = contentToProcess.IndexOf("```", jsonStart);
                        if (codeBlockEnd > jsonStart)
                        {
                            contentToProcess = contentToProcess.Substring(jsonStart, codeBlockEnd - jsonStart).Trim();
                        }
                    }
                }

                // If content still contains JSON, extract it
                if (contentToProcess.Contains("{") && contentToProcess.Contains("}"))
                {
                    int start = contentToProcess.IndexOf('{');
                    int end = contentToProcess.LastIndexOf('}') + 1;
                    if (start >= 0 && end > start)
                    {
                        string extractedJson = contentToProcess.Substring(start, end - start);
                        // Now parse the extracted JSON
                        plan = JsonUtility.FromJson<LLMGameSystemPlan>(extractedJson);
                    }
                }
                else
                {
                    // Try direct parsing as fallback
                    plan = JsonUtility.FromJson<LLMGameSystemPlan>(contentToProcess);
                }

                // Basic validation: must have a systemName and at least one step
                if (plan == null || string.IsNullOrEmpty(plan.systemName) || plan.steps == null || plan.steps.Length == 0)
                    return false;

                return true;
            }
            catch (Exception ex)
            {
                Debug.LogError($"Error in LLMGameSystemPlan.TryParse: {ex.Message}");
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