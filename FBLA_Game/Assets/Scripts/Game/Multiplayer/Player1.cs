using UnityEngine;
using JonathansAdventure.Game.Normal;

namespace JonathansAdventure.Game.Multiplayer
{
    /// <summary>
    ///     Assigns keys to the first player in multiplayer.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/18/2022
    /// </remarks>
    public class Player1 : Player
    {
        protected internal override bool InteractKey => Input.GetKeyDown(KeyCode.Space);

        private protected override float HorizontalAxis => Input.GetAxis(nameof(Axis.ADHorizontal));

        private protected override bool JumpKey => Input.GetKeyDown(KeyCode.W);
    }

}

