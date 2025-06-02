# Clearing Unity Package Cache

To resolve the compilation errors from cached package files, follow these steps:

## Method 1: Clear Package Cache (Recommended)
1. Close Unity Editor
2. Delete the following folders:
   - `Library/PackageCache/`
   - `Library/Packages/`
3. Reopen Unity - it will reimport the packages with the latest changes

## Method 2: Force Reimport
1. In Unity Editor, go to: **Window > Package Manager**
2. Find the Bifrost package
3. Click on it and select **Reimport**

## Method 3: Manual Cache Clear
1. Close Unity
2. Navigate to your project's Library folder
3. Delete the specific package cache:
   ```
   Library/PackageCache/com.nategarelik.bifrost@*
   ```
4. Reopen Unity

## Additional Steps if Errors Persist:
1. **Reimport All Assets**: Assets > Reimport All
2. **Clear Console**: Right-click in Console > Clear
3. **Restart Unity**: Sometimes a full restart is needed

## WebSocket DLL Location
The websocket-sharp.dll should be located at:
```
Assets/Plugins/websocket-sharp.dll
```

If it's missing, the MCP server won't compile properly.