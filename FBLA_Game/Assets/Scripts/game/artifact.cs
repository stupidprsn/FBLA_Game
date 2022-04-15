/*
 * Hanlin Zhang
 * Purpose: Functionality for the artifacts
 */

using UnityEngine;
using System.Collections;

public class artifact : MonoBehaviour {
    // References to the artifacts renderer and possible sprite images
    // Used to randomize the artifact image
    [SerializeField] private SpriteRenderer artifactRenderer;
    [SerializeField] private Sprite[] sprites = new Sprite[3];

    // Referance to the artifact's collider
    [SerializeField] private BoxCollider2D artifactCollider;

    // Referance to the player's collider
    private CapsuleCollider2D playerCollider;

    // Referance to the tile where the artifacts are placed
    private GameObject artifactPlacer;

    // Boolean to keep track of if it is already collected
    private bool collected = false;

    // Proceedure for animating the artifact going to it's place in the artifact placer
    // An IEnumerator is used because it allows the use of time
    private IEnumerator onCollect() {
        // Play the sound effect for collecting an artifact
        FindObjectOfType<SoundManager>().PlaySound("playerCollectArtifact");

        // Find the number of amulets already collected and total number of amulets in the level
        // This is used for placing the amulets
        int numOfAmulets = FindObjectOfType<closedDoor>().numOfAmulets;
        int currentAmulets = artifactPlacer.transform.childCount;

        // Make the artifact a child of the artifact placer
        transform.SetParent(artifactPlacer.transform);

        // Variables for setting up the animation
        Vector3 startPos = transform.localPosition;
        Vector3 finishPos;
        float t = 0;

        // Determine the final x coordinate of the sprite
        float x;
        if (numOfAmulets % 2 == 1) {
            x = 0f;
        } else {
            x = 0.08f;
        }

        x = x - (0.16f * (Mathf.FloorToInt(numOfAmulets / 2) - currentAmulets));

        // Use our calculated x value to determine the final position for our artifact
        finishPos = new Vector3(x, 0, 0);

        // Change the rendering layer so that the artifacts appear on top of everything while the animation plays
        artifactRenderer.sortingLayerName = "Foreground";

        // Move the artifact over t time
        while(t<1) {
            // Add the time that has passed
            t += Time.deltaTime / 0.3f;
            // Lerp is a unity method that linearly moves an object's position from the startPos to the finishPos
            transform.localPosition = Vector3.Lerp(startPos, finishPos, t);
            // Repeat this code segment
            yield return null;
        }

        // Change the rendering layer so that the artifact is behind the player
        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PlacedAmulets";
        // Check if all the artifacts have been placed
        artifactPlacer.transform.parent.Find("Door").GetComponent<closedDoor>().checkArtifacts();
    }

    void Start() {
        // Set referances to the player collider and artifact placer
        playerCollider = GameObject.Find("Jonathan").GetComponent<CapsuleCollider2D>();
        artifactPlacer = transform.parent.Find("Artifact Placer").gameObject;

        // Set teh artifact's sprite to a random one
        artifactRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    void Update() {
        // Check if the user is touching the artifact, has pressed w, and that the artifact isn't already collected
        if (Input.GetKeyDown("w") && artifactCollider.IsTouching(playerCollider) && !collected) {
            // Start the IEnumerator
            StartCoroutine(onCollect());
            // Update the collected function
            collected = true;
        }
    }
}
