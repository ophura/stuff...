namespace ophura.jp
{
    using UnityEditor;
    using UnityEngine;


    internal class AnimationClipModifier : EditorWindow
    {
        [MenuItem("Tools/AnimationClip Modifier")]
        private static void FocusOrDisplayWindow() =>
            _ = GetWindow<AnimationClipModifier>(false, "AnimationClip Modifier", true);


        private void OnGUI()
        {
            loopTime = EditorGUILayout.Toggle("Loop Time", loopTime);

            if (GUILayout.Button("Set Loop Time"))
            {
                SetAnimationClipLoopTime(loopTime);
            }
        }

        
        // https://forum.unity.com/threads/how-to-enable-looptime-property-of-animationclip-via-code.832633/
        private void SetAnimationClipLoopTime(bool value)
        {
            string[] clipGUIDs = AssetDatabase.FindAssets(
                $"t:{nameof(AnimationClip)}", new[] { "Assets" }
                );

            foreach (string clipGUID in clipGUIDs)
            {
                string clipPath = AssetDatabase.GUIDToAssetPath(clipGUID);

                AnimationClip clip = AssetDatabase.LoadAssetAtPath<AnimationClip>(clipPath);

                AnimationClipSettings clipSettings = AnimationUtility.GetAnimationClipSettings(
                    clip
                    );

                clipSettings.loopTime = value;

                AnimationUtility.SetAnimationClipSettings(clip, clipSettings);
            }

            AssetDatabase.SaveAssets();
        }


        private bool loopTime;
    }
}
