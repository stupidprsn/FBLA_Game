/*
 * Hanlin Zhang
 * Purpose: Door script for when all the artifacts are collected
 */

using UnityEngine;

public class closedDoor : MonoBehaviour {
    // Number of amulets in the stage
    // Value set in inspecter depending on the level
    public int numOfAmulets;

    // Referances to objects and scripts
    private GameObject artifactPlacer;
    [SerializeField] private SpriteRenderer doorPieceRenderer;
    [SerializeField] private openedDoor openedDoorScript;

    private void Start() {
        // Set the referance to the artifact placer
        artifactPlacer = transform.parent.Find("Artifact Placer").gameObject;
    }

    // Public method that is called by the artifacts when the player collects them
    public void checkArtifacts() {
        // Checks if all of the artifacts in the level have been collected
        if (artifactPlacer.transform.childCount == numOfAmulets) {
            // Turn off the sprite for the door so that the door is open
            doorPieceRenderer.enabled = false;
            // Play the sound for opening the door
            FindObjectOfType<soundManager>().PlaySound("gameDoorOpen");
            // Turn on the script for opening the door
            openedDoorScript.enabled = true;
        }
    }
}