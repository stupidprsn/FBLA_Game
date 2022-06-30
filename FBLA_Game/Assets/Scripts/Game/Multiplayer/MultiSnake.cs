using UnityEngine;

namespace JonathansAdventure.Game.Multi
{
    /// <summary>
    ///     Snake for boss levels.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/27/2022
    /// </remarks>
    public class MultiSnake : Snake
    {
        /// <summary>
        ///     The snake spawner for multiplayer mode.
        /// </summary>
        private SnakeSpawner spawner;

        /// <summary>
        ///     Cache reference.
        /// </summary>
        private void Awake()
        {
            spawner = GameObject.FindWithTag("Spawner").GetComponent<SnakeSpawner>();
        }

        /// <summary>
        ///     Disable the object instead of destroying it
        ///     so that it can be recycled by the 
        ///     <see cref="ObjectPooler"/>. Also signal 
        ///     it's death.
        /// </summary>
        protected override void KillSnake()
        {
            spawner.SnakeKilled();
            gameObject.SetActive(false);
        }
    }

}

