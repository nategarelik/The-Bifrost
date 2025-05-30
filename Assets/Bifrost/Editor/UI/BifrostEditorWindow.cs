using UnityEditor;
using UnityEngine;
using Bifrost.Editor.UI;
using Bifrost.Editor;
using System.Threading.Tasks;

#pragma warning disable CS0414 // Field is assigned but its value is never used

public class BifrostEditorWindow : EditorWindow
{
    private bool showOnboarding = false;
    private const string ONBOARDING_SHOWN_KEY = "Bifrost_OnboardingShown";
    private BifrostPromptLibraryUI promptLibraryUI;
    private bool showSteamGuide = false;
    private PromptTemplateManager promptManager;
    private enum Tab { Chat, Settings, Modes }
    private Tab currentTab = Tab.Chat;
    private string errorMessage = null;
    private BifrostSettingsUI settingsUI;
    private BifrostModeEditor modeEditor;
    private BifrostChatUI chatUI;

    [MenuItem("Window/Bifrost Editor")]
    public static void ShowWindow()
    {
        GetWindow<BifrostEditorWindow>("Bifrost Editor");
    }

    private void OnEnable()
    {
        Bifrost.Editor.UI.BifrostSettingsUI.EnsureBifrostResourcesFolder();
        chatUI = new BifrostChatUI();
        settingsUI = new BifrostSettingsUI();
        modeEditor = new BifrostModeEditor();
        if (!EditorPrefs.GetBool(ONBOARDING_SHOWN_KEY, false))
        {
            showOnboarding = true;
            EditorPrefs.SetBool(ONBOARDING_SHOWN_KEY, true);
        }
        promptManager = new PromptTemplateManager();
        promptLibraryUI = new BifrostPromptLibraryUI(promptManager);
    }

    private void OnGUI()
    {
        // Null checks and re-initialization
        if (chatUI == null) chatUI = new BifrostChatUI();
        if (settingsUI == null) settingsUI = new BifrostSettingsUI();
        if (modeEditor == null) modeEditor = new BifrostModeEditor();
        if (promptLibraryUI == null && promptManager != null) promptLibraryUI = new BifrostPromptLibraryUI(promptManager);

        DrawTabs();
        EditorGUILayout.Space();

        if (showSteamGuide)
        {
            DrawSteamGuidePanel();
            return;
        }

        if (!string.IsNullOrEmpty(errorMessage))
        {
            EditorGUILayout.HelpBox(errorMessage, MessageType.Error);
        }

        switch (currentTab)
        {
            case Tab.Chat:
                if (chatUI != null) DrawChatTab();
                break;
            case Tab.Settings:
                if (settingsUI != null) settingsUI.Draw();
                break;
            case Tab.Modes:
                if (modeEditor != null) modeEditor.Draw();
                break;
            case (Tab)3:
                if (promptLibraryUI != null) DrawPromptLibraryTab();
                break;
        }
    }

    private void DrawTabs()
    {
        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Toggle(currentTab == Tab.Chat, new GUIContent("Chat", "Chat with the AI assistant"), EditorStyles.toolbarButton)) currentTab = Tab.Chat;
        if (GUILayout.Toggle(currentTab == Tab.Settings, new GUIContent("Settings", "Configure LLM and tool options"), EditorStyles.toolbarButton)) currentTab = Tab.Settings;
        if (GUILayout.Toggle(currentTab == Tab.Modes, new GUIContent("Modes", "Switch between Bifrost modes"), EditorStyles.toolbarButton)) currentTab = Tab.Modes;
        if (GUILayout.Toggle(currentTab == (Tab)3, new GUIContent("Prompt Library", "Browse and use prompt templates"), EditorStyles.toolbarButton)) currentTab = (Tab)3;
        if (GUILayout.Button(new GUIContent("Steam Guide", "How to get a Steam-ready game"), EditorStyles.toolbarButton)) showSteamGuide = true;
        GUILayout.FlexibleSpace();
        EditorGUILayout.EndHorizontal();
    }

    private void DrawOnboardingPanel()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("Welcome to The Bifrost!", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("\nBifrost lets you use natural language to develop Unity games.\n\n- Use the chat to ask for scripts, assets, or scene changes.\n- Configure your LLM provider and advanced options in Settings.\n- Use the Prompt Library for quick-start templates.\n\nFor more help, see the documentation or contact support.");
        if (GUILayout.Button("Close"))
        {
            showOnboarding = false;
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawPromptLibraryTab()
    {
        promptLibraryUI.Draw();
        if (promptLibraryUI.SelectedTemplateContent != null)
        {
            if (GUILayout.Button("Insert into Chat Input"))
            {
                chatUI.SetInput(promptLibraryUI.SelectedTemplateContent);
            }
        }
    }

    private void DrawSteamGuidePanel()
    {
        EditorGUILayout.BeginVertical("box");
        EditorGUILayout.LabelField("How to Get a Steam-Ready Game", EditorStyles.boldLabel);
        EditorGUILayout.LabelField("1. Prepare your game for build (scenes, assets, settings).\n2. Integrate Steamworks.NET or Facepunch.Steamworks.\n3. Set up Steam app on Steamworks dashboard.\n4. Build and upload using SteamPipe.\n5. Test with Steam client.\n6. Follow Steam's release checklist.");
        if (GUILayout.Button("Close"))
        {
            showSteamGuide = false;
        }
        EditorGUILayout.EndVertical();
    }

    private void DrawChatTab()
    {
        try
        {
            chatUI.Draw(new Rect(0, 0, position.width, position.height));
        }
        catch (System.Exception ex)
        {
            Debug.LogError($"BifrostEditorWindow.DrawChatTab Exception: {ex.Message}\n{ex.StackTrace}");
            errorMessage = $"Error in chat UI: {ex.Message}";
        }
    }
}