using UnityEngine;
using UnityEngine.SceneManagement;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Manages singleplayer gameplay.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/30/2022
    /// </remarks>
    public class GameManager : Singleton<GameManager>
    {

        [SerializeField,
            Range(1, 10),
            Tooltip("Set the number of lives that the user has.")]
        private int playerHealth;

        #region Variables

        /// <summary>
        ///     The number of snakes the player has killed.
        /// </summary>
        private int snakesKilled;

        /// <summary>
        ///     Increment the number of snakes killed.
        /// </summary>
        public void SnakeKilled()
        {
            snakesKilled++;
        }

        /// <summary>
        ///     The number of stages the user has cleared.
        /// </summary>
        private int stagesCleared;

        /// <summary>
        ///     Call when the player beats the current stage.
        /// </summary>
        public void StageCleared()
        {
            GameUI.OnStageExit();
            stagesCleared++;
            congradulate = true;
        }

        /// <summary>
        ///     The time (in seconds) of the player's run.
        /// </summary>
        public float Time { get; set; } = 0f;

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

        /// <summary>
        ///     If the player beat a stage before and thus should be congratulated.
        /// </summary>
        private bool congradulate = false;
        public SingleUI GameUI { get; private set; }

        private SoundManager soundManager;
        private PlayerDeath playerDeath;
        #endregion

        /// <summary>
        ///      Called by <see cref="Normal.OnEnterLevel"/> to reset references.
        /// </summary>
        /// <param name="music"> The music to play for this stage. </param>
        public void OnStageEnter(SoundNames music)
        {

            GameUI = GameObject.FindWithTag("UI").GetComponent<SingleUI>();
            // Update the UI and start the clock.
            GameUI.OnStageEnter(this, congradulate, Time);

            playerDeath = GameObject.FindWithTag("Player").GetComponent<PlayerDeath>();

            // Stops all sound except for:
            //  1) the music for that stage and play it if it's not already playing
            //  2) the win sound
            // This is so that the music is continous between stages with the same music
            soundManager.StopAllSound(music, SoundNames.LevelWin);
            soundManager.PlaySound(music);

        }

        /// <summary>
        ///     Call when the player wins the entire game (not just the stage).
        /// </summary>
        public void WinGame()
        {
            // Lock player movement
            playerDeath.DisableMovement();

            DisplayScore(SoundNames.LevelWin, true);
        }

        /// <summary>
        ///     Displays the leaderboard
        /// </summary>
        /// <param name="soundName"> The sound effect to play. </param>
        /// <param name="win"> If the user won. </param>
        public void DisplayScore(SoundNames soundName, bool win)
        {
            soundManager.StopAllSound();
            soundManager.PlaySound(soundName);

            GameUI.DisplayScore(win, stagesCleared, snakesKilled, Health);
        }

        /// <summary>
        ///     Perform singleton check and initate player health;
        /// </summary>
        private void Awake()
        {
            SingletonCheck(this);
        }

        /// <summary>
        ///     Set Reference.
        /// </summary>
        private void OnEnable()
        {
            soundManager = SoundManager.Instance;
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

