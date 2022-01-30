/*
 * Hanlin Zhang
 * Purpose: To execute code when the main menu scene is loaded
 */ 

using UnityEngine;

public class onEnterMainMenu : MonoBehaviour {
    private void Start() {
        // Find the sound manager
        soundManager soundManager = FindObjectOfType<soundManager>();
        // Stop all previous music and start main menu music
        soundManager.stopAllSound();
        soundManager.PlaySound("musicMainMenu");
        // Destroy this object after the script is finished
        Destroy(gameObject);
    }
}
