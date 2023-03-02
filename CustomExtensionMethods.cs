namespace User.Defined
{
    using UnityEngine.SceneManagement;
    using System.Collections.Generic;
    using UnityEngine;
    using System;
    
    
    /// <summary>
    ///     A <see langword="static"/> <see langword="class"/> to enhance one's life
    ///     in Unity GameDev projects...
    /// </summary>
    internal static class CustomExtensionMethods
    {
        /// <summary>
        ///     Retrieves <i>every single</i> <see cref="Transform"/> in a given <see cref="Scene"/>.
        /// </summary>
        /// <param name="targetScene">
        ///     This parameter is the current <see cref="Scene"/> instance.
        /// </param>
        /// <returns>
        ///     A <see cref="Transform"/> <see cref="Array"/> comprehending <i>all</i>
        ///     <see cref="Transform"/>s available in the <see cref="Scene"/> in question.
        /// </returns>
        internal static Transform[] GetAllTransformsInScene(this Scene targetScene)
        {
            var roots = targetScene.GetRootGameObjects();

            var transList = new List<Transform>();

            for (var i = 0; i < roots.Length; i++)
                transList.AddRange(roots[i].GetParentAndAllOfItsChildren());

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
        /// <param name="obj">
        ///     The originating <see cref="GameObject"/> source currently using the method.
        /// </param>
        /// <returns>
        ///     An <see cref="Array"/> of <i>any</i> <see cref="Transform"/> on this
        ///     <see cref="GameObject"/>.
        /// </returns>
        internal static Transform[] GetParentAndAllOfItsChildren(this GameObject obj) =>

            obj.GetComponentsInChildren<Transform>(includeInactive: true);
    }
}
