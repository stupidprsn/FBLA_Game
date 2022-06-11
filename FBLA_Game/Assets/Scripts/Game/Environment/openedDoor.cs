/*
 * Hanlin Zhang
 * Purpose: Script for when the user has collected all the artifacts
 */
using UnityEngine;

namespace JonathansAdventure.Game.Normal
{
    public class openedDoor : MonoBehaviour
    {
        // Referances to the player and door's colliders
        private CapsuleCollider2D playerCollider;
        [SerializeField] private BoxCollider2D doorCollider;

        private void Start()
        {
            // Set a referance to the player's collider
            playerCollider = GameObject.Find("Jonathan").GetComponent<CapsuleCollider2D>();
        }

        private void Update()
        {
            // Check if the player is touching the door and has pressed w
            if (playerCollider.IsTouching(doorCollider) && (Input.GetKeyDown("w") || Input.GetKeyDown("up")))
            {
                // Call the game play manager's method for winning a stage
                FindObjectOfType<GameManager>().winStage();
            }
        }
    }

}
