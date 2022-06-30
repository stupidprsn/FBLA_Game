using System.Collections;
using UnityEngine;
using TMPro;
using JonathansAdventure.Transitions;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Base class for managing the UI during gameplay.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/30/2022
    /// </remarks>
    public abstract class GameUI : MonoBehaviour
    {
        #region Settings

        [Header("Settings")]

        [SerializeField,
            Tooltip("Set the congratulatory messages that can be shown when the player finishes a level.")]
        private protected string[] congratulatoryMsgs;

        #endregion

        #region References

        [Header("References")]
        [SerializeField] private protected GameObject canvas;
        [SerializeField] private protected TMP_Text timeText;
        [SerializeField] private protected RectTransform heartsImage;

        [Space(20f)]

        [SerializeField] private protected GameObject scorePanel;
        [SerializeField] private protected TMP_Text scoreTitle;
        [SerializeField] private protected TMP_Text scoreSubtitle;
        [SerializeField] private protected TMP_Text scoreText;
        [SerializeField] private protected TMP_Text scoreNumbers;
        [SerializeField] private protected TMP_InputField inputName;

        [Space(20f)]

        [SerializeField] private protected TMP_Text congratulatoryText;
        [SerializeField] private protected Animator congratulatoryAnimator;

        private protected TransitionManager transitions;
        private protected SoundManager soundManager;

        #endregion

        #region Variables

        /// <summary>
        ///     The time (in seconds) of the player's run.
        /// </summary>
        private protected float time;

        /// <summary>
        ///     If the time should be updated.
        /// </summary>
        internal bool DoUpdateTime { private get; set; } = false;

        #endregion

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
        ///     Waits for the user to press enter to return to the main menu.
        ///     Saves their score and name to the leaderboard.
        /// </summary>
        /// <returns> Null </returns>
        private protected IEnumerator WaitForName(int score)
        {
            FileManager fileManager = FileManager.Instance;

            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Return));

            soundManager.PlaySound(SoundNames.SpaceContinue);

            if (GameData.IsSingleplayer)
            {
                fileManager.SingleLeaderboard.SaveEntry(inputName.text, score);
            }
            else
            {
                fileManager.MultiLeaderboard.SaveEntry(inputName.text, score);
            }

            ToMainMenu(MenuPanels.LeaderboardScreen);
        }

        /// <summary>
        ///     Runs at the end of <see cref="ToMainMenu(MenuPanels)"/>
        ///     before the main menu is loaded.
        /// </summary>
        private protected abstract void ToMainMenuCallback();

        /// <summary>
        ///     Returns to the main menu.
        /// </summary>
        /// <param name="panel"> The panel to return to (Default to home). </param>
        public void ToMainMenu(MenuPanels panel = MenuPanels.HomeScreen)
        {
            GameData.MenuPanel = panel;
            transitions.CrossFade(Scenes.MainMenu);
            ToMainMenuCallback();
            Destroy(gameObject);
        }

        /// <summary>
        ///     Cache References.
        /// </summary>
        private protected virtual void Start()
        {
            soundManager = SoundManager.Instance;
            transitions = TransitionManager.Instance;
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