# 🦄 Bifrost: Next-Generation AI-Powered Unity MCP Game Factory

**The most advanced autonomous game development platform for Unity - enabling AI agents to create complete, Steam-publishable games through natural language.**

[![Unity 2020.3+](https://img.shields.io/badge/Unity-2020.3%2B-blue.svg)](https://unity.com)
[![MCP Protocol](https://img.shields.io/badge/MCP-2024--11--05-green.svg)](https://modelcontextprotocol.io)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://opensource.org/licenses/MIT)

---

## 🌟 Overview

Bifrost (formerly UnityBridge) is a revolutionary Unity Editor extension that transforms game development by exposing the entire Unity Editor as a Model Context Protocol (MCP) server. This enables local or remote AI agents (including Claude, GPT-4, Gemini, and local Ollama models) to autonomously design, build, test, and iterate on complete games with minimal human intervention.

### Key Innovations

- **🤖 Full Unity Editor Control**: Every Unity operation available through MCP tools
- **🧠 Multi-Provider AI Support**: OpenAI, Anthropic, Gemini, Ollama (local), and more
- **🔄 Real-time State Synchronization**: Live Unity scene updates streamed to AI agents
- **📊 Autonomous Agent System**: AI agents can plan and execute complex game development tasks
- **🛠️ 40+ Unity Tools**: Comprehensive coverage of scenes, GameObjects, assets, physics, UI, and more
- **💾 Intelligent Memory**: Context-aware memory system for efficient long-running operations
- **🎮 Steam-Ready Output**: Generate complete, publishable games

## 🚀 Quick Start

### Installation

#### Option 1: Unity Package Manager (Recommended)
1. Open Unity (2020.3 or newer)
2. Go to **Window > Package Manager**
3. Click **+** → **Add package from git URL...**
4. Enter: `https://github.com/nategarelik/The-Bifrost.git?path=Assets/Bifrost`

#### Option 2: Manual Installation
1. Clone this repository
2. Copy `Assets/Bifrost` to your Unity project's Assets folder

### First Run

1. Open **Window > Bifrost AI Assistant**
2. Configure your AI provider in the Settings tab
3. Open **Window > Bifrost > MCP Control Panel**
4. Start the MCP server
5. Connect your AI agent (Claude Desktop, Cursor, or custom client)

## 🏗️ Architecture

### System Overview

```
┌─────────────────────────────────────────────────────────────────┐
│                        Unity Editor                              │
├─────────────────────────────────────────────────────────────────┤
│                    Bifrost MCP Layer                             │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────────────┐     │
│  │ MCP Server  │  │ Tool Registry │  │ Resource Registry  │     │
│  │ (WebSocket) │  │  (40+ tools)  │  │  (Live Unity data) │     │
│  └──────┬──────┘  └──────┬───────┘  └─────────┬──────────┘     │
│         │                 │                    │                 │
│  ┌──────▼─────────────────▼────────────────────▼───────────┐    │
│  │              Unity Operation Executors                   │    │
│  └──────────────────────────────────────────────────────────┘    │
├─────────────────────────────────────────────────────────────────┤
│                    Agent Autonomy System                         │
│  ┌─────────────┐  ┌──────────────┐  ┌────────────────────┐     │
│  │   Planner   │  │Context Manager│  │  Memory System     │     │
│  └─────────────┘  └──────────────┘  └────────────────────┘     │
└─────────────────────────────────────────────────────────────────┘
                              ▲
                              │ MCP Protocol (JSON-RPC)
                              ▼
┌─────────────────────────────────────────────────────────────────┐
│              External AI Agents / LLMs                           │
│  Claude Desktop │ Cursor │ GPT-4 │ Gemini │ Ollama │ Custom    │
└─────────────────────────────────────────────────────────────────┘
```

### Core Components

#### 1. MCP Server (`Assets/Bifrost/Editor/AI/MCP/`)
- **MCPServerEnhanced**: WebSocket server implementing MCP 2024-11-05 spec
- **MCPProtocol**: Full protocol implementation with tool/resource management
- **MCPToolRegistry**: Dynamic tool registration and discovery
- **MCPResourceRegistry**: Live Unity data exposure

#### 2. Unity Tools (`Assets/Bifrost/Editor/AI/MCP/Tools/`)
Comprehensive Unity operations organized by category:

**Scene Management**
- `create_scene` - Create scenes with configuration
- `load_scene` - Load scenes (additive/single)
- `save_scene` - Save current or specific scenes
- `analyze_scene` - Detailed scene analysis

**GameObject Operations**
- `create_gameobject_advanced` - Full component setup
- `find_gameobjects` - Advanced queries
- `modify_transform` - Batch transform operations
- `destroy_gameobject` - Safe destruction

**Asset Tools** (extensible)
- Material creation and modification
- Texture generation
- Prefab management
- Script generation

#### 3. Agent System (`Assets/Bifrost/Editor/AI/Agent/`)
- **AgentContextManager**: Intelligent context building
- **AgentPlanner**: Multi-step execution planning
- **AgentMemory**: Short/long-term memory management
- **CostTracker**: Token usage and cost optimization

#### 4. Unity Integration (`Assets/Bifrost/Editor/AI/Unity/`)
- **UnityStateSynchronizer**: Real-time event monitoring
- **UnityOperationQueue**: Thread-safe operation execution

#### 5. LLM Providers (`Assets/Bifrost/Editor/AI/Providers/`)
- **OllamaProvider**: Local LLM support (NEW!)
- Enhanced providers with streaming and tool calling
- Unified interface for all providers

## 📚 MCP Tools Reference

### Scene Management Tools

| Tool | Description | Key Parameters |
|------|-------------|----------------|
| `create_scene` | Create new scene | name, addToBuildSettings, makeActive |
| `load_scene` | Load existing scene | scene, mode (Single/Additive) |
| `save_scene` | Save scene | sceneName, saveAs |
| `analyze_scene` | Get scene details | includeComponents, maxDepth |

### GameObject Tools

| Tool | Description | Key Parameters |
|------|-------------|----------------|
| `create_gameobject_advanced` | Create with full setup | name, primitiveType, position, components |
| `find_gameobjects` | Search by criteria | name, tag, layer, componentType |
| `modify_transform` | Change transform | target, position, rotation, scale |
| `destroy_gameobject` | Remove from scene | target, includeChildren |

### More Tools Coming
- Component manipulation
- Physics configuration
- UI creation
- Animation setup
- Audio management
- Build automation

## 🎯 Usage Examples

### Basic Scene Creation
```javascript
// MCP Tool Call
{
  "tool": "create_scene",
  "arguments": {
    "name": "MainMenu",
    "addToBuildSettings": true
  }
}
```

### Complex GameObject Creation
```javascript
{
  "tool": "create_gameobject_advanced",
  "arguments": {
    "name": "Player",
    "primitiveType": "Capsule",
    "position": { "x": 0, "y": 1, "z": 0 },
    "components": [
      { "type": "Rigidbody", "properties": { "mass": 1.5 } },
      { "type": "CapsuleCollider" }
    ]
  }
}
```

## 🤖 AI Agent Examples

### Autonomous Game Creation
```
User: Create a complete 3D platformer with 5 levels
Agent: *Creates scene hierarchy, player controller, platforms, collectibles, UI, and game logic*
```

### Intelligent Iteration
```
User: The player feels too floaty
Agent: *Analyzes physics settings, adjusts gravity and jump parameters, tests changes*
```

## 🛠️ Configuration

### MCP Server Settings
- **Port**: Default 8090 (configurable)
- **WebSocket Protocol**: Standard WebSocket
- **Authentication**: Optional token-based

### LLM Provider Settings
Configure in Settings tab:
- API keys for cloud providers
- Ollama endpoint for local models
- Model selection per provider
- Token limits and temperature

### Agent Settings
- Memory retention limits
- Cost controls
- Execution timeouts
- Safety constraints

## 📊 Resources

MCP Resources provide live Unity data:

| Resource URI | Description |
|--------------|-------------|
| `unity://scene/hierarchy` | Current scene structure |
| `unity://project/structure` | Project overview |
| `unity://selection` | Selected objects |
| `unity://console/logs` | Console output |
| `unity://build/settings` | Build configuration |
| `unity://assets/list` | Project assets |

## 🧪 Testing

Run tests via Unity Test Runner:
- **Window > General > Test Runner**
- Run EditMode tests for MCP protocol
- Integration tests for full workflows

## 🔒 Security

- MCP server runs locally by default
- Optional authentication for remote connections
- Sandboxing for untrusted agents
- Operation limits and quotas
- Full audit trail

## 🚀 Roadmap

### Phase 1 (Complete) ✅
- Core MCP infrastructure
- Basic Unity tools
- Multi-provider support

### Phase 2 (In Progress) 🔄
- Extended tool coverage
- Advanced agent autonomy
- Performance optimization

### Phase 3 (Planned) 📋
- Visual debugging overlay
- Multiplayer game support
- Asset store integration
- Cloud deployment

## 🤝 Contributing

See [CONTRIBUTING.md](CONTRIBUTING.md) for guidelines.

### Development Setup
1. Fork the repository
2. Create feature branch
3. Implement with tests
4. Submit pull request

## 📄 License

MIT License - see [LICENSE](LICENSE) file.

## 🙏 Acknowledgments

- Unity Technologies for the amazing engine
- Anthropic for the MCP specification
- The open-source community

## 📞 Support

- [GitHub Issues](https://github.com/nategarelik/The-Bifrost/issues)
- [Discord Community](https://discord.gg/bifrost) (Coming soon)
- [Documentation Wiki](https://github.com/nategarelik/The-Bifrost/wiki)

---

**Transform your game development workflow with Bifrost - where AI agents become game developers!**

© 2024 Nate Garelik. Built with ❤️ for the Unity community.
## 🔧 Troubleshooting
