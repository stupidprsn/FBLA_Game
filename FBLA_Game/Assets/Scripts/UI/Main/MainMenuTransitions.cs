using System.Collections;
using UnityEngine;

namespace JonathansAdventure.UI.Main
{
    public class MainMenuTransitions : MonoBehaviour
    {
        #region Settings

        [Header("Transition Settings")]

        [SerializeField,
            Range(0.1f, 2f),
            Tooltip("Set the amount of time in seconds it takes for panel transitions.")]
        private float translationDuration = 1f;

        #endregion

        /// <summary>
        ///     Keeps track of how many coroutines have finished.
        /// </summary>
        /// <remarks>
        ///     Used durring <see cref="TransitionCoroutine{TFrom, TTo}(GameObject, GameObject, bool)"/>
        ///     to check if both shift coroutines have finished.
        /// </remarks>
        private int coroutinesFinished = 0;

        /// <summary>
        ///     Transitions from one panel to another.
        /// </summary>
        /// <remarks>
        ///     Creates a horizontal text sliding effect.
        /// </remarks>
        /// <typeparam name="TFrom"> The class controlling the panel being transitioned from. </typeparam>
        /// <typeparam name="TTo"> The class controlling the panel being transitioned to. </typeparam>
        /// <param name="fromPanel"> The panel being transitioned from. </param>
        /// <param name="toPanel"> The panel being transitioned to. </param>
        /// <param name="toLeft"> If the panels are moving left. </param>
        internal void Transition(GameObject fromPanel, GameObject toPanel, bool toLeft)
        {
            StartCoroutine(TransitionCoroutine(fromPanel, toPanel, toLeft));
        }

        /// <summary>
        ///     <see cref="Transition{TFrom, TTo}(GameObject, GameObject, bool)"/>
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator TransitionCoroutine(GameObject fromPanel, GameObject toPanel, bool toLeft)
        {
            // Get the components of the panel being transitioned from.
            RectTransform fromT = fromPanel.GetComponent<RectTransform>();
            CanvasGroup fromC = fromPanel.GetComponent<CanvasGroup>();
            MonoBehaviour[] fromM = fromPanel.GetComponents<MonoBehaviour>();

            // Get the components of the panel being transitioned to.
            RectTransform toT = toPanel.GetComponent<RectTransform>();
            CanvasGroup toC = toPanel.GetComponent<CanvasGroup>();
            MonoBehaviour[] toM = toPanel.GetComponents<MonoBehaviour>();

            // Activate the panel but disable its interactivity.
            fromPanel.SetActive(true);
            fromC.interactable = false;
            foreach (MonoBehaviour m in fromM)
            {
                m.enabled = false;
            }

            // Activate the panel but disable its interactivity.
            toPanel.SetActive(true);
            toC.interactable = false;
            foreach (MonoBehaviour m in fromM)
            {
                m.enabled = false;
            }

            // If the panels are moving left, place the toPanel to the right.
            // Otherwise place it to the left.
            // 1920 is the pixel width of the screen.
            if (toLeft)
            {
                toT.anchoredPosition = new Vector2(1920, 0);

                StartCoroutine(Shift(fromT, new Vector2(-1920, 0)));
                StartCoroutine(Shift(toT, new Vector2(0, 0)));
            }
            else
            {
                toT.anchoredPosition = new Vector2(-1920, 0);

                StartCoroutine(Shift(fromT, new Vector2(1920, 0)));
                StartCoroutine(Shift(toT, new Vector2(0, 0)));
            }

            // Wait until both shifts are finished.
            yield return new WaitUntil(() => (coroutinesFinished == 2));
            // Reset trigger.
            coroutinesFinished = 0;

            // Enable interactivity on toPanel
            toC.interactable = true;
            foreach (MonoBehaviour m in fromM)
            {
                m.enabled = true;
            }

            // Disable fromPanel
            fromPanel.SetActive(false);
        }

        /// <summary>
        ///     Creates linear translation.
        /// </summary>
        /// <remarks>
        ///     Linear translation of <paramref name="panel"/>
        ///     from its current position to <paramref name="endPos"/>
        ///     over <see cref="translationDuration"/>.
        /// </remarks>
        /// <param name="panel"> The panel to shift. </param>
        /// <param name="endPos"> The position it should end up at. </param>
        /// <returns> null </returns>
        private IEnumerator Shift(RectTransform panel, Vector2 endPos)
        {
            // Set up Lerp.
            Vector2 startPos = panel.anchoredPosition;
            float t = 0;

            // Transition over the translation duration.
            while (t < translationDuration)
            {
                panel.anchoredPosition = Vector2.Lerp(startPos, endPos, t / translationDuration);
                t += Time.deltaTime;
                yield return null;
            }

            // Confirm the final position.
            panel.anchoredPosition = endPos;

            // Signify that the coroutine finished.
            coroutinesFinished++;
        }
    }
}
