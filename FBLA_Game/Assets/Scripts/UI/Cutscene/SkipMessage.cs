using UnityEngine;
using System.Collections;
using JonathansAdventure.Sound;

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
    public class SkipMessage : FadableObject
    {
        #region Settings

        [SerializeField, Range(0f, 10f),
            Tooltip("Set the amount of time, " +
            "the message confirming the user's choice to skip the cutscene, " +
            "shows up for.")]
        private float skipMsgTime;

        #endregion

        #region References

        [SerializeField] private CutsceneManager cutsceneManager;
        private SoundManager soundManager;
        #endregion

        /// <summary>
        ///     Used to check if the user is on the step
        ///     where they confirm their skip.
        /// </summary>
        private bool skip = false;

        /// <summary>
        ///     If the cutscene is currently prompting the 
        ///     user to press space to continue.
        /// </summary>
        /// <remarks>
        ///     This way, the skip message does not appear
        ///     if the user is simply trying to move on
        ///     with the cutscene.
        /// </remarks>
        private bool voidSpace = false;

        /// <summary>
        ///     Set Property for <see cref="voidSpace"/>.
        /// </summary>
        /// <param name="value"> Value to set. </param>
        internal void SetVoidSpace(bool value)
        {
            voidSpace = value;
        }

        /// <summary>
        ///     Manages displaying the message prompting
        ///     the user to confirm if they want to 
        ///     skip the cutscene.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator ConfirmSkipCoroutine()
        {
            FadeIn();
            yield return new WaitForSeconds(skipMsgTime);
            FadeOut();
            skip = false;
        }

        private void Start()
        {
            soundManager = SoundManager.Instance;
        }

        private void Update()
        {
            // Guard clause that checks if a key was pressed.
            if (!Input.anyKeyDown) return;
            // If the user is trying to skip (false) or confirming their skip (true).
            if (skip)
            {
                // Guard clause checks that the button pressed was the return key.
                if (!Input.GetKey(KeyCode.Return)) return;
                soundManager.PlaySound(SoundNames.SpaceContinue);
                cutsceneManager.Transition();
            }
            else
            {
                // Guard clause to check that the user wasn't pressing space to continue.
                if (voidSpace && Input.GetKeyDown(KeyCode.Space)) return;
                soundManager.PlaySound(SoundNames.SpaceContinue);
                StartCoroutine(ConfirmSkipCoroutine());
            }
        }
    }
}