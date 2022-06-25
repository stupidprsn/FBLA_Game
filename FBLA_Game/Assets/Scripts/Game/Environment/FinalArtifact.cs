using UnityEngine;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Functionality for the final artifact taht wins the game.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/23/2022
    /// </remarks>
    public class FinalArtifact : Artifact
    {
        /// <summary>
        ///     The final parent doesn't matter, so keep the same parent.
        /// </summary>
        protected override Transform FinalParent => trans.parent;

        /// <summary>
        ///     The end position is slightly above the center of the screen.
        /// </summary>
        protected override Vector3 EndPos => new Vector3(0, 3, 0);

        /// <summary>
        ///     Signal to the <see cref="GameManager"/> that the player has won.
        /// </summary>
        protected override void AfterCollect()
        {
            GameManager.Instance.WinGame();
        }

        /// <summary>
        ///     Claer awake as the final artifact's texture does not need
        ///     to be randomized.
        /// </summary>
        protected override void Awake() {}
    }

}

