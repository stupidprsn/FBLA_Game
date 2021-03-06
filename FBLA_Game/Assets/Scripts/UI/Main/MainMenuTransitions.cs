using System.Collections;
using UnityEngine;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Manages transitions in the main menu.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/24/2022
    /// </remarks>
    public class MainMenuTransitions : MonoBehaviour
    {
        /// <summary>
        ///     Partial singleton pattern.
        /// </summary>
        public static MainMenuTransitions Instance { get; private set; }

        /// <summary>
        ///     Cached reference to <see cref="soundManager"/>.
        /// </summary>
        private SoundManager soundManager;

        #region Settings

        [Header("Transition Settings")]

        /// <summary>
        ///     Set the amount of time in seconds it takes for panel transitions.
        /// </summary>
        [SerializeField,
            Range(0.1f, 2f),
            Tooltip("Set the amount of time in seconds it takes for panel transitions.")]
        private float translationDuration = 1f;

        #endregion

        #region Panels

        /// <summary>
        ///     An array of panels in the main menu.
        /// </summary>
        [SerializeField,
            Tooltip("An array of panels in the main menu.")] 
        private Panel[] panels;

        #endregion

        #region Transition

        /// <summary>
        ///     Keeps track of how many coroutines have finished.
        /// </summary>
        /// <remarks>
        ///     Used durring <see cref="TransitionCoroutine(Panel, bool)"/>
        ///     to check if both shift coroutines have finished.
        /// </remarks>
        private int coroutinesFinished = 0;

        /// <summary>
        ///     Keeps track of which panel is currently being displayed.
        /// </summary>
        private Panel currentPanel;

        /// <summary>
        ///     Transitions from one panel to another.
        /// </summary>
        /// <remarks>
        ///     Creates a horizontal text sliding effect.
        /// </remarks>
        /// <param name="panel"> The panel to transition to. </param>
        /// <param name="toLeft"> If the panels are moving left. </param>
        internal void Transition(MenuPanels panel, bool toLeft)
        {
            StartCoroutine(TransitionCoroutine(panels[(int) panel], toLeft));
        }

        /// <summary>
        ///     <see cref="Transition(MenuPanels, bool)"/>
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator TransitionCoroutine(Panel toPanel, bool toLeft)
        {
            // Activate the panel but disable its interactivity.
            currentPanel.GameObject.SetActive(true);
            currentPanel.Canvas.interactable = false;
            foreach (MonoBehaviour m in currentPanel.MonoBehaviours)
            {
                m.enabled = false;
            }

            // Activate the panel but disable its interactivity.
            toPanel.GameObject.SetActive(true);
            toPanel.Canvas.interactable = false;
            foreach (MonoBehaviour m in toPanel.MonoBehaviours)
            {
                m.enabled = false;
            }

            // If the panels are moving left, place the toPanel to the right.
            // Otherwise place it to the left.
            // 1920 is the pixel width of the screen.
            if (toLeft)
            {
                toPanel.Transform.anchoredPosition = new Vector2(1920, 0);

                StartCoroutine(Shift(currentPanel.Transform, new Vector2(-1920, 0)));
                StartCoroutine(Shift(toPanel.Transform, new Vector2(0, 0)));
            }
            else
            {
                toPanel.Transform.anchoredPosition = new Vector2(-1920, 0);

                StartCoroutine(Shift(currentPanel.Transform, new Vector2(1920, 0)));
                StartCoroutine(Shift(toPanel.Transform, new Vector2(0, 0)));
            }

            // Wait until both shifts are finished.
            yield return new WaitUntil(() => (coroutinesFinished == 2));
            // Reset trigger.
            coroutinesFinished = 0;

            // Enable interactivity on toPanel
            toPanel.Canvas.interactable = true;
            foreach (MonoBehaviour m in toPanel.MonoBehaviours)
            {
                m.enabled = true;
            }

            // Disable fromPanel
            currentPanel.GameObject.SetActive(false);

            // Record the new current panel.
            currentPanel = toPanel;
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

        #endregion

        /// <summary>
        ///     Singleton check.
        /// </summary>
        private void Awake()
        {
            Instance = this;
        }

        /// <summary>
        ///     Play main menu sound and activate the correct panel.
        /// </summary>
        private void Start()
        {
            soundManager = SoundManager.Instance;
            soundManager.StopAllSound();
            soundManager.PlaySound(SoundNames.MusicMainMenu);
            
            // Turn off all the panels except the one to open on.
            foreach(Panel panel in panels)
            {
                panel.GameObject.SetActive(false);
            }
            currentPanel = panels[(int)GameData.MenuPanel];
            currentPanel.GameObject.SetActive(true);
            currentPanel.Transform.anchoredPosition = new Vector2(0, 0);

        }
    }
}
