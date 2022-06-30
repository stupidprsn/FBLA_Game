using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using JonathansAdventure.Game.Boss;

namespace JonathansAdventure.Game.Multi
{
    /// <summary>
    ///     Manages spawning snakes in multiplayer.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/27/222
    /// </remarks>
    public class SnakeSpawner : MonoBehaviour
    {
        internal List<Transform> SnakeSpawns { get; set; } = new List<Transform>();

        /// <summary>
        ///     The parent to place the snakes under.
        /// </summary>
        [SerializeField,
            Tooltip("The parent to place the snakes under.")]
        private Transform parent;

        /// <summary>
        ///     The parent to place the warnings under.
        /// </summary>
        [SerializeField,
            Tooltip("The parent to place the warnings under.")]
        private Transform warningParent;

        /// <summary>
        ///     The time between the warning showing up and the snakes spawning.
        /// </summary>
        [SerializeField,
            Range(0f, 3f),
            Tooltip("The time between the warning showing up and the snakes spawning.")]
        private float warningTime;

        /// <summary>
        ///     The object pooler.
        /// </summary>
        [SerializeField,
            Tooltip("The object pooler.")]
        private ObjectPooler pooler;

        /// <summary>
        ///     The amount of snakes still alive.
        /// </summary>
        private int snakesLeft;

        /// <summary>
        ///     Signals that a snake has been killed.
        /// </summary>
        public void SnakeKilled()
        {
            snakesLeft--;
        }

        private float spawnTime = 8f;

        /// <summary>
        ///     Spawns snakes.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator SpawnSnakes()
        {
            while(true)
            {
                yield return new WaitForSeconds(spawnTime);
                yield return new WaitUntil(() => snakesLeft < 2);

                List<Transform> spawns = new List<Transform>(SnakeSpawns);

                Vector3 tempPosition = spawns[Random.Range(0, spawns.Count)].position;
                Vector3 location = new Vector3(tempPosition.x, tempPosition.y + 1f, 0);

                GameObject warning = pooler.Spawn(PoolTags.Warning, location, warningParent);

                // Disable the warnings so that they can be reused by the object pooler
                // after the warning time.
                yield return new WaitForSeconds(warningTime);

                warning.SetActive(false);

                pooler.Spawn(PoolTags.Snake, warning.transform.position, parent);
                snakesLeft++;
            }
        }

        /// <summary>
        ///     Start the snake spawner.
        /// </summary>
        private void Start()
        {
            StartCoroutine(SpawnSnakes());
        }
    }

}
