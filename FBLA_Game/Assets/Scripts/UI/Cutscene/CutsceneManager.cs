using UnityEngine;
using UnityEngine.Video;
using System.Collections;

namespace JonathansAdventure.UI.Cutscene
{
    /// <summary>
    ///     Trigger Code to run when the cutscene is loaded.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/10/2022
    /// </remarks>
    public class CutsceneManager : MonoBehaviour
    {

        #region References

        [Header("Object/Prefab References")]
        [SerializeField] private GameObject gameManagerPreset;
        [SerializeField] private GameObject fileManagerPreset;
        [SerializeField] private VideoPlayer backgroundVideoPlayer;
        [SerializeField] private Animator animator;
        [SerializeField] private CutsceneText text;

        #endregion

        private void Start()
        {
            StartCutscene();
        }



        private void SetSettings()
        {
            FileManager fileManager = FindObjectOfType<FileManager>();

            fileManager.LoadDefaults();
            userSettings = fileManager.UserSettingsData.load();
            FindObjectOfType<GameManager>().SetSettings(userSettings);
        }

        private void StartCutscene()
        {
            backgroundVideoPlayer.Play();
            soundManager = FindObjectOfType<SoundManager>();
            soundManager.PlaySound("musicCutscene");
            StartCoroutine(text.StartText());
        }

        internal void Transition()
        {
            soundManager.StopAllSound();
            StartCoroutine(FindObjectOfType<TransitionManager>().transition(animator, "Exit", true));
        }
    }

}
