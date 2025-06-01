# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## Project Overview

Bifrost is a next-generation AI-powered Unity Editor extension that exposes the entire Unity Editor as a Model Context Protocol (MCP) server. It enables AI agents to autonomously create complete, Steam-publishable games through natural language interaction.

## Unity Package Structure

This is a Unity Package Manager (UPM) compatible package. The main code resides in `Assets/Bifrost/Editor/` since it's Editor-only functionality.

## Development Commands

### Unity-Specific Operations
- **Open in Unity**: This is a Unity Editor extension, not a standalone application. Open the project in Unity 2020.3+
- **Test in Unity**: Use Unity's Test Runner (Window > General > Test Runner) to run tests in `Assets/Bifrost/Tests/`
- **Recompile**: Unity automatically recompiles when C# files change
- **Debug**: Use Visual Studio/VS Code with Unity debugger attached

### Package Development
- **Install as Package**: Use Unity Package Manager with git URL: `https://github.com/nategarelik/The-Bifrost.git?path=Assets/Bifrost`
- **Local Development**: Copy `Assets/Bifrost` folder to your Unity project's Assets directory

## Enhanced Architecture (MCP Game Factory)

### MCP Layer (`Editor/AI/MCP/`)
The core MCP implementation that exposes Unity as a server:

1. **Protocol Layer**
   - `MCPProtocol.cs`: Implements MCP 2024-11-05 specification
   - `MCPServerEnhanced.cs`: WebSocket server with full protocol support
   - JSON-RPC 2.0 message handling

2. **Tool System**
   - `MCPToolRegistry.cs`: Dynamic tool registration with reflection
   - `MCPToolBase.cs`: Base class for all tools
   - Tools organized by category in `Tools/` subdirectories
   - Auto-discovery of tools at runtime

3. **Resource System**
   - `MCPResourceRegistry.cs`: Live Unity data exposure
   - `MCPResourceBase.cs`: Base class for resources
   - Resources in `Resources/UnityResources.cs`
   - Subscription support for real-time updates

### Unity Tools (`Editor/AI/MCP/Tools/`)
Comprehensive Unity operations:

- **Scene/**: Scene creation, loading, saving, analysis
- **GameObject/**: Object creation, modification, queries
- **Component/**: Component manipulation (coming soon)
- **Asset/**: Material, texture, prefab operations (coming soon)
- **Script/**: Code generation and modification (coming soon)
- **Physics/**: Physics configuration (coming soon)
- **UI/**: UI creation and setup (coming soon)
- **Audio/**: Audio management (coming soon)
- **Build/**: Build automation (coming soon)

### Agent System (`Editor/AI/Agent/`)
Autonomous execution capabilities:

1. **AgentContextManager**: Builds context from Unity state
2. **AgentPlanner**: Creates multi-step execution plans
3. **AgentMemory**: Short/long-term memory management
4. **CostTracker**: Token usage optimization

### Unity Integration (`Editor/AI/Unity/`)
Deep Unity Editor integration:

1. **UnityStateSynchronizer**: Real-time event monitoring
2. **UnityOperationQueue**: Thread-safe operation execution
3. Event hooks for hierarchy, selection, scene changes

### LLM Providers (`Editor/AI/Providers/`)
- **OllamaProvider**: NEW! Local LLM support
- Enhanced `IBifrostLLMProvider` interface with streaming/tools
- All providers now support the advanced interface

## Key Patterns

### 1. Tool Implementation
```csharp
[MCPTool("tool_name", "Tool description")]
public class MyTool : MCPToolBase
{
    public override JObject InputSchema => /* JSON Schema */;
    public override async Task<MCPToolCallResult> Execute(JObject args, MCPExecutionContext context)
    {
        // Tool implementation
        return Success("Result");
    }
}
```

### 2. Resource Implementation
```csharp
public class MyResource : MCPResourceBase
{
    public override string Uri => "unity://my/resource";
    public override async Task<MCPResourceReadResult> Read(MCPResourceReadParams params)
    {
        // Resource implementation
        return JsonResult(data);
    }
}
```

### 3. Unity Operations
Always use `UnityOperationQueue` for thread safety:
```csharp
await UnityOperationQueue.Instance.EnqueueMainThread(() => {
    // Unity API calls here
});
```

## Important Unity Considerations

1. **Main Thread Operations**: Unity APIs must be called on main thread
2. **EditorApplication.delayCall**: Use for deferred operations
3. **Undo System**: Register all operations with Undo.RegisterCreatedObjectUndo
4. **Asset Database**: Refresh after file operations
5. **Scene Management**: Use EditorSceneManager, not SceneManager
6. **Domain Reload**: Handle static variable reset on recompile

## MCP Protocol Details

### Message Format
```json
{
  "jsonrpc": "2.0",
  "id": "unique-id",
  "method": "tools/call",
  "params": {
    "name": "tool_name",
    "arguments": { /* tool args */ }
  }
}
```

### Tool Discovery
- Tools are auto-discovered via reflection
- Must inherit from `MCPToolBase` or implement `IMCPTool`
- Use `[MCPTool]` attribute for metadata

### Resource URIs
- `unity://scene/hierarchy` - Scene structure
- `unity://project/structure` - Project overview
- `unity://selection` - Current selection
- `unity://console/logs` - Console output

## Testing Approach

1. **Unit Tests**: Test individual components
2. **Integration Tests**: Test MCP protocol flow
3. **Unity Play Mode Tests**: Test actual Unity operations
4. **Mock Unity APIs**: For testing without Unity context

## Performance Considerations

1. **Batch Operations**: Combine multiple Unity operations
2. **Lazy Loading**: Only fetch needed context
3. **Token Optimization**: Minimize LLM token usage
4. **Caching**: Cache frequently accessed data
5. **Async Operations**: Use async/await properly

## Error Handling

1. **Graceful Degradation**: Continue operation on non-critical errors
2. **Clear Error Messages**: Provide actionable error information
3. **Undo Support**: Allow reverting failed operations
4. **Logging**: Comprehensive logging for debugging

## Security

1. **Local by Default**: MCP server binds to localhost
2. **Optional Auth**: Token-based authentication available
3. **Operation Limits**: Configurable rate limiting
4. **Sandboxing**: Restrict file system access

## Future Extensions

When adding new features:
1. Create new tool category if needed
2. Follow existing patterns
3. Add tests
4. Update resource URIs if adding new data sources
5. Consider agent autonomy implications