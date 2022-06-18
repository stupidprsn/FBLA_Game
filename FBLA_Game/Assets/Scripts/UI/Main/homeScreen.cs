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

        private int buttonIndex;

        /// <summary>
        ///     The index of the current button selected.
        /// </summary>
        /// <remarks>
        ///     Wraps the value of <see cref="buttonIndex"/>
        ///     from 0 to <see cref="lastIndex"/>.
        /// </remarks>
        private int ButtonIndex
        {
            get => buttonIndex;
            set
            {
                if (value < 0)
                {
                    buttonIndex = lastIndex;
                    return;
                }
                if(value > lastIndex)
                {
                    buttonIndex = 0;
                    return;
                }
                buttonIndex = value;
            }
        }

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
            soundManager = SoundManager.Instance;
        }

        /// <summary>
        ///     Select the default (play) button.
        /// </summary>
        private void Start()
        {
            buttons[buttonIndex].Select();
        }

        private void Update()
        {
            if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            {
                buttons[++buttonIndex].Select();
            }

            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            {
                buttons[--buttonIndex].Select();
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

        /// <summary>
        ///     Update <see cref="lastIndex"/> when <see cref="buttons"/> is changed.
        /// </summary>
        private void OnValidate()
        {
            lastIndex = buttons.Length - 1;
        }
    }
}

