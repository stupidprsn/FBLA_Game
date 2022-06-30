using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Manages multiplayer gameplay.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/27/2022
    /// </remarks>
    public class MultiplayerManager : MonoBehaviour
    {
        /// <summary>
        ///     A reference to the <see cref="MultiplayerManager"/>.
        /// </summary>
        /// <remarks>
        ///     Partial singleton pattern.
        /// </remarks>
        public static MultiplayerManager Instance;

        #region Variables
        [HideInInspector] public List<Transform> respawnPositions = new List<Transform>();

        [SerializeField] private PlayerDeath[] playerDeaths;

        [SerializeField,
            Range(1, 10),
            Tooltip("Set the number of lives that the user has.")]
        private int playerHealth;

        private int health;

        /// <summary>
        ///     The lives the player has left.
        /// </summary>
        public int Health
        {
            get => health;
            set
            {
                health = value;
                GameUI.UpdateHealthDisplay(health);
            }

        }

        [field: HideInInspector] public MultiUI GameUI { get; private set; }

        /// <summary>
        ///     The number of snakes the player has killed.
        /// </summary>
        private int artifactsCollected;

        /// <summary>
        ///     Increment the number of artifacts collected.
        /// </summary>
        public void ArtifactCollected()
        {
            artifactsCollected++;
        }

        private SoundManager soundManager;

        #endregion

        /// <summary>
        ///     Respawns a player.
        /// </summary>
        public void Respawn(Transform player)
        {
            // Cache position
            Vector3 respawnPosition = respawnPositions[Random.Range(0, respawnPositions.Count - 1)].position;
            player.position = new Vector3(
                respawnPosition.x,
                respawnPosition.y + 1.2f,
                0
            );
        }

        /// <summary>
        ///     Displays the leaderboard
        /// </summary>
        /// <param name="soundName"> The sound effect to play. </param>
        /// <param name="win"> If the user won. </param>
        public void DisplayScore()
        {
            foreach (PlayerDeath playerDeath in playerDeaths)
            {
                playerDeath.DisableMovement();
            }

            soundManager.StopAllSound();
            soundManager.PlaySound(SoundNames.LevelWin);

            GameUI.DisplayMultiScore(artifactsCollected);
        }

        /// <summary>
        ///     Singleton Check
        /// </summary>
        private void Awake()
        {
            Instance = this;
        }
        /// <summary>
        ///     Cache references and start music.
        /// </summary>
        private void OnEnable()
        {
            soundManager = SoundManager.Instance;
            GameUI = GameObject.FindWithTag("UI").GetComponent<MultiUI>();

            soundManager.StopAllSound();
            soundManager.PlaySound(SoundNames.MusicMulti);
        }

        /// <summary>
        ///     Update health count.
        /// </summary>
        private void Start()
        {
            Health = playerHealth;
        }

        /// <summary>
        ///     Allows the user to reset with r and return with tab.
        /// </summary>
        private void Update()
        {
            // Allow the user to reload the game by pressing r
            if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // Allows the user to return to the main menu my pressing tab.
            if (Input.GetKeyDown(KeyCode.Tab)) GameUI.ToMainMenu();
        }
    }
}

