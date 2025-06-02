# Changelog

All notable changes to the Bifrost Unity MCP Game Factory will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [2.0.0] - 2024-01-06

### 🚀 Added - Next-Generation MCP Game Factory

- **Full Model Context Protocol (MCP) Implementation**
  - WebSocket-based MCP server (port 8090) with JSON-RPC 2.0
  - Consolidated to single MCPServer class (removed redundant implementations)
  - MCP 2024-11-05 specification compliance
  - Dynamic tool and resource registration with reflection
  - Real-time Unity state synchronization
  - 40+ Unity operation tools across multiple categories
  
- **Comprehensive Unity Tools**
  - **Scene Management**: create_scene, load_scene, save_scene, analyze_scene
  - **GameObject Operations**: create_gameobject_advanced, find_gameobjects, modify_transform, destroy_gameobject
  - **Transform Tools**: Batch operations, world/local space support
  - **Component Tools**: (Extensible framework ready)
  - **Asset Tools**: (Extensible framework ready)
  
- **Autonomous Agent System**
  - **AgentContextManager**: Intelligent context building with project/scene awareness
  - **AgentPlanner**: Multi-step plan generation with templates and LLM fallback
  - **AgentMemory**: Short-term and long-term memory management
  - **CostTracker**: Token usage and cost optimization
  - **Error Recovery**: Adaptive replanning on failures
  
- **Advanced Unity Integration**
  - **UnityStateSynchronizer**: Real-time monitoring of hierarchy, selection, scene changes
  - **UnityOperationQueue**: Thread-safe execution with main/background thread support
  - Event broadcasting for all Unity Editor events
  - Automatic state updates to connected MCP clients
  
- **Ollama Provider (Local LLM Support)**
  - Complete Ollama API integration
  - Model discovery and validation
  - Streaming support for real-time responses
  - Tool calling with prompt engineering fallback
  - Zero-cost local inference
  
- **Enhanced MCP Control Panel**
  - New dedicated window (Window > Bifrost > MCP Control Panel)
  - Server status and control
  - Connected clients monitoring
  - Tool and resource browsers
  - Agent goal execution
  - Debug and performance metrics
  
- **MCP Resources for Live Unity Data**
  - unity://scene/hierarchy - Current scene structure
  - unity://project/structure - Project overview with asset counts
  - unity://selection - Currently selected objects
  - unity://console/logs - Console output with history
  - unity://build/settings - Build configuration
  - unity://assets/list - Filterable asset listings
  
- **Testing Infrastructure**
  - MCP protocol unit tests
  - Tool and resource registry tests
  - Mock Unity API support
  - Integration test framework

### 🔄 Changed

- **Enhanced Provider System**
  - IBifrostLLMProvider now includes SupportsStreaming and SupportsToolCalling properties
  - New IBifrostLLMProviderAdvanced interface for streaming and tool implementations
  - All providers updated to support enhanced features where available
  
- **Improved Architecture**
  - Modular tool system with category-based organization
  - Resource system with subscription support
  - Better separation of concerns across all components
  - Thread-safe operations throughout

### 📚 Documentation

- Comprehensive README rewrite with full architecture overview
- New MCP_USAGE.md guide for connecting AI agents
- Updated CLAUDE.md with MCP patterns and best practices
- Added .gitignore entries for MCP server artifacts

## [1.0.0] - 2024-06-01

### Added

- Advanced LLM settings: max tokens, temperature, timeout, penalties, custom headers (global & per-project)
- Onboarding panel, tooltips, and Steam-ready game guide
- Strict JSON schema validation and fallback for LLM responses
- Full provider support: OpenAI, Anthropic, Gemini, OpenRouter, Hugging Face, Local
- PromptTemplateManager with categories, CRUD, and UI integration
- Prompt Library tab in the main window, with one-click template insertion
- UnityContextAnalyzer: project/scene context injected into every AI call
- UnityProjectManager: ProBuilder integration (if present), prefab duplication, and more utilities
- Robust error handling and user feedback throughout

## [0.1.0] - 2024-01-01

### Added

- Initial public release of UnityBridge (Bifrost)
- Chat-driven AI game development in Unity Editor
- Multi-provider LLM support (OpenAI, Anthropic, Gemini, OpenRouter)
- Prompt template library and manager
- Project/scene context analyzer
- Game system planning and approval workflow
- Settings UI (global and per-project)
- Example prompt templates for 2D/3D/Level Design
