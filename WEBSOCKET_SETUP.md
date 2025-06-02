# WebSocket Setup for Bifrost MCP

## Quick Fix

If you're seeing WebSocketSharp errors, the websocket-sharp.dll is included in two locations:

1. **In the package**: `Assets/Bifrost/Editor/Plugins/websocket-sharp.dll`
2. **In project root**: `Assets/Plugins/websocket-sharp.dll`

## Troubleshooting WebSocket Errors

### Error: "The type or namespace name 'WebSocketSharp' could not be found"

This means Unity can't find the websocket-sharp.dll. Solutions:

1. **Clear Package Cache** (Recommended)
   - Close Unity
   - Delete `Library/PackageCache/`
   - Reopen Unity

2. **Force Reimport**
   - Right-click on `Assets/Bifrost/Editor/Plugins/websocket-sharp.dll`
   - Select "Reimport"

3. **Manual Fix**
   - Ensure websocket-sharp.dll exists in `Assets/Bifrost/Editor/Plugins/`
   - Check that the .meta file has the correct settings
   - Verify assembly definition references the DLL

### Assembly Definition Settings

The `Bifrost.Editor.asmdef` should have:
```json
{
    "overrideReferences": true,
    "precompiledReferences": [
        "websocket-sharp.dll"
    ]
}
```

### Platform Settings

The websocket-sharp.dll should be configured for:
- **Editor**: Enabled
- **Standalone**: Disabled (Editor-only tool)

## Why WebSockets?

The MCP (Model Context Protocol) specification requires WebSocket transport for:
- Real-time bidirectional communication
- Low-latency Unity control
- Standard protocol compliance
- Compatibility with AI tools (Claude Desktop, Cursor, etc.)

## Alternative Solutions

If WebSocket setup continues to fail:

1. **Download from NuGet**
   ```bash
   # Download websocket-sharp
   wget https://www.nuget.org/api/v2/package/WebSocketSharp/1.0.3-rc11
   ```

2. **Use Unity Package Manager**
   - Some Unity versions include WebSocketSharp
   - Check Window > Package Manager

3. **Fallback TCP Mode**
   - Limited functionality
   - Not MCP-compliant
   - For testing only