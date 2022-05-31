using UnityEngine;
using TMPro;
using System.Collections;

[RequireComponent(typeof(TMP_Text), typeof(CanvasGroup))]
public class ContinueCue : MonoBehaviour {
    [Header("Settings")]

    [Tooltip("Set the amount of time in seconds it takes for the text to fade in")]
    [Range(0f, 1f)] [SerializeField] private float fadeDuration;

    [Tooltip("Set the amount of time in seconds for the text to flash from one frame to the next")]
    [Range(0f, 1f)] [SerializeField] private float blinkSpeed;

    [Tooltip("Set the different frames of text that the program flashes between")]
    [SerializeField] private string[] texts;

    [Header("Object Reference")]

    [SerializeField] private TMP_Text text;
    [SerializeField] private CanvasGroup canvasGroup;

    private bool doBlink;

    public void FadeInText() {
        StartCoroutine(FadeIn());
    }

    public void FadeOutText() {
        canvasGroup.alpha = 0f;
        doBlink = false;
    }

    private IEnumerator FadeIn() {
        for(float i = 0; i < fadeDuration; i += Time.deltaTime) {
            canvasGroup.alpha = i / fadeDuration;
            yield return null;
        }

        doBlink = true;
        StartCoroutine(Blink());
    }

    private IEnumerator Blink() {
        while (doBlink) {
            foreach (string nextText in texts) {
                text.text = nextText;
                yield return new WaitForSeconds(blinkSpeed);
            }
            yield return null; 
        }
    }
}
