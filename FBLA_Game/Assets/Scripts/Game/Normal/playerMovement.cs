/*
 * Hanlin Zhang
 * Purpose: Allows the player to move
 */
using UnityEngine;

namespace JonathansAdventure.Game.Normal
{
    public class playerMovement : MonoBehaviour
    {
        // References to various objects
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform groundCheckerObj;
        [SerializeField] private LayerMask jumpableLayer;
        [SerializeField] private playerAnimation playerAnimation;

        // Speed and jump height of the player
        [SerializeField] private float speed;
        [SerializeField] private float jumpHeight;

        // Reference to sound manager
        private SoundManager soundManager;

        // Variables for motion
        private bool facingRight = true;
        private bool jump = false;
        private float movement;

        // Variables for animation
        private bool isWalking;
        private bool isJumping;

        // Returns true if the user is touching the ground and false if the user is not
        private bool GroundChecker()
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckerObj.position, 0.2f, jumpableLayer);
            if (colliders.Length != 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        // Flip the player if the player starts moving in the opposite direction
        private void flipPlayer()
        {
            if ((movement < 0 && facingRight) || (movement > 0 && !facingRight))
            {
                transform.Rotate(new Vector3(0, 180, 0));
                facingRight = !facingRight;
            }
        }

        // Method runs everytime the player touches an object
        // Checks when the user is grounded again after jumping
        // Used for animation
        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.collider.gameObject.layer == 8 && collision.collider.transform.position.y + 0.1f < transform.position.y)
            {
                isJumping = false;
            }
        }

        // Set a reference to the sound manager
        private void Start()
        {
            soundManager = FindObjectOfType<SoundManager>();
        }

        // Check for user input in the update method
        private void Update()
        {
            // Set the player's horizontal movement
            // Input.GetAxis("Horizontal") returns a float depending on if a/left or d/right are pressed
            movement = Input.GetAxis("Horizontal") * speed;

            // Checks if the user pressed space to jump and if the player is on the ground
            if (Input.GetKeyDown("space") && GroundChecker())
            {
                jump = true;
            }

            // Checks if the user has fallen out out of the game
            if (transform.position.y < -5.5)
            {
                StartCoroutine(FindObjectOfType<GamePlayManager>().onDeath());
            }
        }

        // Use FixedUpdate for inputs to Unity's physics engine
        private void FixedUpdate()
        {
            // Check if the player needs to be flipped
            flipPlayer();

            // Flip the direction of the movement if the user is facing the opposite direction
            if (!facingRight)
            {
                movement *= -1;
            }

            // Move the player left and right
            transform.Translate(
                new Vector2(movement * Time.fixedDeltaTime, rb.velocity.y)
            );

            // Check if the player is still for animation
            if (movement != 0)
            {
                isWalking = true;
            } else
            {
                isWalking = false;
            }

            // Check if the player should jump 
            if (jump)
            {
                rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                soundManager.PlaySound("playerJump");
                isJumping = true;
                jump = false;
            }

            // Animate the player
            playerAnimation.calcAnimation(isWalking, isJumping);
        }

    }

}

