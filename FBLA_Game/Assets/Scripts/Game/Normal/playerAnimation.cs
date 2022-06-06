/*
 * Hanlin Zhang
 * Purpose: Animates the player
 */

using UnityEngine;

namespace JonathansAdventure.Game.Normal
{
    public class playerAnimation : MonoBehaviour
    {
        // Reference to the player's animator 
        [SerializeField] private Animator animator;
        // Reference to the sound manager
        private SoundManager soundManager;

        // Keep track of what animation is currently playing
        private string currentAnimation;

        // Keep track of if the player is pushing a box
        private bool isPushing = false;

        // Method for calculating which animation to play depending on whether the player is walking, jumping, or pushing 
        // Call play animation with our corresponding animation
        public void calcAnimation(bool isWalking, bool isJumping)
        {
            if (isJumping)
            {
                playAnimation("JonathanJumping");
            } else
            {
                if (isPushing)
                {
                    if (isWalking)
                    {
                        playAnimation("JonathanPushing");
                    } else
                    {
                        playAnimation("JonathanPushingStill");
                    }
                } else
                {
                    if (isWalking)
                    {
                        playAnimation("JonathanWalking");
                    } else
                    {
                        playAnimation("JonathanIdle");
                    }
                }
            }
        }

        // Method for playing animation
        public void playAnimation(string newAnimation)
        {
            // Check if we are already playing the animation
            // Return if we are so that it doesn't play the animation twice
            if (currentAnimation == newAnimation) return;

            // Play the animation
            animator.Play(newAnimation);
            // Record that it's the currently playing animation
            currentAnimation = newAnimation;

            // Check if the player walking sound should be playing
            if (newAnimation == "JonathanPushing" || newAnimation == "JonathanWalking")
            {
                soundManager.PlaySound("playerWalk");
            } else
            {
                soundManager.StopSound("playerWalk");
            }

        }

        // Method runs everytime the player touches an object
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if the user has hit a box
            if (collision.collider.tag == "Box")
            {
                // Check if we are to the side of the box by comparing the y positions of the player and box
                if (transform.position.y - collision.gameObject.transform.position.y < 0.9)
                {
                    // Record that the user is pushing a box
                    isPushing = true;
                }
            }
        }

        // Method runs everytime the player stops touching an object
        private void OnCollisionExit2D(Collision2D collision)
        {
            // Check if it was a box
            if (collision.collider.tag == "Box")
            {
                // Record that the user is no longer pushing a box
                isPushing = false;
            }
        }

        // Set a reference to the sound manager
        private void Start()
        {
            soundManager = FindObjectOfType<SoundManager>();
        }
    }

}
