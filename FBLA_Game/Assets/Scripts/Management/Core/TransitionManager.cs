using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace JonathansAdventure.Transitions
{
    /// <summary>
    ///     Manages transitions between scenes.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/24/2022
    /// </remarks>
    public class TransitionManager : Singleton<TransitionManager>
    {
        /// <summary>
        ///     Plays a crossfade and loads the scene.
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="CrossFadeCoroutine(int)"/>.
        /// </remarks>
        /// <param name="scene"> The scene to load. </param>
        public void CrossFade(Scenes scene)
        {
            StartCoroutine(CrossFadeCoroutine((int)scene));
        }

        /// <summary>
        ///     Overload of <see cref="CrossFade(Scenes)"/> in which
        ///     the next scene is loaded.
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="CrossFadeCoroutine(Scenes)"/>.
        /// </remarks>
        /// <param name="animator"> Reference to the <see cref="Animator"/> that has the animation. </param>
        public void CrossFade()
        {
            StartCoroutine(CrossFadeCoroutine(SceneManager.GetActiveScene().buildIndex + 1));
        }

        /// <summary>
        ///     <see cref="CrossFade(Scenes)"/>.
        /// </summary>
        private IEnumerator CrossFadeCoroutine(int buildIndex)
        {
            Animator animator = GameObject.FindWithTag("Transition").GetComponent<Animator>();
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
