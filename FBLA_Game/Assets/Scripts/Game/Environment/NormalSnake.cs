using UnityEngine;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Snake for normal levels.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/18/2022
    /// </remarks>
    public class NormalSnake : Snake
    {
        /// <summary>
        ///     Since snakes are not reused in normal levels,
        ///     the snake can simply be destroyed.
        /// </summary>
        protected override void KillSnake()
        {
            Destroy(gameObject);
        }
    }

}
