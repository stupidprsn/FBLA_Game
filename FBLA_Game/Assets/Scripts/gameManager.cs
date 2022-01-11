using UnityEngine;
using UnityEngine.SceneManagement;

public class gameManager : MonoBehaviour {
    public gamePlayManager gamePlayManager;

    public void toGameScene(string toScene) {
        SceneManager.LoadScene(toScene);
        gamePlayManager.enabled = true;
        gamePlayManager.initiateVariables();
    }

    private void Awake() {
        DontDestroyOnLoad(this.gameObject);

        Screen.SetResolution(1920, 1080, false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
}
