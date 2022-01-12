/*
 * Hanlin Zhang
 * Purpose: Start the main menu music and allow the user to toggle fullscreen
*/

using UnityEngine;

public class titleScreen : MonoBehaviour {
    // Boolean to keep track of whether or not the game is in fullscreen.
    private bool fullScreen = false;

    // Start the main menu music
    private void Start() {
        FindObjectOfType<soundManager>().PlaySound("musicMainMenu");
    }

    // Every frame, check if any shift key is pressed, and if it is, update whether the game is in fullscreen or not
    private void Update() {
        if(Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift")) {
            fullScreen = !fullScreen;
            Screen.SetResolution(1920, 1080, fullScreen);
        }
    }
}
