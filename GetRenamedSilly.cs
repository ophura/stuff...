using UnityEngine.SceneManagement;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


[InitializeOnLoad]
internal static class GetRenamedSilly
{
    static GetRenamedSilly() =>
        
        EditorApplication.hierarchyChanged += RenamePoorForgottenThings;
    
    
    private static readonly Dictionary<int, string> previousNames = new Dictionary<int, string>();
    
    private static Transform[] allTransformsInScene;
    
    
    private static void RenamePoorForgottenThings()
    {
        if (allTransformsInScene == null)
        {
            allTransformsInScene = GetAllTransformsInScene();

            foreach (var trans in allTransformsInScene)
                previousNames[trans.GetInstanceID()] = trans.name;
            
            return;
        }

        foreach (var trans in allTransformsInScene)
        {
            var instanceID = trans.GetInstanceID();
            
            if (!previousNames.ContainsKey(instanceID))
            {
                previousNames[instanceID] = trans.name;

                continue;
            }

            if (previousNames[instanceID] != trans.name)
            {
                Debug.Log($"name changed: {previousNames[instanceID]} -> {trans.name}");

                previousNames[instanceID] = trans.name;
            }
        }
    }
    
    private static Transform[] GetAllTransformsInScene()
    {
        var rootGameObjects = SceneManager.GetActiveScene().GetRootGameObjects();

        var transList = new List<Transform>();

        for (var i = 0; i < rootGameObjects.Length; i++)
            transList.AddRange(rootGameObjects[i].GetComponentsInChildren<Transform>(true));

        return transList.ToArray();
    }
}
