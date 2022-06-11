using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JonathansAdventure
{
    /// <summary>
    ///     Manages transitions between scenes.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/8/2022
    /// </remarks>
    public class TransitionManager : MonoBehaviour
    {
        // Method that sets a trigger in an animator
        /// <summary>
        ///     Plays a transition.
        /// </summary>
        /// <param name="animator"> Reference to the <see cref="Animator"/> that has the animation. </param>
        /// <param name="trigger"> Name of the trigger used to start the animation. </param>
        public void Transition(Animator animator, string trigger)
        {
            animator.SetTrigger(trigger);
        }

        /// <summary>
        ///     Overload that plays a transition and loads the next sceen.
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="TransitionCoroutine(Animator, string, bool)"/>.
        /// </remarks>
        /// <param name="animator"> Reference to the <see cref="Animator"/> that has the animation. </param>
        /// <param name="trigger"> Name of the trigger used to start the animation. </param>
        /// <param name="nextScene"> If the next scene should be loaded </param>
        /// <returns> null </returns>
        public void Transition(Animator animator, string trigger, bool nextScene)
        {
            StartCoroutine(TransitionCoroutine(animator, trigger, nextScene));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="animator"></param>
        /// <param name="trigger"></param>
        /// <param name="nextScene"></param>
        /// <returns></returns>
        private IEnumerator TransitionCoroutine(Animator animator, string trigger, bool nextScene)
        {
            animator.SetTrigger(trigger);
            if (nextScene)
            {
                yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
            }
        }


    }

}
