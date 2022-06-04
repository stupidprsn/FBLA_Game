/*
 * Hanlin Zhang
 * Purpose: Trigger code to run when the cutscene is loaded.
 *          This script also manages some code that runs
 *          when the game first loads since the cutscene is
 *          loaded first.
 */

using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class CutsceneManager : MonoBehaviour {
    [Tooltip("Set the amount of time the message confirming the user's choice to skip the cutscene shows up for.")]
    [Range(0f, 10f)]
    [SerializeField] private float skipMsgTime;

    [Header("Object/Prefab References")]
    [SerializeField] private GameObject gameManagerPreset;
    [SerializeField] private GameObject fileManagerPreset;
    [SerializeField] private VideoPlayer backgroundVideoPlayer;
    [SerializeField] private Animator animator;
    [SerializeField] private CutsceneText text;
    [SerializeField] private CanvasGroup skipText;

    private UserSettings userSettings;
    private bool skip = false;

    private SoundManager soundManager;

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

    private void Update() {
        if(Input.GetKeyDown(KeyCode.Return)) {
            if(skip) {
                Transition();
            } else {
                skip = true;
                skipText.alpha = 1;
                StartCoroutine(confirmSkip());
            }
        }
    }

    private IEnumerator confirmSkip() {
        yield return new WaitForSeconds(skipMsgTime);
        skip = false;
        skipText.alpha = 0;
    }

    private void SetSettings() {
        FileManager fileManager = FindObjectOfType<FileManager>();

        fileManager.LoadDefaults();
        userSettings = fileManager.UserSettingsData.load();
        FindObjectOfType<GameManager>().SetSettings(userSettings);
    }

    private void StartCutscene() {
        backgroundVideoPlayer.Play();
        soundManager = FindObjectOfType<SoundManager>();
        soundManager.PlaySound("musicCutscene");
        StartCoroutine(text.StartText());
    }

    public void Transition() {
        soundManager.StopAllSound();
        StartCoroutine(FindObjectOfType<TransitionManager>().transition(animator, "Exit", true));
    }
}
