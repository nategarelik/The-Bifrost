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

    /// <summary>
    /// ScriptableObject for storing Bifrost AI mode definitions
    /// </summary>
    public class BifrostModeLibrary : ScriptableObject
    {
        private const string LIBRARY_PATH = "Assets/Bifrost/Resources/BifrostModeLibrary.asset";

        [SerializeField] private List<BifrostMode> modes = new List<BifrostMode>
        {
            new BifrostMode { name = "Code", description = "Code generation and editing mode.", promptTemplate = "Prompts/CodePrompt" },
            new BifrostMode { name = "Architect", description = "System and architecture design mode.", promptTemplate = "Prompts/ArchitectPrompt" },
            new BifrostMode { name = "Designer", description = "Game and level design mode.", promptTemplate = "Prompts/DesignerPrompt" },
            new BifrostMode { name = "GameDev", description = "General Unity game development mode.", promptTemplate = "Prompts/GameDevPrompt" }
        };

        public List<BifrostMode> Modes => modes;

        public static BifrostModeLibrary GetOrCreateLibrary()
        {
            var library = AssetDatabase.LoadAssetAtPath<BifrostModeLibrary>(LIBRARY_PATH);
            if (library == null)
            {
                library = ScriptableObject.CreateInstance<BifrostModeLibrary>();
                AssetDatabase.CreateAsset(library, LIBRARY_PATH);
                AssetDatabase.SaveAssets();
            }
            return library;
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