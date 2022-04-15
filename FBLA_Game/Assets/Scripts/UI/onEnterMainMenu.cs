/*
 * Hanlin Zhang
 * Purpose: To execute code when the main menu scene is loaded
 */ 

using UnityEngine;

public class onEnterMainMenu : MonoBehaviour {
    // Referance to the 5 panels in the menu
    // [0] - Title Screen
    // [1] - Home Screen
    // [2] - Instructions Screen
    // [3] - Credits Screen
    // [4] - Leaderborad Screen
    [SerializeField] private GameObject[] panels = new GameObject[5];

    private void Start() {
        // Check gameManager for which panel we should open to. Default is title screen.
        panels[FindObjectOfType<GameManager>().mainMenuPanel].SetActive(true);

        // Find the sound manager
        SoundManager soundManager = FindObjectOfType<SoundManager>();
        // Stop all previous music and start main menu music
        soundManager.StopAllSound();
        soundManager.PlaySound("musicMainMenu");
    }
}
