using UnityEditor;
using PlayModeState = UnityEditor.PlayModeStateChange;


internal sealed class SceneViewFocus
{
    [InitializeOnLoadMethod]
    private static void RegisterEventHandlerOnStartup() =>
        
        EditorApplication.playModeStateChanged += PlayModeStateChangedEventHandler;
    
    
    private static void PlayModeStateChangedEventHandler(PlayModeState playModeState)
    {
        if (playModeState == PlayModeState.EnteredPlayMode)
            EditorWindow.FocusWindowIfItsOpen<SceneView>();
    }
    
    
    ~SceneViewFocus() =>
        
        EditorApplication.playModeStateChanged -= PlayModeStateChangedEventHandler;
}
