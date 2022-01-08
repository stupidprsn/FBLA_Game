using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {
    public gamePlayManager gamePlayManager;

    public void toGameScene(string toScene) {
        SceneManager.LoadScene(toScene);
        gamePlayManager.enabled = true;
    }

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);
    }

    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
}
