using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     The singleplayer boss fight.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/27/2022
    ///     </para>
    /// </remarks>
    public class Boss : MonoBehaviour
    {
        /// <summary>
        ///     Singleton Pattern.
        /// </summary>
        internal static Boss Instance { get; private set; }

        #region Settings
        
        /// <summary>
        ///     Settings for the rounds of the boss fight.
        /// </summary>
        [System.Serializable]
        private class Round
        {
            /// <summary>
            ///     The wait time (seconds) before the round starts.
            /// </summary>
            [field: SerializeField,
                Range(0f, 5f),
                Tooltip("The wait time (seconds) before the round starts.")]
            internal float Wait { get; set; }

            /// <summary>
            ///     The number of snakes that spawn that round.
            /// </summary>
            [field: SerializeField,
                Range(1, 10),
                Tooltip("The number of snakes that spawn that round.")]
            internal int SnakeCount { get; set; }

            /// <summary>
            ///     The number of lasers that spawn that round.
            /// </summary>
            [field: SerializeField,
                Range(2, 20),
                Tooltip("The number of lasers that spawn that round.")]
            internal int LaserCount { get; set; }
        }

        /// <summary>
        ///     Set the number of rounds and the number of enemies that spawn each round.
        /// </summary>
        [Header("Settings")]
        [SerializeField,
            Tooltip("Set the number of rounds and the number of enemies that spawn each round.")]
        private Round[] Rounds;

        /// <summary>
        ///     Set the amount of time between the warning symbol and the enemy spawning.
        /// </summary>
        [SerializeField,
            Range(0f, 5f),
            Tooltip("Set the amount of time between the warning symbol and the enemy spawning.")]
        private float warningTime;

        /// <summary>
        ///     Set the amount of time between the last snake dying and the lasers spawning.
        /// </summary>
        [SerializeField,
            Range(0f, 5f),
            Tooltip("Set the amount of time between the last snake dying and the lasers spawning.")]
        private float laserWait;

        #endregion

        #region References

        [Header("References")]
        [SerializeField] private ObjectPooler objectPooler;
        [SerializeField] private Transform snakeParent;
        [SerializeField] private Transform laserParent;
        [SerializeField] private Transform warningParent;
        [SerializeField] private Transform platformTrans;

        [SerializeField] private GameObject door;
        [SerializeField] private BossAnimation bossAnimation;

        #endregion

        #region Variables

        /// <summary>
        ///     The amount of snakes left this round.
        /// </summary>
        private int snakesLeft;

        /// <summary>
        ///     If the warning is finished.
        /// </summary>
        private bool warningFinished;

        /// <summary>
        ///     A list of positions to spawn snake enemies.
        /// </summary>
        private readonly List<Vector3> snakePositions = new List<Vector3>();

        /// <summary>
        ///     A list of indexes for <see cref="snakePositions"/>.
        /// </summary>
        private readonly List<int> positionIndexes = new List<int>();

        #endregion

        /// <summary>
        ///     Shows the boss damanged animation and decrements the number of snakes left.
        /// </summary>
        internal void ArtifactCollected()
        {
            bossAnimation.DamageAnimation();
            snakesLeft--;
        }

        /// <summary>
        ///     Winning the game (when all snake enemies have been killed).
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator OnWin()
        {
            bossAnimation.DeathAnimation();
            yield return new WaitForSeconds(2);
            door.SetActive(true);
        }

        /// <summary>
        ///     Spawns warning symbols.
        /// </summary>
        /// <param name="positions"> The positions to spawn them at. </param>
        /// <returns> null </returns>
        private IEnumerator SpawnWarnings(Vector3[] positions)
        {
            // List of warning symbols.
            List<GameObject> warnings = new List<GameObject>();

            // Spawn warning symbols the warn the player.
            foreach (Vector3 pos in positions)
            {
                warnings.Add(
                    objectPooler.Spawn(PoolTags.Warning, pos, warningParent)
                );
            }

            // Disable the warnings so that they can be reused by the object pooler
            // after the warning time.
            yield return new WaitForSeconds(warningTime);
            foreach (var item in warnings)
            {
                item.SetActive(false);
            }

            // Signal that the warning phase is done.
            warningFinished = true;
        }

        /// <summary>
        ///     Spawns snake and their corresponding warning symbols.
        /// </summary>
        /// <remarks>
        ///     Randomly picks platforms to spawn snakes on. Only one snake spawns per platform. 
        /// </remarks>
        /// <param name="numOfSnakes"> The number of snakes to spawn. </param>
        private IEnumerator SpawnSnakes(int numOfSnakes)
        {
            // Temporary list used to store the indexes of possible platforms to spawn on.
            // Editing a list of ints uses less memory than a list of Vector3. 
            List<int> indexes = new List<int>(positionIndexes);
            // Eliminate random indexes until the number of indexes equals the number of snakes.
            while(indexes.Count != numOfSnakes)
            {
                indexes.RemoveAt(Random.Range(0, indexes.Count));
            }

            // Array of positions to spawn snakes on.
            Vector3[] spawnPositions = new Vector3[numOfSnakes];

            // Fill the array with the random indexes.
            for(int i = 0; i < numOfSnakes; i++)
            {
                spawnPositions[i] = snakePositions[indexes[i]];
            }

            // Spawn warnings and wait until the warning phase is finished.
            warningFinished = false;
            StartCoroutine(SpawnWarnings(spawnPositions));
            yield return new WaitUntil(() => warningFinished);

            // Spawn snakes
            foreach(Vector3 position in spawnPositions)
            {
                objectPooler.Spawn(PoolTags.Snake, position, snakeParent);
            }
        }

        /// <summary>
        ///     Spawns laser beams.
        /// </summary>
        /// <remarks>
        ///     Spawns evenly distributed lasers with one over the boss.
        /// </remarks>
        /// <param name="numOfLasers"> The number of lasers to spawn. </param>
        /// <returns> null </returns>
        private IEnumerator SpawnLasers(int numOfLasers)
        {
            List<float> xCoords = new List<float>(numOfLasers);

            // One laser above the boss.
            xCoords.Add(0f);
            int lasersLeft = numOfLasers - 1;
            
            int lasersOnOneSide = lasersLeft / 2; // Divide by two to evenly distribute between both sides.
            int divisor = lasersOnOneSide + 1; // Add 1 so that the lasers aren't on the edges of the screen.
            float inc = 12f / divisor; // 12 is the maximum x value of the screen.

            // Fill x coordinates.
            for (int i = 1; i <= lasersOnOneSide; i++)
            {
                xCoords.Add(i * inc);
                xCoords.Add(-1 * i * inc);
            }

            // Fill a list of warning symbol positions
            Vector3[] warningPositions = new Vector3[numOfLasers];
            for(int i = 0; i < numOfLasers; i++)
            {
                // 3.5 is close to the ceiling of the stage.
                warningPositions[i] = new Vector3(xCoords[i], 4.5f, 0f);
            }

            // Spawn warnings and wait for them to finish.
            warningFinished = false;
            StartCoroutine(SpawnWarnings(warningPositions));
            yield return new WaitUntil(() => warningFinished);

            // Spawn lasers.
            foreach(float x in xCoords)
            {
                objectPooler.Spawn(PoolTags.Laser, new Vector3(x, 5f, 0f), laserParent);
            }
        }

        /// <summary>
        ///     Manages sending out the rounds of enemies.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator RoundSender()
        {
            foreach(Round round in Rounds)
            {
                // Send out the snakes and wait till all of the artifacts are collected.
                yield return new WaitForSeconds(round.Wait);
                StartCoroutine(SpawnSnakes(round.SnakeCount));
                snakesLeft = round.SnakeCount;
                yield return new WaitUntil(() => snakesLeft == 0);

                // Send out the lasers.
                yield return new WaitForSeconds(laserWait);
                StartCoroutine(SpawnLasers(round.LaserCount));
                yield return new WaitForSeconds(warningTime + 2);
            }

            StartCoroutine(OnWin());
        }

        /// <summary>
        ///     Partial singleton check.
        /// </summary>
        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        ///     Fill the list of platforms.
        /// </summary>
        private void Start()
        {
            // Find all platforms where the snake enemies can be spawned
            // Find every game object under the Platforms folder
            List<Transform> tempPositions = new List<Transform>(platformTrans.GetComponentsInChildren<Transform>());

            // For each gameobject
            foreach(Transform temp in tempPositions)
            {
                // Make sure it's the platform
                if(temp.CompareTag("Platform"))
                {
                    // Add it's position to the list of positions.
                    Vector3 pos = temp.position;
                    snakePositions.Add(
                        new Vector3(pos.x, pos.y + 0.75f, 0) // Add 0.75f to y so that the snake spawns above, and not in, the platform.
                    );
                }
            }

            // Fill the list of indexes.
            for (int i = 0; i < snakePositions.Count; i++)
            {
                positionIndexes.Add(i);
            }

            StartCoroutine(RoundSender());
        }

        /// <summary>
        ///     Make sure laser count is an odd number.
        /// </summary>
        private void OnValidate()
        {
            foreach(Round round in Rounds)
            {
                if(round.LaserCount % 2 == 0)
                {
                    Debug.LogError("Laser count should be an odd number");
                }
            }
        }

    }

}

