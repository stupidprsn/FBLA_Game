using UnityEngine;
using UnityEngine.UI;
using JonathansAdventure.Sound;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Creates navigation on the home screen.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/17/2022
    /// </remarks>
    public class HomeScreen : MonoBehaviour
    {

        #region References

        [Header("Object References")]

        [SerializeField,
            Tooltip("Drag in the buttons from top to bottom.")]
        private Button[] buttons;

        private SoundManager soundManager;

        #endregion

        /// <summary>
        ///     The last valid index to <see cref="buttons"/>.
        /// </summary>
        private int lastIndex;

        /// <summary>
        ///     The index of the button currently selected.
        /// </summary>
        private int buttonIndex;

        /// <summary>
        ///     Closes the application.
        /// </summary>
        /// <remarks>
        ///     Used by the quit button.
        /// </remarks>
        public void QuitButton()
        {
            Application.Quit();
        }

        private void Awake()
        {
            lastIndex = buttons.Length - 1;
        }

        /// <summary>
        ///     Select the default (play) button.
        /// </summary>
        private void Start()
        {
            soundManager = SoundManager.Instance;
            buttons[buttonIndex].Select();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                // Inversed as a higher button has a lower index.
                buttonIndex--;
                if (buttonIndex < 0)
                {
                    buttonIndex = lastIndex;
                }
                buttons[buttonIndex].Select();
                soundManager.PlaySound(SoundNames.ButtonSelect);
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                // Inversed as a lower button has a higher index.
                buttonIndex++;
                if(buttonIndex > lastIndex)
                {
                    buttonIndex = 0;
                }
                buttons[buttonIndex].Select();
                soundManager.PlaySound(SoundNames.ButtonSelect);
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                soundManager.PlaySound(SoundNames.SpaceContinue);

                // Reset button visuals
                buttons[buttonIndex].GetComponent<Image>().color = new Color(255, 255, 255, 0);

                // Perform the action associated with the button
                buttons[buttonIndex].onClick.Invoke();
            }
        }
    }
}

