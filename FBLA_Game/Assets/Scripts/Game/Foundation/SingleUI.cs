using UnityEngine;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Manages UI for singleplayer levels.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/30/2022
    /// </remarks>
    public class SingleUI : GameUI
    {
        #region Titles

        [Header("Titles")]

        [SerializeField,
            Tooltip("The title displayed when the user wins the game.")]
        private string winTitle;

        [SerializeField,
            Tooltip("The subtitle displayed when the user wins the game.")]
        private string winSubtitle;

        [Space(20f)]

        [SerializeField,
            Tooltip("The title displayed when the user loses the game.")]
        private string loseTitle;

        [SerializeField,
            Tooltip("The subtitle displayed when the user loses the game.")]
        private string loseSubtitle;

        #endregion

        private GameManager gameManager;

        /// <summary>
        ///     Reset gamemanager's singleton so that it
        ///     can be repawned.
        /// </summary>
        private protected override void ToMainMenuCallback()
        {
            GameManager.Instance = null;
            Destroy(gameManager.gameObject);
        }

        /// <summary>
        ///     Sets up the UI when a the user enters a new stage.
        /// </summary>
        /// <param name="manager"> Reference to the gamemanager. </param>
        /// <param name="congratulate"> Should the congratulatory message play. </param>
        /// <param name="gameTime"> The amount of time that has elapsed. </param>
        public void OnStageEnter(GameManager manager, bool congratulate, float gameTime)
        {
            // Update variables
            gameManager = manager;
            time = gameTime;

            // Show the UI.
            canvas.SetActive(true);

            // Hide the score panel.
            scorePanel.SetActive(false);

            if (congratulate)
            {
                // Pick a random congratulatory sentence.
                congratulatoryText.SetText(
                    congratulatoryMsgs[Random.Range(0, congratulatoryMsgs.Length)]
                );

                // Play the animation to show it.
                congratulatoryAnimator.SetTrigger("Show");
            }

            // Start updating the time.
            DoUpdateTime = true;
        }

        /// <summary>
        ///     Return <see cref="time"/> and <see cref="Health"/>
        ///     values to <see cref="GameManager"/>. Stop updating time
        ///     during transition, and crossfade into the next scene.
        /// </summary>
        public void OnStageExit()
        {
            DoUpdateTime = false;
            gameManager.Time = time;
            soundManager.PlaySound(SoundNames.LevelWin);
            transitions.CrossFade();
        }

        /// <summary>
        ///     Calculates and displayes the user's score.
        /// </summary>
        /// <param name="isWin"> If the score is being displayed because of a victory or defeat. </param>
        /// <param name="stagesCleared"> The number of stages the user has defeated. </param>
        /// <param name="snakesKilled"> The number of snakes the user has killed. </param>
        /// <param name="health"> THe number of lives the player has left. </param>
        public void DisplayScore(bool isWin, int stagesCleared, int snakesKilled, int health)
        {
            // 100 points for eachs stage cleared.
            int stageScore = stagesCleared * 100;

            // The player is expected to complete each stage in 200 seconds.
            int givenTime = stagesCleared * 200;

            // If the user gets a score below the given time,
            // They earn a bonus for each second below.
            // Else they get no time points.
            int timeScore;
            if (time < givenTime)
            {
                timeScore = givenTime - Mathf.FloorToInt(time);
            }
            else
            {
                timeScore = 0;
            }

            // The player gets 20 points for each snake killed.
            int snakeScore = snakesKilled * 10;

            // The player gets 50 points for each life left.
            int healthScore = health * 50;

            // Final score is composed of all of the subscores.
            int finalScore = stageScore + timeScore + snakeScore + healthScore;

            // Turn on the score panel.
            scorePanel.SetActive(true);

            if (isWin)
            {
                scoreTitle.SetText(winTitle);
                scoreSubtitle.SetText(winSubtitle);
            }
            else
            {
                scoreTitle.SetText(loseTitle);
                scoreSubtitle.SetText(loseSubtitle);
            }

            // Display the info.
            // Formatting is weird because @ reads indentation.
            scoreText.SetText(string.Format(
$@"Stages Cleared ({stagesCleared})
Time({Mathf.FloorToInt(time)})
Snakes Killed({snakesKilled})
Lives({health})
------------------------------------------
Total: "
            ));

            // Displays the score.
            // Formatting is weird because @ reads indentation.
            scoreNumbers.SetText(string.Format(
$@"{stageScore}
{timeScore}
{snakeScore}
{healthScore}

{finalScore}"
            ));

            // Alow the user to input their name and disable game manager.
            inputName.ActivateInputField();
            gameManager.enabled = false;

            StartCoroutine(WaitForName(finalScore));
        }
    }
}
