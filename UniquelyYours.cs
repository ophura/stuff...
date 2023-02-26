namespace Uniquely.Yours
{
    using AvatarDescriptor = VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;
    using VRC.SDK3.Dynamics.PhysBone.Components;
    using UnityEngine.SceneManagement;
    using UnityEngine;
    using UnityEditor;
    
    
    internal class UniquelyYours : EditorWindow
    {
        [MenuItem("Rapture/uniquely Rapture's")]
        private static void FocusOrDisplayWindow()
        {
            var pos = new Rect((Screen.width - 385F) / 2F,
                (Screen.height - 512F) / 2F, 385F, 512F
                );
            
            _ = GetWindowWithRect<UniquelyYours>(
                pos, true, "uniquely, yours!", true
                );
        }
        
        
        private AvatarDescriptor descriptor;
        
        private Texture texture;
        
        
        private void OnEnable()
        {
            TryFindAvatar();

            if (texture == null)
            {
                texture = AssetDatabase.LoadAssetAtPath<Texture>(
                    "Assets/editor/resource/rappy.png"
                    );
            }
        }
        
        private void OnGUI()
        {
            if (texture != null)
            {
                EditorGUI.DrawPreviewTexture(
                    new Rect(0F, 0F, 385F, 385F), texture
                    );
            }
            
            GUILayout.Space(395F);

            var style = new GUIStyle(GUI.skin.button)
            {
                richText = true,
                fontSize = 15
            };

            descriptor = (AvatarDescriptor)EditorGUILayout.ObjectField(
                new GUIContent("subject Avatar",
                "it means your main Avatar you slilly xD"),
                descriptor,
                typeof(AvatarDescriptor),
                true,
                GUILayout.Height(50F)
            );
            
            GUILayout.Space(10F);
            
            GUI.enabled = descriptor != null;

            if (GUILayout.Button("add the funny!", style, GUILayout.Height(50F)))
            {
                PerformActionOnAvi();
            }
            
            GUI.enabled = true;
        }
        
        
        private void TryFindAvatar()
        {
            if (descriptor != null) return;

            var rootObjs = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var obj in rootObjs)
            {
                descriptor = obj.GetComponentInChildren<AvatarDescriptor>(true);

                if (descriptor != null) break;
            }

            try
            {
                descriptor.gameObject.SetActive(true);

                EditorGUIUtility.PingObject(descriptor);
            }
            catch (System.NullReferenceException)
            {
                InformRappy();
            }
        }
        
        private void InformRappy()
        {
            var message = $"hey {System.Environment.UserName}! xD\nthis `code` ";
            message += $"failed to find your Avatar, consider to drag-and-drop ";
            message += "it manually instead after clicking `ok`!";

            _ = EditorUtility.DisplayDialog("Rapture's", message, "ok");
        }
        
        private void PerformActionOnAvi()
        {
            var animator = descriptor.GetComponent<Animator>();

            if (!animator.avatar.isHuman) return;

            var neck = animator.GetBoneTransform(HumanBodyBones.Neck);

            var physBone = neck.gameObject.GetOrAddComponent<VRCPhysBone>();

            if (physBone.ignoreTransforms.Count > 0)
            {
                var noti = System.Environment.UserName.ToLower();
                noti = $"but {noti}...\nyou already did that there!";

                ShowNotification(new GUIContent(noti), 1.5F);

                EditorGUIUtility.PingObject(neck);

                return;
            }

            AddIgnoreTrans(neck, physBone);

            var head = animator.GetBoneTransform(HumanBodyBones.Head);
            
            AddIgnoreTrans(head, physBone);
            
            physBone.rootTransform = neck;

            EditorGUIUtility.PingObject(neck);
        }

        private void AddIgnoreTrans(Transform bone, VRCPhysBone pBone)
        {
            for (var i = 0; i < bone.childCount; i++)
            {
                var child = bone.GetChild(i);
                
                var name = child.name.ToLower();

                if (name.Contains("ear") || name.Contains("head")) continue;

                pBone.ignoreTransforms.Add(child);
            }
        }
    }
}
