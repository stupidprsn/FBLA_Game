/*
 * Hanlin Zhang
 * Purpose: To execute code when the main menu scene is loaded
 */ 

using UnityEngine;
using System.Collections;

public class MainMenuManager : MonoBehaviour {
    [Header("Transition Settings")]

    [Tooltip("Set the amount of time in seconds it takes for panel transitions.")]
    [Range(0.5f, 2f)]
    [SerializeField]
    private float translationDuration = 1f;


    [Header("Object Reference")]

    [SerializeField] private GameObject mainMenu;
    [SerializeField] private GameObject[] panels = new GameObject[5];

    public void PanelTransition(GameObject fromPanel, GameObject toPanel, Vector2 direction) {
        fromPanel.SetActive(true);
        toPanel.SetActive(true);

        RectTransform toPanelTransform = toPanel.GetComponent<RectTransform>();
        toPanelTransform.anchoredPosition = new Vector2(0, -1080);

        StartCoroutine(translate(toPanelTransform, direction));
        StartCoroutine(translate(fromPanel.GetComponent<RectTransform>(), direction));

        fromPanel.SetActive(false);
    }

    private IEnumerator translate(RectTransform panel, Vector2 direction) {
        Vector2 startPos = panel.anchoredPosition;
        Vector2 endPos = direction;

        endPos.y *= 1080;

        float t = 0;

        while (t < translationDuration) {
            panel.anchoredPosition = Vector2.Lerp(startPos, endPos, t / translationDuration);
            t += Time.deltaTime;
            yield return null;
        }

        panel.anchoredPosition = endPos;
    }

    private void Start() {
        // Check gameManager for which panel we should open to. Default is title screen.
        panels[(int)FindObjectOfType<GameManager>().mainMenuPanel].SetActive(true);

        // Find the sound manager
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        // Stop all previous music and start main menu music
        soundManager.StopAllSound();
        soundManager.PlaySound("musicMainMenu");
    }
}
