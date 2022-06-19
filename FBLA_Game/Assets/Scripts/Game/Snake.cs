using UnityEngine;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Base class for snake enemies.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/18/2022
    /// </remarks>
    public abstract class Snake : MonoBehaviour
    {

        #region Abstract

        /// <summary>
        ///     Position relative to the center where the snake AI starts looking 
        ///     for horizontal barriers when it is facing right.
        /// </summary>
        /// <remarks>
        ///     The center of the snake is slightly more toward the left and top.
        /// </remarks>
        private protected Vector2 right;
        /// <summary>
        ///     Position relative to the center where the snake AI starts looking 
        ///     for horizontal barriers when it is facing left.
        /// </summary>
        /// <remarks>
        ///     The center of the snake is slightly more toward the left and top.
        /// </remarks>
        private protected Vector2 left;
        /// <summary>
        ///     Position relative to the center where the 
        ///     snake AI starts looking for vertical barriers.
        /// </summary>
        /// <remarks>
        ///     The center of the snake is slightly more toward the left and top.
        /// </remarks>
        private protected Vector2 down;
        /// <summary>
        ///     Length of raycasts used to look for obstacle's in the snakes way.
        /// </summary>
        private protected Vector2 raycast;
        /// <summary>
        ///     Method to execute when the snake is killed.
        /// </summary>
        private protected abstract void KillSnake();

        #endregion

        #region Settings

        [Header("Settings")]

        [SerializeField,
            Tooltip("Is the snake sprite currently facing right?")] 
        private bool facingRight;
        
        [SerializeField,
            Range(1f, 10f),
            Tooltip("The speed at which the snake moves.")] 
        private float speed;

        #endregion

        #region References

        [Header("Object References")]
        [SerializeField] private Transform trans;
        [SerializeField] private GameObject artifact;
        [SerializeField] private LayerMask boundary;

        #endregion

        #region Movement

        /// <summary>
        ///     Checks if the snake can move forward.
        /// </summary>
        /// <returns> If the snake can move forward. </returns>
        private bool CanGoForward()
        {
            // Check if there's anything in the forward direction
            Vector2 forwardVector2;
            // Check if there's anything below the snake
            Vector2 downVector2;
            // The direction the snake is facing
            Vector2 forwardDirection;


            // Flip our variables depending on if the snake is facing left or right
            // The center of the snake is slightly more toward the left and top.
            if (facingRight)
            {
                forwardVector2 = new Vector2(trans.position.x + right.x, trans.position.y + right.y);
                forwardDirection = Vector2.right;
            } else
            {
                forwardVector2 = new Vector2(trans.position.x + left.x, trans.position.y - left.y);
                forwardDirection = Vector2.left;
            }

            // Down vector is same regardless of which horizontal dirrection the snake faces.
            downVector2 = new Vector2(trans.position.x + down.x, trans.position.y + down.y);

            // Check if there's anything in front of or bellow the snake
            RaycastHit2D[] forwardHits = Physics2D.RaycastAll(forwardVector2, forwardDirection, raycast.x, boundary);
            RaycastHit2D[] downHits = Physics2D.RaycastAll(downVector2, Vector2.down, raycast.y);

            // Checks if there's nothing in front of the snake and that there's a platform in front of the snake
            if (forwardHits.Length == 0 && downHits.Length != 0)
            {
                return true;
            } else
            {
                return false;
            }
        }

        /// <summary>
        ///     Flip the direction the snake is facing.
        /// </summary>
        private void ChangeDirection()
        {
            trans.Rotate(new Vector3(0, 180, 0));
            facingRight = !facingRight;
        }

        /// <summary>
        ///     Manages turning the snake.
        /// </summary>
        private void Update()
        {
            if (!CanGoForward())
            {
                ChangeDirection();
            }
        }

        /// <summary>
        ///     Manages moving the snake.
        /// </summary>
        private void FixedUpdate()
        {
            transform.Translate(
                new Vector2(-speed * Time.fixedDeltaTime, 0)
            );
        }

        #endregion

        /// <summary>
        ///     Checks for collisions with the player.
        /// </summary>
        /// <param name="collision"> Information regarding the object the snake has collided with. </param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Checks if it was the player
            if (collision.collider.CompareTag("Player"))
            {
                // Checks if the player is on top of or to the side of the snake
                // If the player is on top, kill the snake, otherwise, kill the player
                if (collision.transform.position.y - trans.position.y > 0.1f)
                {
                    // Drop an artifact
                    artifact.SetActive(true);
                    artifact.transform.position = trans.position;

                    SoundManager.Instance.PlaySound(SoundNames.KillSnake);

                    KillSnake();
                } else
                {
                    // Run the death animation for the player
                    GameManager.Instance.onDeath();
                }
            }
        }
    }

}
