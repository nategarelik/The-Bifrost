# The Bifrost: Project Breakdown

## Project Overview

**The Bifrost** is an AI-powered Unity Editor extension that enables natural language-driven game development. Inspired by tools like Roo Code for VS Code, it allows users to create, modify, and manage Unity game projects using conversational prompts, with deep Unity integration and support for multiple LLM (Large Language Model) providers.

---

## Key Features Built

- **Unity Editor Integration:** Custom EditorWindow for chat-driven workflows.
- **AI Provider Abstraction:** Support for multiple LLMs (OpenAI, local, etc.).
- **Natural Language to Game Plan:** Converts user prompts into structured game system plans.
- **Asset Generation:** Automatically creates scripts, prefabs, and UI assets based on AI plans.
- **Robust Error Handling:** Handles Unity asset operations and AI failures gracefully.
- **Extensible Prompt System:** Modular prompt templates for different AI tasks.
- **Async/await Patterns:** Designed for responsive, non-blocking AI calls.

---

## What's Been Built

### 1. Editor UI and Workflow

- **BifrostEditorWindow.cs**  
  Main Editor window for user interaction. Handles chat UI, user input, tab navigation, and orchestrates the main workflow.

- **UI/BifrostChatUI.cs**  
  Chat interface logic for the Editor window, including message display and user input fields.

- **UI/BifrostModeEditor.cs**  
  UI for editing Bifrost AI modes.

- **UI/BifrostModeLibrary.cs**  
  ScriptableObject for storing mode definitions.

- **UI/BifrostPromptLibraryUI.cs**  
  UI for browsing and copying prompt templates.

- **UI/BifrostSettings.cs**  
  ScriptableObject for persistent Bifrost settings.

- **UI/BifrostSettingsUI.cs**  
  UI for configuring Bifrost settings (global and per-project).

### 2. AI Integration

- **BifrostAgent.cs**  
  Core logic for interacting with LLM providers, building prompts, and parsing AI responses.

- **AI/LLMResponseSchema.cs**  
  Defines the structure for parsing LLM (AI) responses (e.g., `LLMGameSystemPlan`).

