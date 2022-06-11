using UnityEngine;
using System.Collections;

namespace JonathansAdventure.UI.Cutscene
{
    /// <summary>
    ///     Manages skipping the cutscene.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Creates a skipping system that's split into two parts.
    ///         Pressing any key creates a message that prompts the 
    ///         user to press <see cref="KeyCode.Return"/> to confirm
    ///         that they want to skip the cutscene.
    ///     </para>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/10/2022
    ///     </para>
    /// </remarks>
    public class SkipMessage : MonoBehaviour
    {
        #region Settings

        [SerializeField, Range(0f, 10f),
            Tooltip("Set the amount of time, " +
            "the message confirming the user's choice to skip the cutscene, " +
            "shows up for.")]
        private float skipMsgTime;

        #endregion


        [SerializeField] private CanvasGroup skipText;
        [SerializeField] private CutsceneManager cutsceneManager;

        /// <summary>
        ///     Used to check if the user is on the step
        ///     where they confirm their skip.
        /// </summary>
        private bool skip = false;

        private bool voidSpace = false;

        internal void SetVoidSpace(bool value)
        {
            voidSpace = value;
        }

        private IEnumerator ConfirmSkipCoroutine()
        {
            yield return new WaitForSeconds(skipMsgTime);
            skip = false;
            skipText.alpha = 0;
        }

        private void Update()
        {
            if (!Input.anyKeyDown) return;
            if (skip)
            {
                if (Input.GetKey(KeyCode.Return)) cutsceneManager.Transition();
            }
            else
            {
                if (voidSpace && Input.GetKeyDown(KeyCode.Space)) return;
                StartCoroutine(ConfirmSkipCoroutine());
            }
        }
    }
}