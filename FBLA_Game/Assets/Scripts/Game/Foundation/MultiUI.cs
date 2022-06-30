using UnityEngine;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Manages UI for singleplayer levels.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/30/2022
    /// </remarks>
    public class MultiUI : GameUI
    {
        #region Titles

        [Header("Titles")]

        [SerializeField,
            Tooltip("The title displayed for multiplayer mode.")]
        private string multiTitle;

        [SerializeField,
            Tooltip("The subtitle displayed for multiplayer mode.")]
        private string multiSubtitle;

        #endregion

        /// <summary>
        ///     Calculates and displayes the user's score for multiplayer mode.
        /// </summary>
        /// <param name="artifactsCollected"> The number of snakes the user has killed. </param>
        public void DisplayMultiScore(int artifactsCollected)
        {
            int timeScore = Mathf.FloorToInt(time);
            int artifactScore = artifactsCollected * 10;
            int finalScore = timeScore + artifactScore;

            // Turn on the score panel.
            scorePanel.SetActive(true);

            // Update the title and subtitle.
            scoreTitle.SetText(multiTitle);
            scoreSubtitle.SetText(multiSubtitle);

            // Display the info.
            // Formatting is weird because @ reads indentation.
            scoreText.SetText(string.Format(
$@"Time({timeScore})
Artifacts Collected ({artifactsCollected})
------------------------------------------
Total: "
            ));

            // Displays the score.
            // Formatting is weird because @ reads indentation.
            scoreNumbers.SetText(string.Format(
$@"{timeScore}
{artifactScore}

{finalScore}"
            ));

            // Alow the user to input their name and disable game manager.
            inputName.ActivateInputField();
            MultiplayerManager.Instance.enabled = false;

            StartCoroutine(WaitForName(finalScore));
        }

        /// <summary>
        ///     No callback necesarry.
        /// </summary>
        private protected override void ToMainMenuCallback() { }

        /// <summary>
        ///     Turn on the canvas and start the clock.
        /// </summary>
        private protected override void Start()
        {
            base.Start();
            // Show the UI.
            canvas.SetActive(true);

            // Hide the score panel.
            scorePanel.SetActive(false);

            // Start updating the time.
            DoUpdateTime = true;
        }
    }
}
