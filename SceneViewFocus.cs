namespace ophura.jp
{
    using UnityEditor;
    
    
    // a not recommended way to keep focus on the the so-called: SceneView.
    // NOTE: this is a UnityEditor script.
    internal sealed class SceneViewFocus : EditorWindow
    {
        [InitializeOnEnterPlayMode]
        private static void _() => EditorApplication.delayCall += () =>
        FocusWindowIfItsOpen<SceneView>();
    }
}
