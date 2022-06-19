using UnityEngine;
using JonathansAdventure.Game.Normal;

namespace JonathansAdventure.Game.Singleplayer
{
    /// <summary>
    ///     Player controller for singleplayer games.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Allows the user to use either WASD or Arrow keys.
    ///     </para>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/18/2022
    ///     </para>
    /// </remarks>
    public class SingleplayerPlayer : Player
    {
        protected internal override bool InteractKey => Input.GetKeyDown(KeyCode.Space);

        private protected override float HorizontalAxis => Input.GetAxis(nameof(Axis.Horizontal));

        private protected override bool JumpKey => Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
    }
}