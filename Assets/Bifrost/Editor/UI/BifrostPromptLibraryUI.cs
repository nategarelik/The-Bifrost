using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Bifrost.Editor.UI
{
    /// <summary>
    /// UI for browsing and copying prompt templates from the Bifrost Prompt Library.
    /// </summary>
    public class BifrostPromptLibraryUI
    {
        private PromptTemplateManager promptManager;
        private List<string> templateNames = new List<string>();
        private int selectedIndex = -1;
        private Vector2 scrollPos;
        private string selectedContent = "";

        public BifrostPromptLibraryUI(PromptTemplateManager promptManager)
        {
            this.promptManager = promptManager;
            ReloadTemplates();
        }

        public void ReloadTemplates()
        {
            templateNames = promptManager.GetAllTemplateNames();
            selectedIndex = -1;
            selectedContent = "";
        }

        public void Draw()
        {
            EditorGUILayout.LabelField("Prompt Template Library", EditorStyles.boldLabel);
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical(GUILayout.Width(200));
            scrollPos = EditorGUILayout.BeginScrollView(scrollPos, GUILayout.Height(300));
            for (int i = 0; i < templateNames.Count; i++)
            {
                if (GUILayout.Button(templateNames[i], (selectedIndex == i) ? EditorStyles.toolbarButton : EditorStyles.miniButton))
                {
                    selectedIndex = i;
                    selectedContent = promptManager.GetTemplate(templateNames[i]);
                }
            }
            EditorGUILayout.EndScrollView();
            EditorGUILayout.EndVertical();
            EditorGUILayout.BeginVertical();
            if (selectedIndex >= 0)
            {
                EditorGUILayout.LabelField($"Template: {templateNames[selectedIndex]}", EditorStyles.boldLabel);
                EditorGUILayout.TextArea(selectedContent, GUILayout.Height(250));
                if (GUILayout.Button("Copy to Clipboard"))
                {
                    EditorGUIUtility.systemCopyBuffer = selectedContent;
                    EditorUtility.DisplayDialog("Copied!", "Prompt template copied to clipboard.", "OK");
                }
            }
            else
            {
                EditorGUILayout.HelpBox("Select a template to view its content.", MessageType.Info);
            }
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        public string SelectedTemplateContent => selectedContent;
    }
}