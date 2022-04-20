using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class CutsceneText : MonoBehaviour {
    [Header("Object/Prefab References")]
    [SerializeField] private TMP_Text text;
    [SerializeField] private CutsceneManager cutsceneManager;

    [Header("Cutscene Text")]

    [Range(0.01f, 1f)]
    [Tooltip("Set the amount of time it takes each character to type.")]
    [SerializeField]
    private float typeTime;

    [Range(0f, 5f)]
    [Tooltip("Set the amount of time before the first message is played")]
    [SerializeField]
    private float buffer = 2f;

    [Tooltip("Set the text to display")]
    [TextArea]
    [SerializeField]
    private string[] texts;

    private int index = 0;
    private bool canContinue = false;

    public IEnumerator StartText() {
        yield return new WaitForSeconds(buffer);

        while (index < texts.Length) {
            StartCoroutine(DisplayText());
            yield return new WaitUntil(() => canContinue);
            canContinue = false;
            index++;
            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Space));
        }

        cutsceneManager.Transition();
    }

    private IEnumerator DisplayText() {
        text.text = "";

        foreach (char c in texts[index]) {
            if (c.Equals("\n")) {
                text.text += c;
                Debug.Log("e");
            } else {
                text.text += c;
                yield return new WaitForSeconds(typeTime);
            }
        }

        canContinue = true;
    }
}
