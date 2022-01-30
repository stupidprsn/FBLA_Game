/*
 * Hanlin Zhang
 * Purpose: Misc methods used to manage the game
*/

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class gameManager : MonoBehaviour {
    // Referance to other scripts that manage gameplay and sound
    // Referances are set in meta data
    public gamePlayManager gamePlayManager;
    public soundManager soundManager;

    public static List<Rank> theRankings = new List<Rank>();

    // Boolean to keep track of whether or not the game is in fullscreen.
    private bool fullScreen = false;

    // Method for loading a game scene, it is used by the "play" and "2 players" buttons.
    public void toGameScene(string toScene) {
        SceneManager.LoadScene(toScene);
        gamePlayManager.enabled = true;
        gamePlayManager.initiateVariables();
        soundManager.stopSound("musicMainMenu");
        soundManager.PlaySound("musicNormalLevel");
    }

    public void toMainMenu() {
        SceneManager.LoadScene(0);
        soundManager.stopSound("musicNormalLevel");
    }

    public void newEntry(string name, int score) {
        theRankings.Add(new Rank(name, score));
        foreach(var item in theRankings) {
            Debug.Log(item.name);
        }
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

        if (Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift")) {
            fullScreen = !fullScreen;
            Screen.SetResolution(1920, 1080, fullScreen);
        }
    }
}
