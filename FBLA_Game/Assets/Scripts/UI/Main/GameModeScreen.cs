using UnityEngine;
using UnityEngine.UI;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Provides functionality for the gamemode select screen.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/24/2022
    /// </remarks>
    public class GameModeScreen : HorizontalLayout
    {
        [SerializeField] private Button quitButton;

        #region Button Methods

        /// <summary>
        ///     Quits the application.
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }

        /// <summary>
        ///     Sets the game mode.
        /// </summary>
        /// <param name="singlePlayer"> If the game is in singleplayer or not. </param>
        public void SetGameMode(bool singlePlayer)
        {
            GameData.IsSingleplayer = singlePlayer;
        }

        #endregion

        /// <summary>
        ///     If the quit button is currently selected.
        /// </summary>
        private bool quitSelected = false;

        /// <summary>
        ///     Detect user input.
        /// </summary>
        private protected override void Update()
        {
            // Since there is only one button in the quit button's row,
            // if it is selected, the user can't navigate left or right.
            if (quitSelected)
            {
                // Returns to first row.
                if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
                {
                    buttons[buttonIndex].Select();
                    soundManager.PlaySound(SoundNames.ButtonSelect);
                    quitSelected = false;
                }
            }
            else
            {
                // Go to quit button.
                if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
                {
                    quitButton.Select();
                    quitSelected = true;
                }

                // Left and right navigation.
                if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) Increment();

                if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) Decrement();
            }

            // Select button with space.
            if (Input.GetKeyDown(KeyCode.Space)) Select();
        }
    }

}

