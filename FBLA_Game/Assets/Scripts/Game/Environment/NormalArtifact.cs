using UnityEngine;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Functionality for artifacts in normal levels.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/21/2022
    /// </remarks>
    public class NormalArtifact : Artifact
    {

        #region Reference

        private ArtifactHolder holder;

        #endregion

        /// <summary>
        ///     The final parent is the artifact holder.
        /// </summary>
        protected override Transform FinalParent => holder.transform;

        /// <summary>
        ///     Get the next end position from the artifact holder.
        /// </summary>
        protected override Vector3 EndPos => holder.GetNextPosition();

        /// <summary>
        ///     Tell the artifact holder that an artifact has been collected.
        /// </summary>
        protected override void AfterCollect()
        {
            holder.ArtifactCollected();
        }

        /// <summary>
        ///     Set a reference to the artifact holder.
        /// </summary>
        protected override void Start()
        {
            base.Start();
            holder = ArtifactHolder.Instance;
        }
    }
}