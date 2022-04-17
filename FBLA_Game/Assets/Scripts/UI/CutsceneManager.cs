/*
 * Hanlin Zhang
 * Purpose: Trigger code to run when the cutscene is loaded.
 *          This script also manages some code that runs
 *          when the game first loads since the cutscene is
 *          loaded first.
 */

using System.Collections;
using UnityEngine;
using UnityEngine.Video;

public class CutsceneManager : MonoBehaviour {
    [Header("Object/Prefab References")]
    [SerializeField] private GameObject gameManagerPreset;
    [SerializeField] private GameObject fileManagerPreset;
    [SerializeField] private VideoPlayer backgroundVideoPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private VideoClip[] textClips;

    [SerializeField] private VideoPlayer currentTextPlayer;
    [SerializeField] private VideoPlayer nextTextPlayer;
    private int videoIndex = 0;
    private UserSettings userSettings;

    private void Awake() {
        if(FindObjectOfType<GameManager>() == null) {
            Instantiate(gameManagerPreset);
            GameObject fileManager = Instantiate(fileManagerPreset);
            DontDestroyOnLoad(fileManager);
        }

        SetSettings();
    }

    private void Start() {
        if (userSettings.playCutScene)
        {
            StartPlayVideo();
        }
        else
        {
            Transition();
        }
    }

    private void SetSettings() {
        FileManager fileManager = FindObjectOfType<FileManager>();

        fileManager.LoadDefaults();
        userSettings = fileManager.LoadUserSettings();
        FindObjectOfType<GameManager>().SetSettings(userSettings);
    }

    private void StartPlayVideo() {
        currentTextPlayer.clip = textClips[videoIndex];
        videoIndex++;
        currentTextPlayer.Prepare();
    }

    private IEnumerator IsVideoPrepared() {
        while (!currentTextPlayer.isPrepared) {
            yield return null;
        }
        yield return;
    }

    private void Transition() {
        StartCoroutine(FindObjectOfType<TransitionManager>().transition(animator, "Exit", true));
    }
}
