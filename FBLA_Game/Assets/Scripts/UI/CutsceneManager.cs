/*
 * Hanlin Zhang
 * Purpose: Trigger code to run when the cutscene is loaded.
 *          This script also manages some code that runs
 *          when the game first loads since the cutscene is
 *          loaded first.
 */

using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour {
    [Header("Object/Prefab References")]
    [SerializeField] private GameObject gameManagerPreset;
    [SerializeField] private GameObject fileManagerPreset;
    [SerializeField] private VideoPlayer videoPlayer;
    [SerializeField] private Animator animator;

    private void Awake() {
        if(FindObjectOfType<GameManager>() == null) {
            GameObject gameManager = Instantiate(gameManagerPreset);
            Instantiate(fileManagerPreset, gameManager.transform);
        }

        SetSettings();
    }

    private void SetSettings() {
        FileManager fileManager = FindObjectOfType<FileManager>();

        fileManager.LoadDefaults();
        UserSettings userSettings = fileManager.LoadUserSettings();
        FindObjectOfType<GameManager>().SetSettings(userSettings);

        if (userSettings.playCutScene) {
            videoPlayer.SetDirectAudioVolume(0, userSettings.volume);
            videoPlayer.Play();
            videoPlayer.loopPointReached += onVideoEnd;
        } else {
            transition();
        }
    }

    private void onVideoEnd(VideoPlayer vp) {
        transition();
    }

    private void transition() {
        StartCoroutine(FindObjectOfType<TransitionManager>().transition(animator, "Exit", true));
    }
}
