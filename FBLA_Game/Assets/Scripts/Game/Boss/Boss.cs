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
    ///         Last Modified: 6/23/2022
    ///     </para>
    ///     <para>
    ///         <see cref="Flip"/> was inspired by
    ///         "Unity Tutorial Quick Tip: The BEST way to flip your character sprite in Unity"
    ///         by "Nick Hwang" 2020.
    ///         <seealso cref="https://www.youtube.com/watch?v=ccxXxvlS4mI"/>
    ///     </para>
    /// </remarks>
    public class Boss : MonoBehaviour, IDestroyable
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

        [Header("Settings")]
        [SerializeField,
            Tooltip("Set the number of rounds and the number of snakes that spawn each round.")]
        private Round[] spawnCount;

        [SerializeField,
            Range(0f, 5f),
            Tooltip("Set the amount of time between the warning symbol and the enemy spawning.")]
        private float warningTime;        
        
        [SerializeField,
            Range(0f, 5f),
            Tooltip("Set the amount of time between the last snake dying and the lasers spawning.")]
        private float laserWait;

        [SerializeField,
            Tooltip("How much the boss should be nudged once it is killed.")]
        private Vector2 deathNudge;

        [SerializeField,
            Tooltip("Is the player sprite currently facing right?")]
        private bool facingRight;

        #endregion

        #region References

        [Header("References")]
        [SerializeField] private ObjectPooler objectPooler;
        [SerializeField] private Transform snakeParent;
        [SerializeField] private Transform laserParent;
        [SerializeField] private Transform platformTrans;

        [SerializeField] private GameObject door;

        [SerializeField] private Rigidbody2D bossRB;
        [SerializeField] private Animator bossAnimator;

        [SerializeField] private Transform playerTrans;

        #endregion

        #region Variables

        /// <summary>
        ///     The amount of snakes left this round.
        /// </summary>
        private int snakesLeft;

        /// <summary>
        ///     A list of positions to spawn snake enemies.
        /// </summary>
        private readonly List<Vector3> snakePositions = new List<Vector3>();

        /// <summary>
        ///     A list of indexes for <see cref="snakePositions"/>.
        /// </summary>
        private readonly List<int> positionIndexes = new List<int>();

        /// <summary>
        ///     List of x coordinates to spawn lasers.
        /// </summary>
        /// <remarks>
        ///     This list is used in order to not have a heap of 
        ///     lasers spawn near the same position.
        /// </remarks>
        private readonly List<int> laserPositions = new List<int>
        {
            -11, -10, -9, -8, -7, -6, -5, -4, -3, -2,
            2, 3, 4, 5, 6, 7, 8, 9, 10, 11
        };

        /// <summary>
        ///     The ID for the trigger that plays the boss's damanged animation.
        /// </summary>
        private int damageAnimation;

        #endregion

        /// <summary>
        ///     Shows the boss damanged animation and decrements the number of snakes left.
        /// </summary>
        internal void ArtifactCollected()
        {
            bossAnimator.SetTrigger(damageAnimation);
            snakesLeft--;
        }

        /// <summary>
        ///     Call when the boss takes damage.
        /// </summary>
        public void OnDamage()
        {
            bossAnimator.SetTrigger(damageAnimation);
        }

        /// <summary>
        ///     Winning the game (when all snake enemies have been killed).
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator OnWin()
        {
            // Allow the boss to move
            bossRB.constraints = RigidbodyConstraints2D.None;

            // Give the boss a slight nudge
            bossRB.AddForce(new Vector2(deathNudge.x * Time.deltaTime, deathNudge.y * Time.deltaTime), ForceMode2D.Impulse);

            yield return new WaitForSeconds(2);
            door.SetActive(true);
        }

        /// <summary>
        ///     Creates warning symbols and then replaces them with the prefab to spawn.
        /// </summary>
        /// <param name="positions"> The positions to spawn in. </param>
        /// <param name="spawn"> The object to spawn. </param>
        /// <param name="parent"> The parent to place the spawned objects in. </param>
        /// <param name="delay"> The delay before starting. </param>
        /// <returns></returns>
        private IEnumerator Spawn(Vector3[] positions, PoolTags spawn, Transform parent, float delay)
        {
            yield return new WaitForSeconds(delay);

            // List of warning symbols.
            List<GameObject> warnings = new List<GameObject> ();

            // Spawn warning symbols the warn the player.
            foreach(Vector3 pos in positions)
            {
                warnings.Add(
                    objectPooler.Spawn(PoolTags.Warning, pos, parent)
                );
            }

            // Disable the warnings so that they can be reused by the object pooler.
            foreach (var item in warnings)
            {
                item.SetActive(false);
            }

            yield return new WaitForSeconds(warningTime);
            
            // Spawn the object.
            foreach(Vector3 pos in positions)
            {
                print(spawn.ToString() + pos.ToString() + parent.ToString());
                objectPooler.Spawn(spawn, pos, parent);
            }

        }

        /// <summary>
        ///     Spawns snake enemies.
        /// </summary>
        /// <remarks>
        ///     Randomly picks platforms to spawn snakes on. Only one snake spawns per platform. 
        /// </remarks>
        /// <param name="secondsToWait"> The amount of time to wait before spawning the snakes. </param>
        /// <param name="numOfSnakes"> The number of snakes to spawn. </param>
        private void SpawnSnakes(float secondsToWait, int numOfSnakes)
        {
            // Temporary list used to store the indexes of possible platforms to spawn on.
            // Editing a list of ints uses less memory than a list of Vector3. 
            List<int> tempIndexes = new List<int>(positionIndexes);

            // Eliminate random indexes until the number of indexes equals the number of snakes.
            while(tempIndexes.Count != numOfSnakes)
            {
                tempIndexes.RemoveAt(Random.Range(0, tempIndexes.Count - 1));
            }

            // Array of positions to spawn snakes on.
            Vector3[] spawnPositions = new Vector3[numOfSnakes];

            // Fill the array.
            for(int i = 0; i < numOfSnakes; i++)
            {
                spawnPositions[i] = snakePositions[tempIndexes[i]];
                print(spawnPositions[i]);
            }

            StartCoroutine(Spawn(spawnPositions, PoolTags.Snake, snakeParent, secondsToWait));
        }

        /// <summary>
        ///     Spawns laser beams.
        /// </summary>
        /// <remarks>
        ///     There are two special lasers, one that spawns above the player, and one above the boss.
        /// </remarks>
        /// <param name="secondsToWait"> The amount of time to wait before spawning the lasers. </param>
        /// <param name="numOfLasers"> The number of lasers to spawn. </param>
        private void SpawnLasers(int numOfLasers)
        {
            List<int> temp = new List<int>(laserPositions);

            Vector3[] spawnPositions = new Vector3[numOfLasers];


            // Special case 1: laser above player.
            spawnPositions[0] = new Vector3(platformTrans.position.x, 0, 0);

            // Special case 2: laser above boss.
            // The boss' sprite is slightly more to the right.
            spawnPositions[1] = new Vector3(Random.Range(-0.9f, 1f), 0, 0);

            // Fill up the remaining positions.
            // Start at 2 in order to not override the special cases.
            for(int i = 2; i < numOfLasers; i++)
            {
                int index = Random.Range(0, temp.Count - 1);
                spawnPositions[i] = new Vector3(
                    temp[index] + Random.Range(-0.6f, 0.6f),
                    0, 0
                    );
                temp.RemoveAt(index);
            }

            StartCoroutine(Spawn(spawnPositions, PoolTags.Laser, laserParent, laserWait));
        }

        private IEnumerator RoundSender()
        {
            print("Start");
            foreach(Round round in spawnCount)
            {
                print(round.SnakeCount);
                SpawnSnakes(round.Wait, round.SnakeCount);
                snakesLeft = round.SnakeCount;
                yield return new WaitUntil(() => snakesLeft == 0);
                SpawnLasers(round.LaserCount);
            }

            StartCoroutine(OnWin());
        }

        /// <summary>
        ///     Set variables.
        /// </summary>
        private void Awake()
        {
            // Find the ID for the damage animation.
            damageAnimation = Animator.StringToHash("Damage");

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
                // Makesure it's the platform
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

            foreach(Vector3 temp in snakePositions)
            {
                print(temp);
            }

            StartCoroutine(RoundSender());
        }

        /// <summary>
        ///     Method for flipping the direction the boss is facing to match the player.
        /// </summary>
        private void Update()
        {
            // Cache x coordinate.
            float x = playerTrans.position.x;

            // If the user is to the left and the boss is facing right,
            // or if the user is to the right and the boss is facing left...
            if ((x < 0 && facingRight) || (x > 0 && !facingRight))
            {
                transform.Rotate(new Vector3(0, 180, 0));
                facingRight = !facingRight;
            }
        }
    }

}

