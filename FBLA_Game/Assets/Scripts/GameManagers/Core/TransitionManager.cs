/*
 * Hanlin Zhang
 * Purpose: Manages transitions
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JonathansAdventure.GameManagers.Core
{
    public class TransitionManager : MonoBehaviour
    {
        // Method that sets a trigger in an animator
        public void transition(Animator animator, string trigger)
        {
            animator.SetTrigger(trigger);
        }

        // Method overload that also loads the next scene
        public IEnumerator transition(Animator animator, string trigger, bool nextScene)
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
