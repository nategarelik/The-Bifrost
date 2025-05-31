using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

namespace Bifrost.Editor.UI
{
    /// <summary>
    /// Editor UI for customizing Bifrost AI modes (Code, Architect, Designer, GameDev, etc.)
    /// </summary>
    public class BifrostModeEditor
    {
        private BifrostModeLibrary modeLibrary;
        private SerializedObject serializedLibrary;
        private SerializedProperty modesProperty;

        public BifrostModeEditor()
        {
            modeLibrary = BifrostModeLibrary.GetOrCreateLibrary();
            serializedLibrary = new SerializedObject(modeLibrary);
            modesProperty = serializedLibrary.FindProperty("modes");
        }

        public void Draw()
        {
            serializedLibrary.Update();

            EditorGUILayout.LabelField("Bifrost AI Modes", EditorStyles.boldLabel);
            EditorGUILayout.Space();

            for (int i = 0; i < modesProperty.arraySize; i++)
            {
                var modeProp = modesProperty.GetArrayElementAtIndex(i);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                EditorGUILayout.PropertyField(modeProp.FindPropertyRelative("name"), new GUIContent("Mode Name"));
                EditorGUILayout.PropertyField(modeProp.FindPropertyRelative("description"), new GUIContent("Description"));
                EditorGUILayout.PropertyField(modeProp.FindPropertyRelative("promptTemplate"), new GUIContent("Prompt Template (Resource Path)"));
                if (GUILayout.Button("Remove Mode"))
                {
                    modesProperty.DeleteArrayElementAtIndex(i);
                    break;
                }
                EditorGUILayout.EndVertical();
                EditorGUILayout.Space();
            }

            if (GUILayout.Button("Add New Mode"))
            {
                modesProperty.InsertArrayElementAtIndex(modesProperty.arraySize);
                var newMode = modesProperty.GetArrayElementAtIndex(modesProperty.arraySize - 1);
                newMode.FindPropertyRelative("name").stringValue = "New Mode";
                newMode.FindPropertyRelative("description").stringValue = "";
                newMode.FindPropertyRelative("promptTemplate").stringValue = "";
            }

            EditorGUILayout.Space();
            if (GUILayout.Button("Save Modes"))
            {
                serializedLibrary.ApplyModifiedProperties();
                EditorUtility.SetDirty(modeLibrary);
                AssetDatabase.SaveAssets();
            }
        }
    }

    [System.Serializable]
    public class BifrostMode
    {
        public string name;
        public string description;
        public string promptTemplate; // Resource path to prompt template
    }
}