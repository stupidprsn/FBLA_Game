using UnityEngine;

public class homeScreen : MonoBehaviour {
    public void changePanel(GameObject toPanel) {
        toPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void quitButton() {
        Application.Quit();
    }
}
