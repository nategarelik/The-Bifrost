# 🌉 Bifrost MCP Usage Guide

This guide explains how to use Bifrost's Model Context Protocol (MCP) server to enable AI agents to control Unity.

## Table of Contents
- [Quick Start](#quick-start)
- [MCP Server Setup](#mcp-server-setup)
- [Connecting AI Agents](#connecting-ai-agents)
- [Available Tools](#available-tools)
- [Resources](#resources)
- [Advanced Usage](#advanced-usage)
- [Troubleshooting](#troubleshooting)

## Quick Start

1. **Start Unity** with your project
2. Open **Window > Bifrost > MCP Control Panel**
3. Click **Start Server** (default port: 8090)
4. Connect your AI agent to `ws://localhost:8090`

## MCP Server Setup

### Starting the Server

```csharp
// The server starts automatically when you click "Start Server" in the UI
// Or programmatically:
var mcpServer = new MCPServerEnhanced(8090);
mcpServer.Start();
```

### Configuration Options

| Setting | Default | Description |
|---------|---------|-------------|
| Port | 8090 | WebSocket server port |
| Timeout | 60s | Request timeout |
| Max Connections | 10 | Maximum simultaneous clients |
| Log Level | Info | Logging verbosity |

### Server Status

The MCP Control Panel shows:
- 🟢 **Running**: Server is active
- 🔴 **Stopped**: Server is not running
- Connected clients list
- Real-time activity logs

## Connecting AI Agents

### Claude Desktop

1. Install Claude Desktop
2. Edit config at `%APPDATA%/Claude/claude_desktop_config.json`:

```json
{
  "mcpServers": {
    "bifrost": {
      "command": "npx",
      "args": ["-y", "@modelcontextprotocol/server-stdio", "ws://localhost:8090"]
    }
  }
}
```

### Cursor

1. Open Cursor settings
2. Add MCP server URL: `ws://localhost:8090`
3. Enable MCP tools in AI settings

### Custom Client

Connect any WebSocket client supporting MCP protocol:

```javascript
const ws = new WebSocket('ws://localhost:8090');

ws.on('open', () => {
  // Send initialization
  ws.send(JSON.stringify({
    jsonrpc: "2.0",
    id: "1",
    method: "initialize",
    params: {
      protocolVersion: "2024-11-05",
      clientInfo: {
        name: "My AI Agent",
        version: "1.0.0"
      }
    }
  }));
});
```

## Available Tools

### Scene Management

#### create_scene
Creates a new Unity scene.

```json
{
  "name": "create_scene",
  "arguments": {
    "name": "GameLevel1",
    "addToBuildSettings": true,
    "makeActive": true
  }
}
```

#### load_scene
Loads an existing scene.

```json
{
  "name": "load_scene",
  "arguments": {
    "scene": "MainMenu",
    "mode": "Single"  // or "Additive"
  }
}
```

#### save_scene
Saves the current or specified scene.

```json
{
  "name": "save_scene",
  "arguments": {
    "sceneName": "GameLevel1",
    "saveAs": "Assets/Scenes/Level1_backup.unity"
  }
}
```

#### analyze_scene
Gets detailed scene information.

```json
{
  "name": "analyze_scene",
  "arguments": {
    "includeComponents": true,
    "maxDepth": 5
  }
}
```

### GameObject Operations

#### create_gameobject_advanced
Creates GameObject with components.

```json
{
  "name": "create_gameobject_advanced",
  "arguments": {
    "name": "Player",
    "primitiveType": "Capsule",
    "position": { "x": 0, "y": 1, "z": 0 },
    "rotation": { "x": 0, "y": 0, "z": 0 },
    "scale": { "x": 1, "y": 1, "z": 1 },
    "parent": "GameObjects/Characters",
    "components": [
      {
        "type": "Rigidbody",
        "properties": {
          "mass": 1.5,
          "useGravity": true
        }
      },
      {
        "type": "CapsuleCollider",
        "properties": {
          "radius": 0.5,
          "height": 2
        }
      }
    ]
  }
}
```

#### find_gameobjects
Search for GameObjects.

```json
{
  "name": "find_gameobjects",
  "arguments": {
    "name": "Enemy",
    "tag": "Enemy",
    "layer": "Enemies",
    "componentType": "NavMeshAgent",
    "includeInactive": false
  }
}
```

#### modify_transform
Change GameObject transform.

```json
{
  "name": "modify_transform",
  "arguments": {
    "target": "Player",
    "position": { "x": 10, "y": 0, "z": 5 },
    "rotation": { "x": 0, "y": 180, "z": 0 },
    "scale": { "x": 1.5, "y": 1.5, "z": 1.5 },
    "space": "World"  // or "Local"
  }
}
```

#### destroy_gameobject
Remove GameObject from scene.

```json
{
  "name": "destroy_gameobject",
  "arguments": {
    "target": "Enemy_01",
    "includeChildren": true,
    "immediate": true
  }
}
```

## Resources

Resources provide read-only access to Unity data.

### unity://scene/hierarchy
Current scene structure as JSON.

```json
{
  "method": "resources/read",
  "params": {
    "uri": "unity://scene/hierarchy"
  }
}
```

### unity://project/structure
Project overview with asset counts.

```json
{
  "method": "resources/read",
  "params": {
    "uri": "unity://project/structure"
  }
}
```

### unity://selection
Currently selected objects.

```json
{
  "method": "resources/read",
  "params": {
    "uri": "unity://selection"
  }
}
```

### unity://console/logs
Recent console messages.

```json
{
  "method": "resources/read",
  "params": {
    "uri": "unity://console/logs"
  }
}
```

## Advanced Usage

### Batch Operations

Execute multiple operations efficiently:

```python
# Example: Create a complete game level
operations = [
    {"tool": "create_scene", "args": {"name": "Level1"}},
    {"tool": "create_gameobject_advanced", "args": {
        "name": "Terrain",
        "primitiveType": "Plane",
        "scale": {"x": 100, "y": 1, "z": 100}
    }},
    {"tool": "create_gameobject_advanced", "args": {
        "name": "Player",
        "primitiveType": "Capsule",
        "position": {"x": 0, "y": 1, "z": 0},
        "components": [{"type": "CharacterController"}]
    }},
    {"tool": "save_scene", "args": {}}
]
```

### Autonomous Agent Example

```python
# AI agent creating a game
async def create_platformer_game():
    # 1. Create main menu scene
    await call_tool("create_scene", {"name": "MainMenu"})
    
    # 2. Add UI elements
    await call_tool("create_ui_canvas", {"name": "MenuCanvas"})
    await call_tool("create_ui_button", {
        "name": "PlayButton",
        "text": "Play Game",
        "position": {"x": 0, "y": 0}
    })
    
    # 3. Create game level
    await call_tool("create_scene", {"name": "Level1"})
    
    # 4. Add player
    player_result = await call_tool("create_gameobject_advanced", {
        "name": "Player",
        "primitiveType": "Capsule",
        "components": [
            {"type": "CharacterController"},
            {"type": "PlayerMovement"}  # Custom script
        ]
    })
    
    # 5. Create platforms
    for i in range(10):
        await call_tool("create_gameobject_advanced", {
            "name": f"Platform_{i}",
            "primitiveType": "Cube",
            "position": {"x": i * 5, "y": i * 2, "z": 0},
            "scale": {"x": 4, "y": 0.5, "z": 4}
        })
```

### Real-time Monitoring

Subscribe to Unity state changes:

```javascript
// Subscribe to hierarchy changes
ws.send(JSON.stringify({
  method: "resources/subscribe",
  params: {
    uri: "unity://scene/hierarchy"
  }
}));

// Receive updates
ws.on('message', (data) => {
  const msg = JSON.parse(data);
  if (msg.method === "resources/updated") {
    console.log("Scene changed:", msg.params);
  }
});
```

## Troubleshooting

### Common Issues

#### Server Won't Start
- **Port in use**: Change port in settings
- **Firewall blocking**: Allow Unity through firewall
- **Unity not in play mode**: Some operations require play mode

#### Connection Failed
- **Wrong URL**: Ensure using `ws://` not `http://`
- **Server not running**: Check MCP Control Panel
- **Client timeout**: Increase timeout in client settings

#### Tools Not Working
- **Missing parameters**: Check tool schema
- **Invalid values**: Ensure correct types (numbers, strings)
- **Unity state**: Some tools require specific conditions

### Debug Mode

Enable detailed logging:
1. Open MCP Control Panel
2. Go to Debug tab
3. Enable "Show Detailed Logs"
4. Check Unity Console for errors

### Performance Tips

1. **Batch operations** when possible
2. **Use find sparingly** on large scenes
3. **Limit analyze_scene depth** for performance
4. **Cache resource reads** when appropriate

## Best Practices

1. **Always save scenes** after major changes
2. **Use descriptive names** for created objects
3. **Organize with empty GameObjects** as folders
4. **Test incrementally** with small operations
5. **Monitor performance** with Unity Profiler

## Examples Repository

Find more examples at:
- [Bifrost Examples](https://github.com/nategarelik/bifrost-examples)
- [Community Scripts](https://github.com/nategarelik/bifrost-community)

## Support

- **Documentation**: [Wiki](https://github.com/nategarelik/The-Bifrost/wiki)
- **Issues**: [GitHub Issues](https://github.com/nategarelik/The-Bifrost/issues)
- **Discord**: [Community Server](https://discord.gg/bifrost)

---

Happy AI-powered game development! 🎮🤖