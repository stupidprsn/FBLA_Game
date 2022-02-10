/*
 * Hanlin Zhang
 * Purpose: Functionality for the final artifact that wins the game
 *          It is very similar to the regular artifact script
 */

using UnityEngine;
using System.Collections;

public class finalArtifact : MonoBehaviour {

    // Referances to the artifact and player's collider
    [SerializeField] private BoxCollider2D artifactCollider;

    private CapsuleCollider2D playerCollider;

    // Proceedure for animating the artifact going to it's place in the artifact placer
    // An IEnumerator is used because it allows the use of time
    private IEnumerator onCollect() {
        // Play the sound effect for collecting an artifact
        FindObjectOfType<soundManager>().PlaySound("gameLevelWin");

        // Variables for setting up the animation
        Vector3 startPos = transform.localPosition;
        Vector3 finishPos = new Vector3(0, 3, 0);
        float t = 0;

        // Change the rendering layer so that the artifacts appear on top of everything while the animation plays
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";
        
        // Move the artifact over t time
        while (t < 1) {
            // Add the time that has passed
            t += Time.deltaTime / 0.3f;
            // Lerp is a unity method that linearly moves an object's position from the startPos to the finishPos
            transform.localPosition = Vector3.Lerp(startPos, finishPos, t);
            // Repeat this code segment
            yield return null;
        }

        // Call function for winning the game
        FindObjectOfType<gamePlayManager>().winGame();
    }

    void Start() {
        // Set a referance to the player collider
        playerCollider = GameObject.Find("Jonathan").GetComponent<CapsuleCollider2D>();
    }

    void Update() {
        // Check if the user is touching the artifact and has pressed w
        if (artifactCollider.IsTouching(playerCollider) && Input.GetKeyDown("w")) {
            // Start the IEnumerator
            StartCoroutine(onCollect());
        }
    }
}
