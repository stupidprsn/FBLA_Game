/*
 * Hanlin Zhang
 * Purpose: Used in the UI, for any panel where the user can press space to continue
 */

using UnityEngine;

public class spaceToContinue : MonoBehaviour {
    // The panel we switch to.
    [SerializeField] private GameObject toPanel;

    private void Update() {
        // Check for space
        if (Input.GetKeyDown("space")) {
            // Activate the new panel, deactivate the old one, and play the audio. 
            toPanel.SetActive(true);
            gameObject.SetActive(false);
            FindObjectOfType<SoundManager>().PlaySound("UISpacebar");
        }
    }
}
