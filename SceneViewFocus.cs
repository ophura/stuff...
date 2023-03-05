#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;


internal sealed class SceneViewFocus : MonoBehaviour
{
    private const string GAMEOBJECT_NAME = nameof(SceneViewFocus);
    
    private static event System.Action OnAwakeEvent;
    
    private static new GameObject gameObject;


    [MenuItem("SceneView/Disable Focus", true)]
    private static bool DisableFocusValidate()
    {
        gameObject = GameObject.Find(GAMEOBJECT_NAME);

        return gameObject;
    }


    [MenuItem("SceneView/Disable Focus")]
    private static void DisableFocus() => DestroyImmediate(gameObject);


    [MenuItem("SceneView/Enable Focus", true)]
    private static bool EnableFocusValidate() => !DisableFocusValidate();


    [MenuItem("SceneView/Enable Focus")]
    private static void EnableFocus() => gameObject = new GameObject(GAMEOBJECT_NAME, typeof(SceneViewFocus))
    {
        hideFlags = HideFlags.HideInHierarchy | HideFlags.HideInInspector | HideFlags.NotEditable
    };


    [InitializeOnLoadMethod]
    private static void RegisterEventHandlerOnStartup() => OnAwakeEvent += OnAwakeEventHandler;
    
    
    private static void OnAwakeEventHandler()
    {
        if (EditorWindow.HasOpenInstances<SceneView>())
        {
            SceneView.lastActiveSceneView.Focus();
        }
        else
        {
            var gameView = typeof(Editor).Assembly.GetType("UnityEditor.GameView");

            _ = EditorWindow.GetWindow<SceneView>(new[] { gameView });
        }
    }
    
    
    private void Awake() => OnAwakeEvent?.Invoke();
}
#endif
