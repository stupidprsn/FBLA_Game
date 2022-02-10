/*
 * Hanlin Zhang
 * Purpose: Used to tell the game play manager when a new scene is loaded
 */
using UnityEngine;

public class onEnterLevel : MonoBehaviour {
    // Music to be played
    // Set in unity inspector depending on what music should be played for the current stage
    [SerializeField] private string music;
    private void Start() {
        // Call the method in the game play manager for when a new stage is loaded
        FindObjectOfType<gamePlayManager>().onStageEnter(music);
    }
}
