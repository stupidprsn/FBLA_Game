using UnityEngine;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     Snake for boss levels.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/27/2022
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
    }

}

