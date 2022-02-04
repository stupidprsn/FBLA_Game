/*
 * Hanlin Zhang
 * Purpose: Misc methods used to manage the game
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class gameManager : MonoBehaviour {
    public static gameManager singletonCheck;

    // Referance to other scripts that manage gameplay and sound
    // Referances are set in meta data
    [SerializeField] private gamePlayManager gamePlayManager;
    [SerializeField] private twoPlayerManager twoPlayerManager;
    [SerializeField] private soundManager soundManager;

    // Boolean to keep track of whether or not the game is in fullscreen.
    private bool fullScreen = false;

    // Read by onEnterMainMenu to know what panel to load
    public int mainMenuPanel = 0;

    // Method for returning to main menu.
    // Takes in one parameter, panel, which dictates which panel to return to.
    public void toMainMenu(int panel) {
        gamePlayManager.DEBUG.SetText("f");
        gamePlayManager.enabled = false;
        mainMenuPanel = panel;
        SceneManager.LoadScene(0);
    }

    private void Awake() {
        if(singletonCheck == null) {
            singletonCheck = this;
        } else {
            Destroy(gameObject);
        }

        // Keep the game manager in all scenes
        DontDestroyOnLoad(this.gameObject);

        // Set the screen resolution and lock the mouse
        Screen.SetResolution(1920, 1080, false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update() {
        // Allow the application to quit with the esc key
        if(Input.GetKeyDown("escape")) {
            Application.Quit();
        }

        // Toggle fullscreen with the shift keys
        if (Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift")) {
            fullScreen = !fullScreen;
            Screen.SetResolution(1920, 1080, fullScreen);
        }
    }
}
