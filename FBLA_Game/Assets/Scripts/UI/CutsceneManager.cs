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
using TMPro;

public class CutsceneManager : MonoBehaviour {
    [Header("Object/Prefab References")]
    [SerializeField] private GameObject gameManagerPreset;
    [SerializeField] private GameObject fileManagerPreset;
    [SerializeField] private VideoPlayer backgroundVideoPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private CutsceneText text;

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
        if (userSettings.playCutScene) {
            StartCutscene();
        } else {
            Transition();
        }
    }

    private void SetSettings() {
        FileManager fileManager = FindObjectOfType<FileManager>();

        fileManager.LoadDefaults();
        userSettings = fileManager.LoadUserSettings();
        FindObjectOfType<GameManager>().SetSettings(userSettings);
    }

    private void StartCutscene() {
        backgroundVideoPlayer.Play();
        StartCoroutine(text.StartText());
    }

    public void Transition() {
        StartCoroutine(FindObjectOfType<TransitionManager>().transition(animator, "Exit", true));
    }
}
