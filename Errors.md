Library\PackageCache\com.nategarelik.bifrost@91129a11efa0\Editor\UI\BifrostEditorWindow.cs(29,24): warning CS1998: This async method lacks 'await' operators and will run synchronously. Consider using the 'await' operator to await non-blocking API calls, or 'await Task.Run(...)' to do CPU-bound work on a background thread.

The Bifrost is already up-to-date.
UnityEditor.EditorApplication:Internal_CallUpdateFunctions ()

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostSettings.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostSettings:GetOrCreateSettings () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostSettingsUI.cs:290)
Bifrost.Editor.UI.BifrostSettingsUI:.ctor () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostSettingsUI.cs:42)
Bifrost.Editor.BifrostEditorWindow/<OnEnable>d__17:MoveNext () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:44)
System.Runtime.CompilerServices.AsyncVoidMethodBuilder:Start<Bifrost.Editor.BifrostEditorWindow/<OnEnable>d__17> (Bifrost.Editor.BifrostEditorWindow/<OnEnable>d__17&)
Bifrost.Editor.BifrostEditorWindow:OnEnable ()
UnityEditor.EditorWindow:GetWindow<Bifrost.Editor.BifrostEditorWindow> (bool,string,bool)
Bifrost.Editor.BifrostEditorWindow:ShowWindow () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:37)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostSettings.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostSettings.GetOrCreateSettings () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostSettingsUI.cs:290)
Bifrost.Editor.UI.BifrostSettingsUI..ctor () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostSettingsUI.cs:42)
Bifrost.Editor.BifrostEditorWindow.OnEnable () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:44)
System.Runtime.CompilerServices.AsyncMethodBuilderCore+<>c.<ThrowAsync>b__7_0 (System.Object state) (at <71119615d44348f087b10ce3c1671c84>:0)
UnityEngine.UnitySynchronizationContext+WorkRequest.Invoke () (at <0022d4fb3cd44d45a62e51c39f257e7c>:0)
UnityEngine.UnitySynchronizationContext.Exec () (at <0022d4fb3cd44d45a62e51c39f257e7c>:0)
UnityEngine.UnitySynchronizationContext.ExecuteTasks () (at <0022d4fb3cd44d45a62e51c39f257e7c>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUIRaw (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUI (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleEventBubbleUp (UnityEngine.UIElements.EventBase evt) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.HandleEventAcrossPropagationPathWithCompatibilityEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.EventBase compatibilityEvt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.VisualElement target, System.Boolean isCapturingTarget) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.DispatchToElementUnderPointerOrPanelRoot (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, System.Int32 pointerId, UnityEngine.Vector2 position) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.DispatchToCapturingElementOrElementUnderPointer (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, System.Int32 pointerId, UnityEngine.Vector2 position) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.PointerDownEvent.Dispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.ProcessEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.Dispatch (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.SendEvent (UnityEngine.UIElements.EventBase e, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUIRaw (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUI (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleEventBubbleUp (UnityEngine.UIElements.EventBase evt) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.HandleEventAcrossPropagationPathWithCompatibilityEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.EventBase compatibilityEvt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.VisualElement target, System.Boolean isCapturingTarget) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.DispatchToCapturingElementOrElementUnderPointer (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, System.Int32 pointerId, UnityEngine.Vector2 position) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.PointerUpEvent.Dispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.ProcessEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.Dispatch (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.SendEvent (UnityEngine.UIElements.EventBase e, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUIRaw (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUI (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleEventBubbleUp (UnityEngine.UIElements.EventBase evt) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.HandleEventAcrossPropagationPathWithCompatibilityEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.EventBase compatibilityEvt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.VisualElement target, System.Boolean isCapturingTarget) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.DispatchToElementUnderPointerOrPanelRoot (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, System.Int32 pointerId, UnityEngine.Vector2 position) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.DispatchToCapturingElementOrElementUnderPointer (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, System.Int32 pointerId, UnityEngine.Vector2 position) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.PointerDownEvent.Dispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.ProcessEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.Dispatch (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.SendEvent (UnityEngine.UIElements.EventBase e, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

Parent directory must exist before creating asset at Assets/Bifrost/Resources/BifrostModeLibrary.asset.
UnityEditor.AssetDatabase:CreateAsset (UnityEngine.Object,string)
Bifrost.Editor.UI.BifrostModeLibrary:GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow:DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow:OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

GUI Error: Invalid GUILayout state in BifrostEditorWindow view. Verify that all layout Begin/End calls match
UnityEngine.GUIUtility:ProcessEvent (int,intptr,bool&)

UnityException: Creating asset at path Assets/Bifrost/Resources/BifrostModeLibrary.asset failed.
UnityEditor.AssetDatabase.CreateAsset (UnityEngine.Object asset, System.String path) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
Bifrost.Editor.UI.BifrostModeLibrary.GetOrCreateLibrary () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/UI/BifrostModeEditor.cs:88)
Bifrost.Editor.BifrostEditorWindow.DrawChatTab () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:103)
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:77)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUIRaw (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUI (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleEventBubbleUp (UnityEngine.UIElements.EventBase evt) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.HandleEventAcrossPropagationPathWithCompatibilityEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.EventBase compatibilityEvt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.VisualElement target, System.Boolean isCapturingTarget) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.DispatchToCapturingElementOrElementUnderPointer (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, System.Int32 pointerId, UnityEngine.Vector2 position) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.PointerUpEvent.Dispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.ProcessEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.Dispatch (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.SendEvent (UnityEngine.UIElements.EventBase e, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUIRaw (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUI (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleEventBubbleUp (UnityEngine.UIElements.EventBase evt) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.HandleEventAcrossPropagationPathWithCompatibilityEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.EventBase compatibilityEvt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.VisualElement target, System.Boolean isCapturingTarget) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.DispatchToElementUnderPointerOrPanelRoot (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, System.Int32 pointerId, UnityEngine.Vector2 position) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.DispatchToCapturingElementOrElementUnderPointer (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, System.Int32 pointerId, UnityEngine.Vector2 position) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.PointerDownEvent.Dispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.ProcessEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.Dispatch (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.SendEvent (UnityEngine.UIElements.EventBase e, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUIRaw (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.SendEventToIMGUI (UnityEngine.UIElements.EventBase evt, System.Boolean canAffectFocus, System.Boolean verifyBounds) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleEventBubbleUp (UnityEngine.UIElements.EventBase evt) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.HandleEventAcrossPropagationPathWithCompatibilityEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.EventBase compatibilityEvt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.VisualElement target, System.Boolean isCapturingTarget) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatchUtilities.DispatchToCapturingElementOrElementUnderPointer (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, System.Int32 pointerId, UnityEngine.Vector2 position) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.PointerUpEvent.Dispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.ProcessEvent (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.EventDispatcher.Dispatch (UnityEngine.UIElements.EventBase evt, UnityEngine.UIElements.BaseVisualElementPanel panel, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.SendEvent (UnityEngine.UIElements.EventBase e, UnityEngine.UIElements.DispatchMode dispatchMode) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility:ProcessEvent(Int32, IntPtr, Boolean&)

NullReferenceException: Object reference not set to an instance of an object
Bifrost.Editor.BifrostEditorWindow.OnGUI () (at Library/PackageCache/com.nategarelik.bifrost@91129a11efa0/Editor/BifrostEditorWindow.cs:80)
UnityEditor.HostView.InvokeOnGUI (UnityEngine.Rect onGUIPosition) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.DrawView (UnityEngine.Rect dockAreaRect) (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEditor.DockArea.OldOnGUI () (at <1d4abd3cd5ee415ab29235fc867cd8e6>:0)
UnityEngine.UIElements.IMGUIContainer.DoOnGUI (UnityEngine.Event evt, UnityEngine.Matrix4x4 parentTransform, UnityEngine.Rect clippingRect, System.Boolean isComputingLayout, UnityEngine.Rect layoutSize, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.HandleIMGUIEvent (UnityEngine.Event e, UnityEngine.Matrix4x4 worldTransform, UnityEngine.Rect clippingRect, System.Action onGUIHandler, System.Boolean canAffectFocus) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.IMGUIContainer.DoIMGUIRepaint () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIR.RenderChainCommand.ExecuteNonDrawMesh (UnityEngine.UIElements.UIR.DrawParams drawParams, System.Single pixelsPerPoint, System.Exception& immediateException) (at <92b37236153645b0b2627b7fec12ed59>:0)
Rethrow as ImmediateModeException
UnityEngine.UIElements.UIR.RenderChain.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIRRepaintUpdater.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.BaseVisualElementPanel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.Panel.Render () (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEditor.UIElements.EditorPanel.Render () (at <5648a94285474ac58abf4973bc9cc08c>:0)
UnityEngine.UIElements.UIElementsUtility.DoDispatch (UnityEngine.UIElements.BaseVisualElementPanel panel) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIElementsUtility.UnityEngine.UIElements.IUIElementsUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& eventHandled) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.UIElements.UIEventRegistration+<>c.<.cctor>b__1_2 (System.Int32 i, System.IntPtr ptr) (at <92b37236153645b0b2627b7fec12ed59>:0)
UnityEngine.GUIUtility.ProcessEvent (System.Int32 instanceID, System.IntPtr nativeEventPtr, System.Boolean& result) (at <79ead9f12d8f41348c858c7de3eabc8b>:0)

