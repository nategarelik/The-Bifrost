# Unity Package Common Fixes

## Namespace Issues

### Newtonsoft.Json
Unity uses a specific version of Newtonsoft.Json. Always use:
```csharp
using Unity.Plastic.Newtonsoft.Json;
using Unity.Plastic.Newtonsoft.Json.Linq;
```

NOT:
```csharp
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
```

## PackageCache Errors

When you see errors from `Library\PackageCache\com.nategarelik.bifrost@...`:

1. **Close Unity completely**
2. **Delete PackageCache**:
   ```bash
   rm -rf Library/PackageCache/
   ```
3. **Reopen Unity** - it will reimport with latest changes

## WebSocketSharp Dependency

The websocket-sharp.dll is included in:
- `Assets/Bifrost/Editor/Plugins/websocket-sharp.dll`

If missing, copy from:
- `Assets/Plugins/websocket-sharp.dll`

## Assembly Definition Settings

The `Bifrost.Editor.asmdef` must have:
```json
{
    "references": [
        "Unity.Plastic.Newtonsoft.Json"
    ],
    "overrideReferences": true,
    "precompiledReferences": [
        "websocket-sharp.dll"
    ]
}
```

## Version Conflicts

If Unity keeps using old package versions:
1. Increment version in `package.json`
2. Clear PackageCache
3. Restart Unity

## Quick Checklist

- [ ] All Newtonsoft references use Unity.Plastic prefix
- [ ] websocket-sharp.dll is in Editor/Plugins
- [ ] Assembly definition references both dependencies
- [ ] Package version incremented after changes
- [ ] PackageCache cleared after fixes