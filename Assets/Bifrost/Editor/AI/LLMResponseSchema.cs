using System;
using UnityEngine;
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;

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
                                Debug.Log($"Extracted content from OpenRouter response: {contentToProcess.Substring(0, Math.Min(200, contentToProcess.Length))}...");
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
                        if (jsonStart < 0)
                        {
                            // If no newline, check if it's ```{ format
                            if (contentToProcess.Length > codeBlockStart + 3 && contentToProcess[codeBlockStart + 3] == '{')
                            {
                                jsonStart = codeBlockStart + 3;
                            }
                            else
                            {
                                jsonStart = codeBlockStart + 3; // Skip ```
                                // Skip past "json" if present
                                if (contentToProcess.Substring(jsonStart).StartsWith("json"))
                                {
                                    jsonStart += 4;
                                }
                            }
                        }
                        else
                        {
                            jsonStart++; // Move past the newline
                        }

                        // Find the closing ```
                        int codeBlockEnd = contentToProcess.IndexOf("```", jsonStart);
                        if (codeBlockEnd > jsonStart)
                        {
                            contentToProcess = contentToProcess.Substring(jsonStart, codeBlockEnd - jsonStart).Trim();
                            Debug.Log($"Extracted JSON from markdown: {contentToProcess.Substring(0, Math.Min(200, contentToProcess.Length))}...");
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
                        Debug.Log($"Final JSON to parse: {extractedJson.Substring(0, Math.Min(300, extractedJson.Length))}...");

                        // Use Newtonsoft.Json for more robust parsing
                        plan = JsonConvert.DeserializeObject<LLMGameSystemPlan>(extractedJson);
                    }
                }
                else
                {
                    // Try direct parsing as fallback
                    Debug.Log($"Direct parsing attempt: {contentToProcess.Substring(0, Math.Min(200, contentToProcess.Length))}...");
                    plan = JsonConvert.DeserializeObject<LLMGameSystemPlan>(contentToProcess);
                }

                // Basic validation: must have a systemName and at least one step
                if (plan == null || string.IsNullOrEmpty(plan.systemName) || plan.steps == null || plan.steps.Length == 0)
                {
                    Debug.LogWarning("Plan validation failed: missing systemName or steps");
                    return false;
                }

                Debug.Log($"Successfully parsed plan: {plan.systemName} with {plan.steps.Length} steps");
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