using UnityEngine;
using UnityEngine.UI;
using JonathansAdventure.Sound;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Basae class for any UI that uses WASD to navigate the buttons.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/24/2022
    /// </remarks>
    public abstract class ButtonLayout : MonoBehaviour
    {
        #region References

        [Header("Object References")]

        [SerializeField,
            Tooltip("Drag in the buttons from top to bottom.")]
        private protected Button[] buttons;

        private protected SoundManager soundManager;

        #endregion

        #region Variables

        /// <summary>
        ///     The last valid index to <see cref="buttons"/>.
        /// </summary>
        private protected int lastIndex;

        /// <summary>
        ///     The index of the button currently selected.
        /// </summary>
        private protected int buttonIndex = 0;

        #endregion

        #region Abstract

        /// <summary>
        ///     Goes to the next button (up or right).
        /// </summary>
        private protected void Increment()
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

        /// <summary>
        ///     Goes to the previous button (down or left).
        /// </summary>
        private protected void Decrement()
        {
            // Inversed as a lower button has a higher index.
            buttonIndex++;
            if (buttonIndex > lastIndex)
            {
                buttonIndex = 0;
            }
            buttons[buttonIndex].Select();
            soundManager.PlaySound(SoundNames.ButtonSelect);
        }

        /// <summary>
        ///     "clicks" the button.
        /// </summary>
        private protected void Select()
        {
            soundManager.PlaySound(SoundNames.SpaceContinue);

            // Perform the action associated with the button
            buttons[buttonIndex].onClick.Invoke();
        }

        #endregion

        /// <summary>
        ///     Find the last index and the sound manager.
        /// </summary>
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
    }
}

