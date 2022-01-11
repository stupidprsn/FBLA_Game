using UnityEngine;
using TMPro;

public class windowToggle : MonoBehaviour {
    public TMP_Text text;

    private bool fullScreen = false;
    private bool fullHD = true;

    private void updateScreen() {
        if(fullHD) {
            Screen.SetResolution(1920, 1080, fullScreen);
        } else {
            Screen.SetResolution(1280, 720, fullScreen);
        }
    }

    void Update() {
        if(Input.GetKeyDown("tab")) {
            fullHD = !fullHD;

            if(fullHD) {
                text.text = "Press [tab] to toggle to 1080p (if the game window is too small)\n\nPress[shift] to toggle fullscreen";
            } else {
                text.text = "Press [tab] to toggle to 720p (if you cannot see the full game)\n\nPress[shift] to toggle fullscreen";
            }

            updateScreen();
        }

        if(Input.GetKeyDown("left shift") || Input.GetKeyDown("right shift")) {
            fullScreen = !fullScreen;
            updateScreen();
        }
    }
}
