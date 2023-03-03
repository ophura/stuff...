using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDK3.Avatars.Components;
using static VRC.SDK3.Avatars.Components.VRCAvatarDescriptor;


// [InitializeOnLoad]
internal static class GetRenamedYouLittleSilly
{
    private static readonly Dictionary<int, string> previousNamesDict = new Dictionary<int, string>();
    
    private static GameObject[] allGameObjectsInDescriptor;


    //static GetRenamedYouLittleSilly() =>

    //EditorApplication.hierarchyChanged += ExampleName;
    
    
    [MenuItem("Tools/Testst")]
    private static void ExampleName()
    {
        var gameObject = GameObject.Find("leefa");

        var discriptor = gameObject.GetComponent<VRCAvatarDescriptor>();

        foreach (var clip in discriptor.GetAnimationClips())
            Debug.Log(clip.name);
    }
    
    
    private static GameObject GetRenamedGameObject()
    {
        if (allGameObjectsInDescriptor == null)
        {
            allGameObjectsInDescriptor = GetAllGameObjectsInDescriptor();

            for (var i = 0; i < allGameObjectsInDescriptor.Length; i++)
            {
                var gameObject = allGameObjectsInDescriptor[i];

                previousNamesDict[gameObject.GetInstanceID()] = gameObject.name;
            }

            return null;
        }

        for (var i = 0; i < allGameObjectsInDescriptor.Length; i++)
        {
            var gameObject = allGameObjectsInDescriptor[i];

            var instanceID = gameObject.GetInstanceID();

            if (previousNamesDict.ContainsKey(instanceID) is false)
            {
                previousNamesDict[instanceID] = gameObject.name;

                continue; // the loop
            }
            
            var previousName = previousNamesDict[instanceID];

            if (previousName != gameObject.name)
            {
                // Debug.Log($"name changed: {previousName} -> {gameObject.name}");
                Debug.Log($"name changed: {previousName} -> {gameObject.name}");

                previousNamesDict[instanceID] = gameObject.name;

                return gameObject;
            }
        }

        return null;
    }
    
    private static GameObject[] GetAllGameObjectsInDescriptor()
    {
        var roots = SceneManager.GetActiveScene().GetRootGameObjects();

        var gameObjectList = new List<GameObject>();

        for (var i = 0; i < roots.Length; i++)
        {
            var descriptor = roots[i].GetComponent<VRCAvatarDescriptor>();

            if ((object)descriptor is null) continue; // the loop...

            else gameObjectList.AddRange(GetSelfAndDescendants(descriptor.gameObject));
        }

        return gameObjectList.ToArray();
    }
    
    private static GameObject[] GetSelfAndDescendants(GameObject gameObject)
    {
        var transforms = gameObject.GetComponentsInChildren<Transform>(includeInactive: true);

        var gameObjectList = new List<GameObject>(transforms.Length);

        for (var i = 0; i < transforms.Length; i++)
            gameObjectList.Add(transforms[i].gameObject);

        return gameObjectList.ToArray();
    }
    
    private static AnimationClip[] GetAnimationClips(this VRCAvatarDescriptor descriptor)
    {
        var animationClipList = new List<AnimationClip>();
        
        var controllers = descriptor.GetAnimatorControllers();
        
        for (var i = 0; i < controllers.Length; i++)
        {
            if ((object)controllers[i] is null) continue; // the loop

            else if (controllers[i].animationClips is null) continue; // the loop

            else animationClipList.AddRange(controllers[i].animationClips);
        }
        
        return animationClipList.ToArray();
    }
    
    private static AnimatorController[] GetAnimatorControllers(this VRCAvatarDescriptor descriptor)
    {
        var controllers = new List<AnimatorController>(8);
        
        var playableLayers = descriptor.GetPlayableLayers();

        for (var i = 0; i < playableLayers.Length; i++)
        {
            if ((object)playableLayers[i].animatorController is null) continue; // the loop
            
            else controllers.Add(playableLayers[i].animatorController as AnimatorController);
        }

        return controllers.ToArray();
    }
    
    private static CustomAnimLayer[] GetPlayableLayers(this VRCAvatarDescriptor descriptor)
    {
        if (descriptor.MightContainAnimations() is false)
            throw new System.NullReferenceException("zero animation found");

        var playableLayerList = new List<CustomAnimLayer>();

        var baseLayers = descriptor.baseAnimationLayers;

        for (var i = 0; i < baseLayers.Length; i++)
            playableLayerList.Add(baseLayers[i]);

        var specialLayers = descriptor.specialAnimationLayers;

        for (var i = 0; i < specialLayers.Length; i++)
            playableLayerList.Add(specialLayers[i]);

        return playableLayerList.ToArray();
    }
    
    private static bool MightContainAnimations(this VRCAvatarDescriptor descriptor)
    {
        var specialPlayableLayers = descriptor.specialAnimationLayers;
        var basePlayableLayers = descriptor.baseAnimationLayers;
        
        return basePlayableLayers[0].isDefault is false
            || basePlayableLayers[1].isDefault is false
            || basePlayableLayers[2].isDefault is false
            || basePlayableLayers[3].isDefault is false
            || basePlayableLayers[4].isDefault is false
            || specialPlayableLayers[0].isDefault is false
            || specialPlayableLayers[1].isDefault is false
            || specialPlayableLayers[2].isDefault is false;
    }
}
