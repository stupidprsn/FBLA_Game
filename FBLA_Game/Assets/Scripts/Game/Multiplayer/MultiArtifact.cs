using UnityEngine;

namespace JonathansAdventure.Game.Multi
{
    /// <summary>
    ///     Functionality for the artifacts in the multiplayer level.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/27/2022
    /// </remarks>
    public class MultiArtifact : Artifact
    {
        #region References

        [SerializeField] private Transform snake;
        private MultiplayerManager manager;

        #endregion

        /// <summary>
        ///     The artifact does not have a final parent.
        /// </summary>
        protected override Transform FinalParent => transform;

        /// <summary>
        ///     The artifact moves up.
        /// </summary>
        protected override Vector3 EndPos => new Vector3(trans.position.x, 10, 0);

        protected override void Start()
        {
            base.Start();
            manager = MultiplayerManager.Instance;
        }

        /// <summary>
        ///     The artifact returns to the snake
        ///     so that it can be reused by the object pooler.
        /// </summary>
        protected override void AfterCollect()
        {
            trans.localPosition = new Vector3(0, 0, 0);
            manager.ArtifactCollected();
            gameObject.SetActive(false);
        }
    }

}

