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

        /// <summary>
        ///     Set values for the snake's size.
        /// </summary>
        private void Awake()
        {
            right = new Vector2(1.05f, -0.45f);
            downRight = new Vector2(0.9f, -0.7f);
            left = new Vector2(-0.7f, -0.45f);
            downLeft = new Vector2(-0.9f, -0.7f);
            raycast = new Vector2(0.3f, 0.75f);
        }

        
    }

}
