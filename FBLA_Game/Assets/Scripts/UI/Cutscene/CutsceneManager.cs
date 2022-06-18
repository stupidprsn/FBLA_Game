using UnityEngine;
using UnityEngine.Video;
using JonathansAdventure.Sound;

namespace JonathansAdventure.UI.Cutscene
{
    /// <summary>
    ///     Trigger Code to run when the cutscene is loaded.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/12/2022
    /// </remarks>
    public class CutsceneManager : MonoBehaviour
    {

        #region References

        [Header("Object/Prefab References")]
        [SerializeField] private VideoPlayer backgroundVideoPlayer;
        [SerializeField] private Animator animator;
        [SerializeField] private CutsceneText text;
        private SoundManager soundManager;

        #endregion

        private void Start()
        {
            soundManager = SoundManager.Instance;
            StartCutscene();
        }

        private void StartCutscene()
        {
            backgroundVideoPlayer.Play();
            soundManager.PlaySound(SoundNames.MusicCutscene);
            text.StartText();
        }

        internal void Transition()
        {
            soundManager.StopAllSound();
            FindObjectOfType<TransitionManager>().CrossFade(animator);
        }
    }

}
