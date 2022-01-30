/*
 * Hanlin Zhang
 * Purpose: Used in the UI, for any panel where the user can press space to continue
 */

using UnityEngine;

public class spaceToContinue : MonoBehaviour {
    // The panel we switch to. It is set in the meta data. 
    public GameObject toPanel;

    // Check for space
    private void Update() {
        if (Input.GetKeyDown("space")) {
            // Activate the new panel, deactivate the old one, and play the audio. 
            toPanel.SetActive(true);
            gameObject.SetActive(false);
            FindObjectOfType<soundManager>().PlaySound("UISpacebar");
        }
    }
}
