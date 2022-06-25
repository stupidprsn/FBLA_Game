using UnityEngine;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     Snake for boss levels.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/22/2022
    /// </remarks>
    public class PooledSnake : Snake
    {
        /// <summary>
        ///     Disable the object instead of destroying it
        ///     so that it can be recycled by the 
        ///     <see cref="ObjectPooler"/>.
        /// </summary>
        protected override void KillSnake()
        {
            gameObject.SetActive(false);
        }

        /// <summary>
        ///     Set values for the snake's size.
        /// </summary>
        private void Awake()
        {
            right = new Vector2(0.7f, -0.25f);
            downRight = new Vector2(0.45f, -0.5f);
            left = new Vector2(-0.35f, -0.25f);
            downLeft = new Vector2(-0.45f, -0.5f);
            raycast = new Vector2(0.2f, 0.5f);
        }
    }

}

