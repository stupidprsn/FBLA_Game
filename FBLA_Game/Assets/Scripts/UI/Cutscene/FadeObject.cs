using System.Collections;
using UnityEngine;

namespace JonathansAdventure.UI.Cutscene
{
    /// <summary>
    ///     Creates fade in and fade out animation for objects.
    /// </summary>
    [RequireComponent(typeof(CanvasGroup))]
    public class FadeObject : MonoBehaviour
    {
        #region Settings

        [Header("Settings")]

        [SerializeField,
            Range(0f, 1f),
            Tooltip("Set the amount of time in seconds it takes for the text to fade in")]
        private float fadeInDuration;

        [SerializeField,
            Range(0f, 1f),
            Tooltip("Set the amount of time in seconds it takes for the text to fade out." +
                "This should be less than the fade in duration.")]
        private float fadeOutDuration;

        #endregion

        #region References
        
        [SerializeField] private CanvasGroup canvasGroup;

        #endregion

        /// <summary>
        ///     Makes the object fade in.
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="FadeInCoroutine">.
        /// </remarks>
        internal void FadeInText()
        {
            StartCoroutine(FadeInCoroutine());
        }

        /// <summary>
        ///     Makes the object fade out.
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="FadeOutCoroutine"/>
        /// </remarks>
        internal void FadeOutText()
        {
            StartCoroutine(FadeOutCoroutine());
        }

        /// <summary>
        ///     Creates fade in effect over <see cref="fadeDuration"/>.
        /// </summary>
        /// <returns> null </returns>
        private protected IEnumerator FadeInCoroutine()
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
        private protected IEnumerator FadeOutCoroutine()
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