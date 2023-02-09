namespace ophura.jp
{
    using UnityEditor;
    
    
    internal sealed class SceneViewFocus : EditorWindow
    {
        [InitializeOnEnterPlayMode]
        private static void _() => EditorApplication.delayCall += () =>
        FocusWindowIfItsOpen<SceneView>();
    }
}
