using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace JonathansAdventure.Game.Multi
{
    /// <summary>
    ///     Manages enabling and disabling the platforms (levels).
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/26/2022
    /// </remarks>
    public class LevelManager : MonoBehaviour
    {
        #region Settings

        [Header("Settings")]

        /// <summary>
        ///     The y value where levels become active.
        /// </summary>
        [SerializeField,
            Range(-10f, 10f),
            Tooltip("The y value where levels become active.")]
        private float maxPos;

        /// <summary>
        ///     The lowest position the platform goes before being marked unusable.
        /// </summary>
        [SerializeField,
            Range(-10f, 10f),
            Tooltip("The lowest position the platform goes before being marked unactive.")]
        private float minPos;

        #endregion

        [Header("Reference")]

        [SerializeField] private SnakeSpawner spawner;
        [SerializeField] private MultiplayerManager manager;

        #region Variables

        /// <summary>
        ///     Manages references to components of the level.
        /// </summary>
        private class LevelObject
        {
            /// <summary>
            ///     The gameobject.
            /// </summary>
            internal GameObject GameObject { get; private set; }

            /// <summary>
            ///     The transform.
            /// </summary>
            internal Transform Transform { get; private set; }

            /// <summary>
            ///     The index in <see cref="activeLevels"/>.
            /// </summary>
            internal int Index { get; set; }

            /// <summary>
            ///     The main constructor which caches references by the
            ///     object's transform.
            /// </summary>
            /// <param name="transform"> The transform of the level. </param>
            internal LevelObject(Transform transform)
            {
                GameObject = transform.gameObject;
                Transform = transform;
            }
        }

        /// <summary>
        ///     All levels in the stage.
        /// </summary>
        private List<LevelObject> levels = new List<LevelObject>();

        /// <summary>
        ///     The active levels (i.e. in the viewport).
        /// </summary>
        private readonly List<LevelObject> activeLevels = new List<LevelObject>();

        /// <summary>
        ///     The level to be enabled.
        /// </summary>
        private LevelObject nextLevel;

        /// <summary>
        ///     Checks when the next level becomes active.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator CheckNextLevel()
        {
            while (true)
            {
                // Wait until it's position is within threshhold.
                if (nextLevel.Transform.position.y < maxPos)
                {
                    // Add the next level to active levels.
                    activeLevels.Add(nextLevel);

                    // Reset the next level.
                    nextLevel = levels[nextLevel.Index + 1];

                    // Recalculate the positions.
                    CalcSpawns();
                }

                yield return null;
            }
        }

        /// <summary>
        ///     The level to be disabled.
        /// </summary>
        private LevelObject previousLevel;

        /// <summary>
        ///     Checks when the previous level becomes inactive.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator CheckPreviousLevel()
        {
            while (true)
            {
                // Cache position
                Vector3 pos = previousLevel.Transform.position;

                // Wait until it's position is within threshhold.
                yield return new WaitUntil(() => pos.y < minPos);

                // Recycle the level by moving it to the top.
                // Each level is 2.7 higher. 
                previousLevel.Transform.position = new Vector3(0, pos.y + (levels.Count * 2.7f), 0);

                // Add the previous level to active levels.
                activeLevels.Remove(previousLevel);

                // Wait until the level is no longer visible.
                yield return new WaitUntil(() => pos.y < 7.5f);

                // Reset the previous level.
                previousLevel = levels[previousLevel.Index + 1];

                // Recalculate the positions.
                CalcSpawns();

            }
        }

        /// <summary>
        ///     Finds and caches all active locations where snakes
        ///     can be spawned and where the player can respawn.
        /// </summary>
        private void CalcSpawns()
        {
            spawner.SnakeSpawns = new List<Transform>();
            manager.respawnPositions = new List<Transform>();
            // For each active level.
            foreach (LevelObject level in activeLevels)
            {
                // For all objects in that level.
                foreach (Transform t in level.Transform.GetComponentsInChildren<Transform>())
                {
                    // Filter for snake spawning locations.
                    if (t.CompareTag("SpawnSnake"))
                    {
                        spawner.SnakeSpawns.Add(t);
                    }

                    if (t.CompareTag("Box"))
                    {
                        manager.respawnPositions.Add(t);
                    }

                }
            }
        }

        #endregion

        /// <summary>
        ///     Find and cache the <see cref="LevelObject"/>s.
        /// </summary>
        private void Awake()
        {
            // Find all children of the platform.
            foreach (Transform t in transform.GetComponentsInChildren<Transform>())
            {
                // Sort for levels.
                if (!t.CompareTag("Level")) continue;
                // Append to level list.
                levels.Add(new LevelObject(t));
            }

            // Order the levels from lowest y coordinate to highest y coordinate.
            levels = levels.OrderBy(x => x.Transform.position.y).ToList();

            // Used to determine the next level.
            int highestIndex = -1;

            // Find the active levels.
            for (int i = 0; i < levels.Count; i++)
            {
                // Set index.
                levels[i].Index = i;

                // Cache y coordinate.
                float y = levels[i].Transform.position.y;

                // Check that the level is in the viewport.
                if (y > minPos && y < maxPos)
                {
                    activeLevels.Add(levels[i]);

                    // Update index
                    if (i > highestIndex) highestIndex = i;
                }
            }

            // Lowest y coordinate is the level that's going to be disabled first.
            previousLevel = levels[0];

            // Next level is one index higher than the highest active level.
            nextLevel = levels[highestIndex + 1];

            CalcSpawns();
        }

        /// <summary>
        ///     Start the coroutines for refreshing active panels and spawning snakes.
        /// </summary>
        private void Start()
        {
            StartCoroutine(CheckPreviousLevel());
            StartCoroutine(CheckNextLevel());
        }
    }
}
