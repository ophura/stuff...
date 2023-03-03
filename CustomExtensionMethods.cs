namespace User.Defined
{
    using System;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;


    /// <summary>
    ///     A <see langword="static"/> <see langword="class"/> providing extension methods to enhance
    ///     one's experience in Unity GameDev projects.
    /// </summary>
    /// <![CDATA[
    /// using UnityEngine;
    /// using UnityEngine.SceneManagement;
    /// using User.Defined;
    /// 
    /// 
    /// // NOTE: This shoud be attached to a GameObject as a C# file named: ShowCase
    /// internal class ShowCase : MonoBehaviour
    /// {
    ///     [SerializeField]
    ///     private bool entireScene = true;
    /// 
    /// 
    ///     private void Awake()
    ///     {
    ///         if (entireScene) EntireSceneScoped();
    ///     
    ///         else GameObjectScoped();
    ///     }
    /// 
    /// 
    ///     private void EntireSceneScoped()
    ///     {
    ///         foreach (var gameObject in SceneManager.GetActiveScene().GetAllGameObjectsInHierarchy())
    ///             // TODO: Your own code here instead...
    ///             print(gameObject.name);
    ///     }
    /// 
    /// 
    ///     private void GameObjectScoped()
    ///     {
    ///         foreach (var gameObject in gameObject.GetSelfAndDescendants())
    ///             // TODO: Your own code here instead...
    ///             print(gameObject.name);
    ///     }
    /// }]]>
    internal static class CustomExtensionMethods
    {
        /// <summary>
        ///     Retrieves <i>every single</i> <see cref="Transform"/> in a given <see cref="Scene"/>.
        /// </summary>
        /// <param name="self">
        ///     This parameter is the current <see cref="Scene"/> instance.
        /// </param>
        /// <returns>
        ///     A <see cref="Transform"/> <see cref="Array"/> comprehending <i>all</i>
        ///     <see cref="Transform"/>s available in the <see cref="Scene"/> in question.
        /// </returns>
        internal static GameObject[] GetAllGameObjectsInHierarchy(this Scene self)
        {
            var roots = self.GetRootGameObjects();

            var transList = new List<GameObject>();

            for (var i = 0; i < roots.Length; i++)
                transList.AddRange(roots[i].GetSelfAndDescendants());

            return transList.ToArray();
        }


        /// <summary>
        ///     Finds each <see cref="Transform"/> in a given root.
        /// </summary>
        /// <remarks>
        ///     Note: this method does not only get any possible parent, but also all of its children
        ///     <br/>and if a given child found to be a parent itself, the loop goes on...
        ///     <para>
        ///         For further details, check the following link out:
        ///         <br/><see href="https://docs.unity3d.com/ScriptReference/Component.GetComponentsInChildren.html"/>
        ///     </para>
        /// </remarks>
        /// <param name="self">
        ///     The originating <see cref="GameObject"/> source currently using the method.
        /// </param>
        /// <returns>
        ///     An <see cref="Array"/> of <i>any</i> <see cref="Transform"/> on this
        ///     <see cref="GameObject"/>.
        /// </returns>
        internal static GameObject[] GetSelfAndDescendants(this GameObject self)
        {
            var transforms = self.GetComponentsInChildren<Transform>(includeInactive: true);

            var gameObjectList = new List<GameObject>(transforms.Length);

            for (var i = 0; i < transforms.Length; i++)
                gameObjectList.Add(transforms[i].gameObject);

            return gameObjectList.ToArray();
        }
    }
}
