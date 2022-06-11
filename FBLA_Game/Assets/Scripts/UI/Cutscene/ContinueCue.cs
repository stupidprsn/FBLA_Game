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
    ///     Last Modified: 6/10/2022
    /// </remarks>
    [RequireComponent(typeof(TMP_Text), typeof(CanvasGroup))]
    public class ContinueCue : MonoBehaviour
    {
        #region Settings

        [Header("Settings")]

        [
            SerializeField, 
            Range(0f, 1f),
            Tooltip("Set the amount of time in seconds it takes for the text to fade in")
        ]
        private float fadeInDuration;

        [
            SerializeField,
            Range(0f, 1f),
            Tooltip("Set the amount of time in seconds it takes for the text to fade out." +
                "This should be less than the fade in duration.")
]
        private float fadeOutDuration;

        [
            SerializeField, 
            Range(0f, 1f),
            Tooltip("Set the amount of time in seconds for the text to flash from one frame to the next")
        ]
        private float blinkSpeed;

        [
            SerializeField, 
            Tooltip("Set the different frames of text that the program flashes between")
        ]
        private string[] texts;

        #endregion

        #region References

        [Header("Object Reference")]

        [SerializeField] private TMP_Text text;
        [SerializeField] private CanvasGroup canvasGroup;

        #endregion

        /// <summary>
        ///     Makes the cue fade in.
        /// </summary>
        /// <remarks>
        /// <para>
        ///     Run once the main cutscene text has finished typing out.
        /// </para>
        /// <para>
        ///     Wrapper method for <see cref="FadeInCoroutine">.
        /// </para>
        /// </remarks>
        internal void FadeInText()
        {
            StartCoroutine(FadeInCoroutine());
        }

        /// <summary>
        ///     Makes the cue fade out.
        /// </summary>
        /// <remarks>
        ///     Run once the user presses <see cref="KeyCode.Space"/>.
        /// </remarks>
        internal void FadeOutText()
        {
            StartCoroutine(FadeOutCoroutine());
        }

        /// <summary>
        ///     Creates fade in effect over <see cref="fadeDuration"/>.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator FadeInCoroutine()
        {
            // Update loop based on time that ends after the fade in duration.
            for (float i = 0; i < fadeInDuration; i += Time.deltaTime)
            {
                // Divide by fade in duration to create a percent value.
                canvasGroup.alpha = i / fadeInDuration;
                // Required for loop.
                yield return null;
            }

            // Ensure the text is fully faded in (Update loop can leave a decimal).
            canvasGroup.alpha = 1f;
            
            // Start the blinking animation after the text is fully visible.
            StartCoroutine(BlinkCoroutine());
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

        /// <summary>
        ///     Creates fade out animation.
        /// </summary>
        /// <remarks>
        ///     Dynamically changes the duration of the animation
        ///     to maintain the same fade out speed.
        /// </remarks>
        /// <example>
        ///     If the text if half faded in, the animation would
        ///     play at twice the speed.
        /// </example>
        /// <returns> null </returns>
        private IEnumerator FadeOutCoroutine()
        {
            // Stop the fade in animations.
            StopAllCoroutines();
            // Record the alpha the fade in animation left off at.
            float startingAlpha = canvasGroup.alpha;
            // Dynamically changes the duration. 
            fadeOutDuration *= startingAlpha;
            // Inverse update loop based on time.
            for (float i = fadeOutDuration; i > 0; i -= Time.deltaTime)
            {
                // Ensure the starting alpha is the highest.
                canvasGroup.alpha = startingAlpha * i / fadeOutDuration;
                // Required for loop.
                yield return null;
            }
        }
    }

}