- **AI/Providers/**

  - **HuggingFaceProvider.cs, LocalProvider.cs**: Implementations for HuggingFace and local LLM providers.

- **OpenRouterProvider.cs, OpenAIProvider.cs, GeminiProvider.cs, AnthropicProvider.cs**  
  Implementations of different LLM provider APIs.

- **IBifrostLLMProvider.cs**  
  Interface for LLM providers and request options.

- **AI/Prompts/PromptTemplateCategory.cs**  
  Enum for categorizing prompt templates.

- **PromptTemplateManager.cs**  
  Loads and manages AI prompt templates from Resources.

### 3. Game Plan and Asset Generation

- **GameSystemGenerator.cs**  
  Orchestrates the conversion of AI plans into Unity assets (scripts, prefabs, UIs). Also defines the `GameSystemPlan` class.

- **UnityProjectManager.cs**  
  Handles Unity asset operations: creating scripts, prefabs, UIs, scenes, and more.

- **UnityContextAnalyzer.cs**  
  Analyzes the current Unity project and scene context to provide structured information for AI prompts.

- **ImageTo3DGenerator.cs**  
  Handles generating 3D models from images (stubbed for now).

---

## What Still Needs To Be Done (Potential Next Steps)

1. **AI Plan Validation & Feedback:**

   - More robust validation of AI-generated plans before asset creation.
   - User feedback loop for correcting or refining AI plans.

2. **Advanced Asset Generation:**

   - Support for more complex asset types (e.g., ScriptableObjects, custom inspectors).
   - Smarter prefab and UI generation (e.g., auto-wiring components, layout).

3. **Multi-Provider Management:**

   - UI for switching between LLM providers and configuring provider-specific settings.

4. **Error Reporting & Logging:**

   - User-friendly error messages and logs for troubleshooting.

5. **Testing & Documentation:**

   - Unit tests for core logic (plan conversion, asset creation).
   - Expanded README and in-Editor documentation/help.

6. **User Experience Enhancements:**

   - More polished chat UI (history, context, markdown support).
   - Progress indicators for long-running operations.

7. **Security & API Key Management:**

   - Secure storage and management of API keys for LLM providers.

8. **Extensibility:**
   - Plugin system for custom prompts, asset generators, or AI providers.

---

## Directory and File Purpose

### Top-Level (Assets/Bifrost/Editor/)

- **BifrostEditorWindow.cs:**  
  Main Editor window, chat UI, user interaction, plan conversion.
- **BifrostAgent.cs:**  
  Core AI logic, provider management, prompt building.
- **UnityProjectManager.cs:**  
  Asset creation and management utilities.
- **GameSystemGenerator.cs:**  
  Converts plans into Unity assets and defines `GameSystemPlan`.
- **UnityContextAnalyzer.cs:**  
  Analyzes project/scene context for AI.
- **ImageTo3DGenerator.cs:**  
  (Stub) 3D model generation from images.
- **IBifrostLLMProvider.cs:**  
  LLM provider interface and request options.
- **PromptTemplateManager.cs:**  
  Loads/manages prompt templates.
- **OpenRouterProvider.cs, OpenAIProvider.cs, GeminiProvider.cs, AnthropicProvider.cs:**  
  LLM provider implementations.

### AI/

- **LLMResponseSchema.cs:**  
  AI-side plan structure (`LLMGameSystemPlan`).
- **Providers/**:  
  HuggingFace and Local LLM provider implementations.
- **Prompts/PromptTemplateCategory.cs:**  
  Enum for prompt template categories.

### UI/

- **BifrostChatUI.cs:**  
  Chat interface logic.
- **BifrostEditorWindow.cs:**  
  (Alternate/legacy) Editor window.
- **BifrostModeEditor.cs:**  
  UI for editing Bifrost modes.
- **BifrostModeLibrary.cs:**  
  ScriptableObject for mode definitions.
- **BifrostPromptLibraryUI.cs:**  
  UI for prompt template browsing.
- **BifrostSettings.cs:**  
  ScriptableObject for settings.
- **BifrostSettingsUI.cs:**  
  Settings/configuration UI.

### Resources/Prompts/

- Prompt template text files (e.g., `TwoD_BasicMovement.txt`, `ThreeD_CharacterController.txt`, etc.).

---

## Summary Table: Major Files

| File/Folder                            | Purpose                                                        |
| -------------------------------------- | -------------------------------------------------------------- |
| `BifrostEditorWindow.cs`               | Main Editor window, chat UI, user interaction, plan conversion |
| `BifrostAgent.cs`                      | Core AI logic, provider management, prompt building            |
| `UnityProjectManager.cs`               | Asset creation and management utilities                        |
| `GameSystemGenerator.cs`               | Converts plans into Unity assets and defines GameSystemPlan    |
| `UnityContextAnalyzer.cs`              | Analyzes project/scene context for AI                          |
| `ImageTo3DGenerator.cs`                | (Stub) 3D model generation from images                         |
| `IBifrostLLMProvider.cs`               | LLM provider interface and request options                     |
| `PromptTemplateManager.cs`             | Loads/manages prompt templates                                 |
| `AI/LLMResponseSchema.cs`              | AI-side plan structure (LLMGameSystemPlan)                     |
| `AI/Providers/`                        | HuggingFace and Local LLM provider implementations             |
| `AI/Prompts/PromptTemplateCategory.cs` | Enum for prompt template categories                            |
| `UI/BifrostChatUI.cs`                  | Chat interface logic                                           |
| `UI/BifrostEditorWindow.cs`            | (Alternate/legacy) Editor window                               |
| `UI/BifrostModeEditor.cs`              | UI for editing Bifrost modes                                   |
| `UI/BifrostModeLibrary.cs`             | ScriptableObject for mode definitions                          |
| `UI/BifrostPromptLibraryUI.cs`         | UI for prompt template browsing                                |
| `UI/BifrostSettings.cs`                | ScriptableObject for settings                                  |
| `UI/BifrostSettingsUI.cs`              | Settings/configuration UI                                      |
| `Resources/Prompts/`                   | Prompt template text files                                     |

---
