using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System;

namespace Bifrost.Editor.UI
{
    /// <summary>
    /// Chat interface for the Bifrost AI Assistant Unity Editor tool.
    /// Handles message display, input, and chat history management.
    /// </summary>
    public class BifrostChatUI
    {
        private const float INPUT_HEIGHT = 60f;
        private const float SEND_BUTTON_WIDTH = 60f;
        private const float SCROLL_BAR_WIDTH = 15f;
        private const float MESSAGE_PADDING = 10f;
        private const int MAX_HISTORY = 100;

        private Vector2 scrollPosition;
        private string currentInput = "";
        private List<ChatMessage> chatHistory;
        private GUIStyle messageStyle;
        private GUIStyle inputStyle;
        private bool isProcessing;

        // Event fired when user sends a message
        public event Action<string> OnMessageSent;

        // Event fired when user requests to clear chat
        public event Action OnClearChat;

        public BifrostChatUI()
        {
            chatHistory = new List<ChatMessage>();
            InitializeStyles();
            AddWelcomeMessage();
        }

        private void InitializeStyles()
        {
            messageStyle = new GUIStyle(EditorStyles.helpBox)
            {
                richText = true,
                wordWrap = true,
                padding = new RectOffset(10, 10, 10, 10),
                margin = new RectOffset(5, 5, 5, 5)
            };

            inputStyle = new GUIStyle(EditorStyles.textArea)
            {
                wordWrap = true,
                richText = true,
                padding = new RectOffset(8, 8, 8, 8)
            };
        }

        private void AddWelcomeMessage()
        {
            AddMessage(new ChatMessage
            {
                Content = "Welcome to Bifrost! I'm your AI game development assistant. How can I help you create today?",
                IsUser = false,
                Timestamp = DateTime.Now
            });
        }

        public void Draw(Rect position)
        {
            DrawToolbar(position);
            DrawChatHistory(position);
            DrawInputArea(position);
        }

        private void DrawToolbar(Rect position)
        {
            GUILayout.BeginHorizontal(EditorStyles.toolbar);

            if (GUILayout.Button(new GUIContent("Clear Chat", "Clear the chat history"), EditorStyles.toolbarButton))
            {
                if (EditorUtility.DisplayDialog("Clear Chat",
                    "Are you sure you want to clear the chat history?",
                    "Yes", "No"))
                {
                    chatHistory.Clear();
                    AddWelcomeMessage();
                    OnClearChat?.Invoke();
                }
            }

            GUILayout.FlexibleSpace();

            // Processing indicator
            if (isProcessing)
            {
                GUILayout.Label(new GUIContent("Processing...", "The AI is working on your request."), EditorStyles.toolbarButton);
            }

            GUILayout.EndHorizontal();
        }

        private void DrawChatHistory(Rect position)
        {
            float chatAreaHeight = position.height - INPUT_HEIGHT - EditorStyles.toolbar.fixedHeight;
            Rect chatArea = new Rect(0, EditorStyles.toolbar.fixedHeight, position.width, chatAreaHeight);

            scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true,
                GUILayout.Width(chatArea.width),
                GUILayout.Height(chatArea.height));

            foreach (var message in chatHistory)
            {
                DrawMessage(message, chatArea.width);
            }

            GUILayout.EndScrollView();

            // Auto-scroll to bottom when new messages arrive
            if (Event.current.type == EventType.Repaint)
            {
                scrollPosition.y = float.MaxValue;
            }
        }

        private void DrawMessage(ChatMessage message, float chatWidth)
        {
            GUIStyle style = new GUIStyle(messageStyle);
            style.normal.textColor = message.IsUser ?
                EditorGUIUtility.isProSkin ? Color.white : Color.black :
                EditorGUIUtility.isProSkin ? Color.cyan : Color.blue;

            EditorGUILayout.BeginHorizontal();

            if (message.IsUser)
                GUILayout.Space(SCROLL_BAR_WIDTH + MESSAGE_PADDING);

            EditorGUILayout.LabelField($"<b>{(message.IsUser ? "You" : "Bifrost")}</b> ({message.Timestamp:HH:mm})\n{message.Content}",
                style,
                GUILayout.MaxWidth(chatWidth - SCROLL_BAR_WIDTH - MESSAGE_PADDING * 2));

            if (!message.IsUser)
                GUILayout.Space(SCROLL_BAR_WIDTH + MESSAGE_PADDING);

            EditorGUILayout.EndHorizontal();
        }

        private void DrawInputArea(Rect position)
        {
            Rect inputArea = new Rect(0, position.height - INPUT_HEIGHT, position.width, INPUT_HEIGHT);
            GUILayout.BeginArea(inputArea);

            EditorGUILayout.BeginHorizontal();

            // Multi-line input field
            GUI.SetNextControlName("ChatInput");
            currentInput = EditorGUILayout.TextArea(currentInput, inputStyle,
                GUILayout.Height(INPUT_HEIGHT - EditorGUIUtility.standardVerticalSpacing * 2));

            // Send button
            GUI.enabled = !string.IsNullOrWhiteSpace(currentInput) && !isProcessing;
            if (GUILayout.Button("Send", GUILayout.Width(SEND_BUTTON_WIDTH),
                GUILayout.Height(INPUT_HEIGHT - EditorGUIUtility.standardVerticalSpacing * 2)))
            {
                SendMessage();
            }
            GUI.enabled = true;

            EditorGUILayout.EndHorizontal();
            GUILayout.EndArea();

            // Handle Enter key to send message (Shift+Enter for new line)
            if (Event.current.type == EventType.KeyDown &&
                Event.current.keyCode == KeyCode.Return &&
                !Event.current.shift &&
                GUI.GetNameOfFocusedControl() == "ChatInput")
            {
                Event.current.Use();
                SendMessage();
            }
        }

        private void SendMessage()
        {
            if (string.IsNullOrWhiteSpace(currentInput) || isProcessing)
                return;

            string messageContent = currentInput.Trim();
            AddMessage(new ChatMessage
            {
                Content = messageContent,
                IsUser = true,
                Timestamp = DateTime.Now
            });

            OnMessageSent?.Invoke(messageContent);
            currentInput = "";
            GUI.FocusControl("ChatInput");
        }

        public void AddResponse(string response)
        {
            AddMessage(new ChatMessage
            {
                Content = response,
                IsUser = false,
                Timestamp = DateTime.Now
            });
        }

        public void SetProcessingState(bool processing)
        {
            isProcessing = processing;
        }

        private void AddMessage(ChatMessage message)
        {
            chatHistory.Add(message);
            if (chatHistory.Count > MAX_HISTORY)
            {
                chatHistory.RemoveAt(0);
            }
        }

        public void SetInput(string text)
        {
            currentInput = text;
        }
    }

    /// <summary>
    /// Represents a single chat message in the Bifrost chat interface
    /// </summary>
    public class ChatMessage
    {
        public string Content { get; set; }
        public bool IsUser { get; set; }
        public DateTime Timestamp { get; set; }
    }
}