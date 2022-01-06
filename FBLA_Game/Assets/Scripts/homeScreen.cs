using UnityEngine;
using UnityEngine.SceneManagement;

public class homeScreen : MonoBehaviour {
    public void playButton(string toScene) {
        SceneManager.LoadScene(toScene);
    }

    public void changePanel(GameObject toPanel) {
        toPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void quitButton() {
        Application.Quit();
    }
}