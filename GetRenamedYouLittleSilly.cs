using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.SceneManagement;
using VRC.SDK3.Avatars.Components;


// [InitializeOnLoad]
internal static class GetRenamedYouLittleSilly
{
    [MenuItem("Tools/Test")]
    private static void RunTest()
    {
        foreach (var obj in GetAllGameObjectsInDescriptor())
        {
            Debug.Log(obj.name);
        }
    }
    
    
    //static GetRenamedSilly() =>
        
    //    EditorApplication.hierarchyChanged += RenamePoorForgottenThing;
    
    
    private static readonly Dictionary<int, string> previousNamesDict = new Dictionary<int, string>();
    
    private static GameObject[] allGameObjectsInDescriptor;
    
    
    private static void XXX()
    {
        //var animator = 
    }
    
    private static GameObject RenamePoorForgottenThing()
    {
        if (allGameObjectsInDescriptor == null)
        {
            allGameObjectsInDescriptor = GetAllGameObjectsInDescriptor();
            
            foreach (var trans in allGameObjectsInDescriptor)
                previousNamesDict[trans.GetInstanceID()] = trans.name;
            
            return null;
        }
        
        foreach (var gameObject in allGameObjectsInDescriptor)
        {
            var objectInstanceID = gameObject.GetInstanceID();
            
            if (!previousNamesDict.ContainsKey(objectInstanceID))
            {
                previousNamesDict[objectInstanceID] = gameObject.name;

                continue;
            }

            if (previousNamesDict[objectInstanceID] != gameObject.name)
            {
                Debug.Log($"name changed: {previousNamesDict[objectInstanceID]} -> {gameObject.name}");

                previousNamesDict[objectInstanceID] = gameObject.name;

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
}
