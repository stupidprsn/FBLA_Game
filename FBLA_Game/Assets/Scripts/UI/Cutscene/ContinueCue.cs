using UnityEngine;
using TMPro;
using System.Collections;

namespace JonathansAdventure.UI.Cutscene
{
    /// <summary>
    ///     Manages the cue that tells the user to press
    ///     space to continue in the cutscene.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/12/2022
    /// </remarks>
    [RequireComponent(typeof(TMP_Text))]
    public class ContinueCue : FadableObject
    {
        #region Settings

        [Header("Settings")]

        [SerializeField, 
            Range(0f, 1f),
            Tooltip("Set the amount of time in seconds for the text to flash from one frame to the next")]
        private float blinkSpeed;

        [SerializeField, 
            Tooltip("Set the different frames of text that the program flashes between")]
        private string[] texts;

        #endregion

        #region References

        [Header("Object Reference")]

        [SerializeField] private TMP_Text text;

        #endregion

        /// <summary>
        ///     Makes the cue fade in and starts the blinking animation
        ///     once it has finished fading in.
        /// </summary>
        internal void FadeInText()
        {
            // Anonymous action that starts the blinking animation.
            FadeIn(() =>
            {
                StartCoroutine(BlinkCoroutine());
            });
        }

        internal void FadeOutText()
        {
            FadeOut();
        }

        /// <summary>
        ///     Creates blinking animation.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator BlinkCoroutine()
        {
            // Loop forever (until coroutine is forced to stop)
            while (true)
            {
                // Iterate through the blink states at the speed set.
                foreach (string nextText in texts)
                {
                    text.text = nextText;
                    yield return new WaitForSeconds(blinkSpeed);
                }
                // Required for loop.
                yield return null;
            }
        }
    }

}

