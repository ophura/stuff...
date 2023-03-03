using UnityEditor;
using PlayModeState = UnityEditor.PlayModeStateChange;


internal sealed class SceneViewFocus
{
    [InitializeOnLoadMethod]
    private static void RegisterEventHandler() =>
        
        EditorApplication.playModeStateChanged += PlayModeStateChangedEventHandler;
    
    
    private static void PlayModeStateChangedEventHandler(PlayModeState playModeState)
    {
        if (playModeState == PlayModeState.EnteredPlayMode)
            EditorWindow.FocusWindowIfItsOpen<SceneView>();
    }
    
    
    ~SceneViewFocus() =>
        
        EditorApplication.playModeStateChanged -= PlayModeStateChangedEventHandler;
}
