/*
 * Hanlin Zhang
 * Purpose: Manages transitions
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionManager : MonoBehaviour {
    // Method that sets a trigger in an animator
    public void transition(Animator animator, string trigger) {
        animator.SetTrigger(trigger);
    }

    // Method overload that also loads the next scene
    public void transition(Animator animator, string trigger, bool nextScene) {
        animator.SetTrigger(trigger);
        if (nextScene) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }
    }
}
