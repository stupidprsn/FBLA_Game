using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Manages the UI during gameplay.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/22/2022
    /// </remarks>
    public class GameUI : Singleton<GameUI>
    {
        #region Settings

        [Header("Settings")]

        [SerializeField,
            Tooltip("The title displayed when the user wins the game")]
        private string winTitle;

        [SerializeField,
            Tooltip("The subtitle displayed when the user wins the game")]
        private string winSubtitle;

        [Space(20f)]

        [SerializeField,
            Tooltip("The title displayed when the user loses the game")]
        private string loseTitle;

        [SerializeField,
            Tooltip("The subtitle displayed when the user loses the game")]
        private string loseSubtitle;

        [Space(20f)]

        [SerializeField,
            Tooltip("Set the congratulatory messages that can be shown when the player finishes a level")]
        private string[] congratulatoryMsgs;

        #endregion

        #region References

        [Header("References")]
        [SerializeField] private GameObject canvas;
        [SerializeField] private TMP_Text timeText;
        [SerializeField] private RectTransform heartsImage;

        [Space(20f)]

        [SerializeField] private GameObject scorePanel;
        [SerializeField] private TMP_Text scoreTitle;
        [SerializeField] private TMP_Text scoreSubtitle;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text scoreNumbers;
        [SerializeField] private TMP_InputField inputName;

        [Space(20f)]

        [SerializeField] private TMP_Text CongratulatoryText;
        [SerializeField] private Animator congratulatoryAnimator;


        #endregion

        #region Variables

        /// <summary>
        ///     The time (in seconds) of the player's run.
        /// </summary>
        private float time = 0f;

        /// <summary>
        ///     If the time should be updated.
        /// </summary>
        internal bool DoUpdateTime { private get; set; } = false;

        #endregion

        /// <summary>
        ///     Sets up the UI when a the user enters a new stage.
        /// </summary>
        /// <param name="congratulate"> Should the congratulatory message play. </param>
        internal void OnStageEnter(bool congratulate, int health)
        {
            // Hide the score panel.
            scorePanel.SetActive(false);

            // Show the UI.
            canvas.SetActive(true);

            // Update the health image.
            UpdateHealthDisplay(health);

            if (congratulate)
            {
                // Pick a random congratulatory sentence.
                CongratulatoryText.SetText(
                    congratulatoryMsgs[Random.Range(0, congratulatoryMsgs.Length)]
                );

                // Play the animation to show it.
                congratulatoryAnimator.SetTrigger("Show");
            }

            // Start updating the time.
            DoUpdateTime = true;
        }

        /// <summary>
        ///     Updates the health bar to reflect the user's health.
        /// </summary>
        internal void UpdateHealthDisplay(int health)
        {
            // The image for displaying the health repeats the heart sprite which is 50 pixels.
            // The number of hearts shown can be updated by editing the size of this image.
            heartsImage.sizeDelta = new Vector2((health * 50), 50);
        }

        /// <summary>
        ///     Calculates and displayes the user's score.
        /// </summary>
        /// <param name="isWin"> If the score is being displayed because of a victory or defeat. </param>
        /// <param name="stagesCleared"> The number of stages the user has defeated. </param>
        /// <param name="snakesKilled"> The number of snakes the user has killed. </param>
        /// <param name="health"> The number of lives the user has left. </param>
        /// <returns> The player's final score. </returns>
        internal void DisplayScore(bool isWin, int stagesCleared, int snakesKilled, int health)
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

            // Alow the user to input their name.
            inputName.ActivateInputField();

            StartCoroutine(WaitForName(finalScore));
        }

        /// <summary>
        ///     Waits for the user to press enter to return to the main menu.
        ///     Saves their score and name to the leaderboard.
        /// </summary>
        /// <returns> Null </returns>
        private IEnumerator WaitForName(int score)
        {
            FileManager fileManager = FileManager.Instance;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

            SoundManager.Instance.PlaySound(SoundNames.SpaceContinue);
            fileManager.SingleLeaderboard.SaveEntry(inputName.text, score);
            ToMainMenu(MenuPanels.LeaderboardScreen);
        }

        /// <summary>
        ///     Returns to the main menu.
        /// </summary>
        /// <param name="panel"> The panel to return to (Default to home). </param>
        internal void ToMainMenu(MenuPanels panel = MenuPanels.HomeScreen)
        {
            GameData.MenuPanel = panel;
            SceneManager.LoadScene(2);
            Destroy(gameObject);
        }

        /// <summary>
        ///     Run singleton check.
        /// </summary>
        private void Awake()
        {
            SingletonCheck(this);
        }

        /// <summary>
        ///     Update the time count.
        /// </summary>
        private void Update()
        {
            if (DoUpdateTime) time += Time.deltaTime;
            timeText.SetText(Mathf.FloorToInt(time).ToString());
        }
    }
}