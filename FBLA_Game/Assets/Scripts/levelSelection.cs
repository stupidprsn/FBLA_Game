using UnityEngine;
using UnityEngine.SceneManagement;

public class levelSelection : MonoBehaviour {
    public GameObject homeScreen;
    public void levelLoader(string level) {
        SceneManager.LoadScene(level);
    }

    public void toHomeScreen() {
        homeScreen.SetActive(true);
        gameObject.SetActive(false);
    }
}
