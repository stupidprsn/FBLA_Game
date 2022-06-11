/*
 * Hanlin Zhang
 * Purpose: AI for the snake enemies
 */

using UnityEngine;

namespace JonathansAdventure.Game.Normal
{
    public class snakeAI : MonoBehaviour
    {
        // References to gameobjects
        public GameObject artifact;
        public LayerMask boundary;

        // Variables for moving the snake
        public bool facingRight;
        public float speed;

        // Check if the snake can go forward
        private bool canGoForward()
        {
            // Check if there's anything in the forward direction
            Vector2 forwardVector2;
            // Check if there's anything below the snake
            Vector2 downVector2;
            // The direction the snake is facing
            Vector2 forwardDirection;


            // Flip our variables depending on if the snake is facing left or right
            if (facingRight)
            {
                forwardVector2 = new Vector2(transform.position.x + 1.05f, transform.position.y - 0.45f);
                downVector2 = new Vector2(transform.position.x + 0.9f, transform.position.y - 0.7f);
                forwardDirection = Vector2.right;
            } else
            {
                forwardVector2 = new Vector2(transform.position.x - 0.7f, transform.position.y - 0.45f);
                downVector2 = new Vector2(transform.position.x - 0.9f, transform.position.y - 0.7f);
                forwardDirection = Vector2.left;
            }

            // Check if there's anything in front of or bellow the snake
            RaycastHit2D[] forwardHits = Physics2D.RaycastAll(forwardVector2, forwardDirection, 0.3f, boundary);
            RaycastHit2D[] downHits = Physics2D.RaycastAll(downVector2, Vector2.down, 0.75f);

            // For debugging
            Debug.DrawRay(forwardVector2, Vector2.left * 0.3f, Color.green);
            Debug.DrawRay(downVector2, Vector2.down * 0.75f, Color.green);

            // Checks if there's nothing in front of the snake and that there's a platform in front of the snake
            if (forwardHits.Length == 0 && downHits.Length != 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        // Flip the direction the snake is looking
        private void changeDirection()
        {
            transform.Rotate(new Vector3(0, 180, 0));
            facingRight = !facingRight;
        }

        // Checks if the player has hit the snake
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Checks if it was the player
            if (collision.gameObject.tag == "Player")
            {
                // Checks if the player is on top of or to the side of the snake
                // If the player is on top, kill the snake, otherwise, kill the player
                if (collision.transform.position.y - transform.position.y > 0.1f)
                {
                    // Drop an artifact
                    Instantiate(artifact, transform.position, Quaternion.identity, transform.parent);
                    // Play the sound effect
                    FindObjectOfType<SoundManager>().PlaySound("playerKillSnake");
                    // Destroy the snake
                    Destroy(this.gameObject);
                } else
                {
                    // Run the death animation for the player
                    StartCoroutine(FindObjectOfType<GameManager>().onDeath());
                }
            }
        }

        private void Update()
        {
            // Check if the snake can go forward
            if (!canGoForward())
            {
                // If it can't, flip it's direction
                changeDirection();
            }
        }

        private void FixedUpdate()
        {
            // Make the snake go forward
            transform.Translate(
                new Vector2(-speed * Time.fixedDeltaTime, 0)
            );
        }
    }

}
