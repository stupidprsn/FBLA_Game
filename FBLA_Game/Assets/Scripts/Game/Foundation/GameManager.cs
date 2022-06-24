using System.Collections;
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
    ///     Last Modified: 6/23/2022
    /// </remarks>
    public class GameManager : Singleton<GameManager>
    {
        #region Settings

        [Header("Set Game Settings")]

        [SerializeField,
            Range(1, 10),
            Tooltip("Set the number of lives that the user has.")]
        private int playerHealth;

        [SerializeField,
            Range(1f, 10f),
            Tooltip("Set the amount of time (in seconds) it takes for the user to respawn.")]
        private int respawnTime;

        #endregion

        #region Constant Between Stages

        /// <summary>
        ///     The lives the player has left.
        /// </summary>
        private int health;

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
            gameUI.DoUpdateTime = false;
            soundManager.PlaySound(SoundNames.LevelWin);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            stagesCleared++;
            congradulate = true;
        }

        /// <summary>
        ///     If the player beat a stage before and thus should be congratulated.
        /// </summary>
        private bool congradulate = false;

        private SoundManager soundManager;
        private GameUI gameUI;

        #endregion

        #region Stage References

        internal Player CurrentPlayer { get; private set; }

        #endregion

        /// <summary>
        ///     Called by <see cref="Normal.OnEnterLevel"/> to reset references.
        /// </summary>
        /// <param name="music"> The music to play for this stage. </param>
        public void OnStageEnter(SoundNames music)
        {
            CurrentPlayer = new Player();
            CurrentPlayer.SetReferences();

            gameUI = GameObject.FindWithTag("UI").GetComponent<GameUI>();
            gameUI.gameObject.SetActive(false);

            // Update the UI and start the clock.
            gameUI.OnStageEnter(congradulate, health);

            // Stops all sound except for:
            //  1) the music for that stage and play it if it's not already playing
            //  2) the win sound
            // This is so that the music is continous between stages with the same music
            soundManager.StopAllSound(music, SoundNames.LevelWin);
            soundManager.PlaySound(music);

        }

        /// <summary>
        ///     Death for player. 
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="OnDeathCoroutine">.
        /// </remarks>
        public void OnDeath()
        {
            StartCoroutine(OnDeathCoroutine());
        }

        /// <summary>
        ///     <see cref="OnDeath"/>.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator OnDeathCoroutine()
        {
            // Update life count.
            health--;
            gameUI.UpdateHealthDisplay(health);

            // Show the death animation and lock the player.
            CurrentPlayer.PlayerAnimation.PlayAnimation(CurrentPlayer.PlayerAnimation.DiedID);
            CurrentPlayer.PlayerMovement.enabled = false;
            CurrentPlayer.PlayerRB.constraints = RigidbodyConstraints2D.FreezeAll;

            soundManager.PlaySound(SoundNames.Died);

            yield return new WaitForSeconds(respawnTime);

            // Checks if the player still has health
            if (health > 0)
            {
                // Reset the player position and unlock the player.
                CurrentPlayer.Transform.position = CurrentPlayer.SpawnPosition;
                CurrentPlayer.PlayerAnimation.PlayAnimation(CurrentPlayer.PlayerAnimation.IdleID);
                CurrentPlayer.PlayerMovement.enabled = true;
                CurrentPlayer.PlayerRB.constraints = RigidbodyConstraints2D.FreezeRotation;
                
                soundManager.PlaySound(SoundNames.Revive);
            }
            else
            {
                gameUI.DisplayScore(false, stagesCleared, snakesKilled, health);

                soundManager.StopAllSound();
                soundManager.PlaySound(SoundNames.GameOver);

            }
        }

        /// <summary>
        ///     Call when the player wins the entire game (not just the stage).
        /// </summary>
        public void WinGame()
        {
            // Lock player movement
            CurrentPlayer.PlayerMovement.enabled = false;
            CurrentPlayer.PlayerRB.constraints = RigidbodyConstraints2D.FreezeAll;

            soundManager.StopAllSound();
            soundManager.PlaySound(SoundNames.LevelWin);

            gameUI.DisplayScore(true, stagesCleared, snakesKilled, health);
        }
        /// <summary>
        ///     Perform singleton check and initate variables.
        /// </summary>
        private void Awake()
        {
            SingletonCheck(this);
            health = playerHealth;
            soundManager = SoundManager.Instance;
        }

        /// <summary>
        ///     Allows the user to reset with r and return with tab.
        /// </summary>
        private void Update()
        {
            // Allow the user to reload the game by pressing r
            if (Input.GetKeyDown(KeyCode.R)) SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

            // Allows the user to return to the main menu my pressing tab.
            if (Input.GetKeyDown(KeyCode.Tab)) gameUI.ToMainMenu();
        }
    }

}

