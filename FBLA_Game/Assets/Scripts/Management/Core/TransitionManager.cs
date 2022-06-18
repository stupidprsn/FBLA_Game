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
    ///     Last Modified: 6/13/2022
    /// </remarks>
    public class TransitionManager : Singleton<TransitionManager>
    {
        /// <summary>
        ///     Plays a crossfade and loads the scene.
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="CrossFadeCoroutine(Animator, int)"/>.
        /// </remarks>
        /// <param name="animator"> Reference to the <see cref="Animator"/> that has the animation. </param>
        /// <param name="buildIndex"> The <see cref="Scene.buildIndex"/> of the scene to load. </param>
        public void CrossFade(Animator animator, int buildIndex)
        {
            StartCoroutine(CrossFadeCoroutine(animator, buildIndex));
        }

        /// <summary>
        ///     Overload of <see cref="CrossFade(Animator, int)"/> in which
        ///     the next scene is loaded.
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="CrossFadeCoroutine(Animator, int)"/>.
        /// </remarks>
        /// <param name="animator"> Reference to the <see cref="Animator"/> that has the animation. </param>
        public void CrossFade(Animator animator)
        {
            StartCoroutine(CrossFadeCoroutine(animator, SceneManager.GetActiveScene().buildIndex + 1));
        }

        /// <summary>
        ///     <see cref="CrossFade(Animator, string, int)"/>.
        /// </summary>
        private IEnumerator CrossFadeCoroutine(Animator animator, int buildIndex)
        {
            // Play animation.
            animator.SetTrigger("Exit");
        
            // Wait until the animation is done.
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
            SceneManager.LoadScene(buildIndex);
        }

        private void Awake()
        {
            SingletonCheck(this);
        }
    }

}
