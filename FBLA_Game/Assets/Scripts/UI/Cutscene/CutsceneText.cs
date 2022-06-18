using System.Collections;
using UnityEngine;
using TMPro;
using JonathansAdventure.Sound;

namespace JonathansAdventure.UI.Cutscene
{
    /// <summary>
    ///     Creates the typing effect for the text in the cutscene.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/10/2020
    /// </remarks>
    [RequireComponent(typeof(TMP_Text))]
    public class CutsceneText : MonoBehaviour
    {
        #region Settings

        [Header("Cutscene Text")]

        [   
            SerializeField, 
            Range(0.01f, 1f),
            Tooltip("Set the amount of time it takes each character to type.")
        ]
        private float typeTime;

        [
            SerializeField, 
            Range(0f, 5f),
            Tooltip("Set the amount of time before the first message is played")
        ]
        private float buffer;

        [
            SerializeField, 
            TextArea, 
            Tooltip("Set the text to display")
        ]
        private string[] texts;

        #endregion

        #region References

        [Header("Object/Prefab References")]
        [SerializeField] private TMP_Text text;
        [SerializeField] private CutsceneManager cutsceneManager;
        [SerializeField] private ContinueCue continueCue;
        [SerializeField] private SkipMessage skipMessage;

        #endregion

        private SoundManager soundManager;

        /// <summary>
        ///     The index of <see cref="texts"/> that is currently being displayed.
        /// </summary>
        private int index = 0;
        /// <summary>
        ///     Keeps track of when to move onto the next <see cref="texts"/>.
        /// </summary>
        private bool canContinue = false;

        /// <summary>
        ///     Starts typing out the cutscene text.
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="MainCoroutine"/>.
        /// </remarks>  
        internal void StartText()
        {
            StartCoroutine(MainCoroutine());
        }

        /// <summary>
        ///     Main Method for typing out animation.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator MainCoroutine()
        {
            // Wait the buffer time.
            yield return new WaitForSeconds(buffer);

            // Checks that the program has not reached the last text.
            while (index < texts.Length)
            {
                // Starts the type out animation.
                StartCoroutine(TypeOutAnimationCoroutine());

                // Wait for the animation to signal that the text has finished typing.
                yield return new WaitUntil(() => canContinue);
                // Move onto the next text.
                index++;
                // Show the cue to tell the user to press space to continue.
                continueCue.FadeInText();

                // Tell the skip message not to pop up.
                skipMessage.SetVoidSpace(true);

                /*
                 * Wait for the user to press space.
                 * ** canContinue needs to be checked again so that just pressing space
                 *    alone won't trigger the next step
                 */
                yield return new WaitUntil(() => canContinue && Input.GetKeyDown(KeyCode.Space));
                soundManager.PlaySound(SoundNames.SpaceContinue);
                
                // Allow the skip message to pop up again.
                skipMessage.SetVoidSpace(false);

                // Reset the signal.
                canContinue = false;

                // Hide the cue
                continueCue.FadeOutText();
            }

            // Exit the cutscene
            cutsceneManager.Transition();
        }

        /// <summary>
        /// Animation for the typing out text effect.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator TypeOutAnimationCoroutine()
        {
            // Start with blank text.
            text.text = "";

            // Iterate through each character in the string.
            foreach (char c in texts[index])
            {
                // Don't wait time if it's a newline character.
                if (c.Equals("\n"))
                {
                    text.text += c;
                } else
                {
                    // Wait time for all other characters.
                    text.text += c;
                    yield return new WaitForSeconds(typeTime);
                }
            }

            // Signal to Main that the animation is done.
            canContinue = true;
        }

        private void Awake()
        {
            soundManager = SoundManager.Instance;
        }
    }

}