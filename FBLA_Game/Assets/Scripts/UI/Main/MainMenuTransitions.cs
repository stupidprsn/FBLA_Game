using UnityEngine;
using System.Collections;

public class MainMenuTransitions : MonoBehaviour {
    private float translationDuration = 1f;

    private GameObject mainMenu;

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
}
