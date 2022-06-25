using System;
using System.Collections;
using UnityEngine;

namespace JonathansAdventure.UI.Cutscene
{
    /// <summary>
    ///     Creates fade in and fade out animation for objects.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/12/2022
    /// </remarks>
    [RequireComponent(typeof(CanvasGroup))]
    public class FadableObject : MonoBehaviour
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
        private protected void FadeIn()
        {
            StartCoroutine(FadeInCoroutine());
        }

        /// <summary>
        ///     Overload of <see cref="FadeIn"/> with a callback.
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="FadeInCoroutine">.
        /// </remarks>
        /// <param name="callback">
        ///     Callback <see cref="Action"/> to be called once the fade in is complete.
        /// </param>
        private protected void FadeIn(Action callback)
        {
            StartCoroutine(FadeInCoroutine(callback));
        }

        /// <summary>
        ///     Makes the object fade out.
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="FadeOutCoroutine"/>
        /// </remarks>
        private protected void FadeOut()
        {
            // Stop the fade in animations.
            // Do not call in the Coroutine as it will end the coroutine that called it.
            StopAllCoroutines();

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

            // Ensure the text is fully faded in (Update loop can leave a decimal.
            canvasGroup.alpha = 1f;
        }

        /// <summary>
        ///     Overload of <see cref="FadeInCoroutine"/> in which an action is ran
        ///     at the end of the coroutine.
        /// </summary>
        /// <param name="callback"> Runs at the end of the coroutine. </param>
        /// <returns> null </returns>
        private IEnumerator FadeInCoroutine(Action callback)
        {
            // Update loop based on time that ends after the fade in duration.
            for (float i = 0; i < fadeInDuration; i += Time.deltaTime)
            {
                // Divide by fade in duration to create a percent value.
                canvasGroup.alpha = i / fadeInDuration;
                // Required for loop.
                yield return null;
            }

            // Ensure the text is fully faded in (Update loop can leave a decimal.
            canvasGroup.alpha = 1f;

            // "?.Invoke()" guards against a null reference.
            callback?.Invoke();
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

            // Confirm that the alpha is 0 by the end of the animation.
            canvasGroup.alpha = 0f;
        }

    }
}