/*
 * Hanlin Zhang
 * Purpose: To execute code when the main menu scene is loaded
 */ 

using UnityEngine;

public class onEnterMainMenu : MonoBehaviour {
    // Referance to the 5 panels in the menu
    // [0] 
    [SerializeField] private GameObject[] panels = new GameObject[5];

    private void Start() {
        // Check gameManager for which panel we should open to. Default is title screen.
        panels[FindObjectOfType<gameManager>().mainMenuPanel].SetActive(true);

        // Find the sound manager
        soundManager soundManager = FindObjectOfType<soundManager>();
        // Stop all previous music and start main menu music
        soundManager.stopAllSound();
        soundManager.PlaySound("musicMainMenu");
    }
}
