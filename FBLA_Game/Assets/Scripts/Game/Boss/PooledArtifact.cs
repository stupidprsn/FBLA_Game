using UnityEngine;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     Functionality for the artifacts in the boss level.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/23/2022
    /// </remarks>
    public class PooledArtifact : Artifact
    {
        #region References

        [SerializeField] private Transform snake;
        private Transform bossTrans;
        private Boss boss;

        #endregion

        /// <summary>
        ///     The artifact moves to the boss.
        /// </summary>
        protected override Transform FinalParent => bossTrans;

        /// <summary>
        ///     The artifact moves to the center of the boss.
        /// </summary>
        protected override Vector3 EndPos => new Vector3(0, 0, 0);

        /// <summary>
        ///     The artifact damages the boss and then returns to the snake
        ///     so that it can be reused by the object pooler.
        /// </summary>
        protected override void AfterCollect()
        {
            boss.ArtifactCollected();
            trans.SetParent(snake);
            trans.localPosition = new Vector3(0, 0, 0);
            gameObject.SetActive(false);
        }

        /// <summary>
        ///     Set references to the boss.
        /// </summary>
        protected override void Awake()
        {
            base.Awake();
            boss = Boss.Instance;
            bossTrans = boss.transform;
        }
    }

}

