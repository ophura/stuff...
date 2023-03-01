namespace Uniquely.Yours
{
    using Descriptor = VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;
    using VRC.SDK3.Dynamics.PhysBone.Components;
    using UnityEngine.SceneManagement;
    using UnityEngine;
    using UnityEditor;
    using System.IO;
    
    
    internal sealed class UniquelyYours : EditorWindow
    {
        [MenuItem("Rapture/uniquely Rapture's")]
        private static void FocusOrDisplayWindow()
        {
            const float WIDTH = 385F;
            const float HEIGHT = 512F;
            
            var x = (Screen.width - WIDTH) / 2F;
            var y = (Screen.height - HEIGHT) / 2F;
            
            var pos = new Rect(x, y, WIDTH, HEIGHT);
            
            const string TITLE = "uniquely, yours!";
            
            _ = GetWindowWithRect<UniquelyYours>(pos, utility: true, TITLE, focus: true);
        }
        
        
        private void Awake()
        {
            TryFindAvatar();
            
            const string TEXTURE_PATH = "Assets/editor/resource/rappy.png";

            if (File.Exists(TEXTURE_PATH) is false) return;
            
            else texture = AssetDatabase.LoadAssetAtPath<Texture>(TEXTURE_PATH);
        }
        
        private void OnGUI()
        {
            if (texture != null)
            {
                var pos = new Rect(x: 0F, y: 0F, width: 385F, height: 385F);
                
                EditorGUI.DrawPreviewTexture(pos, texture);

                GUILayout.Space(pixels: 395F);
            }
            
            var style = new GUIStyle(GUI.skin.button)
            {
                richText = true,
                fontSize = 15
            };

            var label = new GUIContent(
                "subject Avatar",
                "it means your main Avatar you slilly xD"
            );
            
            descriptor = (Descriptor)EditorGUILayout.ObjectField(
                label,
                obj: descriptor,
                objType: typeof(Descriptor),
                allowSceneObjects: true,
                options: GUILayout.Height(50F)
            );
            
            GUILayout.Space(pixels: 10F);
            
            GUI.enabled = descriptor != null;

            if (GUILayout.Button("add the funny!", style, GUILayout.Height(50F)))
            {
                AddTheFunnyPhysBone();
            }

            GUI.enabled = true;
        }
        
        
        private void TryFindAvatar()
        {
            var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

            foreach (var gO in rootGameObjects)
            {
                descriptor = gO.GetComponentInChildren<Descriptor>(includeInactive: true);

                if (descriptor != null) break;

                else continue; // the loop
            }

            try
            {
                descriptor.gameObject.SetActive(value: true);
                
                EditorGUIUtility.PingObject(descriptor);
            }
            catch (System.NullReferenceException)
            {
                _ = EditorUtility.DisplayDialog(
                    title: "Rapture's",
                    message: $@"hey {System.Environment.UserName}! xD
this `code` failed to find your Avatar...
consider to drag-and-drop it manually instead after clicking `ok`!",
                    "ok"
                );
            }
        }
        
        private void AddTheFunnyPhysBone()
        {
            var animator = descriptor.GetComponent<Animator>();

            if (animator.avatar.isHuman is false) return;

            var eye = animator.GetBoneTransform(HumanBodyBones.RightEye);
            var head = animator.GetBoneTransform(HumanBodyBones.Head);
            var neck = animator.GetBoneTransform(HumanBodyBones.Neck);
            
            var physBone = neck.gameObject.GetOrAddComponent<VRCPhysBone>();
            
            physBone.rootTransform = neck;
            
            if (physBone.ignoreTransforms.Contains(eye))
            {
                var message = System.Environment.UserName.ToLower();
                message = $"but {message}...\nyou already did that there!";

                ShowNotification(new GUIContent(message), 1.5F);
                
                EditorApplication.Beep();
                
                PingAndSelect(neck);

                return;
            }
            
            AddIgnoreTransforms(neck, physBone);
            AddIgnoreTransforms(head, physBone);
            
            PingAndSelect(neck);
        }
        
        private void AddIgnoreTransforms(Transform bone, VRCPhysBone pBone)
        {
            for (var i = 0; i < bone.childCount; i++)
            {
                var child = bone.GetChild(i);
                
                var name = child.name.ToLower();

                if (name.Contains("ear") || name.Contains("head")) continue;

                pBone.ignoreTransforms.Add(child);
            }
        }

        private void PingAndSelect(Object obj)
        {
            EditorGUIUtility.PingObject(obj);

            Selection.activeObject = obj;
        }
        
        
        private Descriptor descriptor;
        
        private Texture texture;
    }
}
