using UnityEngine;
using JonathansAdventure.Game.Normal;

namespace JonathansAdventure.Game.Multiplayer
{
    /// <summary>
    ///     Assigns keys to the second player in multiplayer.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/18/2022
    /// </remarks>
    public class Player2 : Player
    {
        protected internal override bool InteractKey => Input.GetKeyDown(KeyCode.RightShift);

        private protected override float HorizontalAxis => Input.GetAxis(nameof(Axis.ArrowHorizontal));

        private protected override bool JumpKey => Input.GetKeyDown(KeyCode.UpArrow);
    }

}

