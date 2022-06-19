using UnityEngine;
using JonathansAdventure.Game.Normal;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     Snake for boss levels.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/18/2022
    /// </remarks>
    public class BossSnake : Snake
    {
        /// <summary>
        ///     Disable the object instead of destroying it
        ///     so that it can be recycled by the 
        ///     <see cref="ObjectPooler"/>.
        /// </summary>
        private protected override void KillSnake()
        {
            gameObject.SetActive(false);
        }

        private void Awake()
        {
            right = new Vector2(0.7f, -0.25f);
            left = new Vector2(-0.35f, -0.25f);
            down = new Vector2(-0.45f, -0.5f);
            raycast = new Vector2(0.2f, 0.5f);
        }
    }

}

