using UnityEngine;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Base class for snake enemies.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/27/2022
    /// </remarks>
    public abstract class Snake : MonoBehaviour, IDestroyable
    {

        #region Abstract

        /// <summary>
        ///     Position relative to the center where the snake AI starts looking 
        ///     for horizontal barriers when it is facing right.
        /// </summary>
        /// <remarks>
        ///     The center of the snake is slightly more toward the left and top.
        /// </remarks>
        protected Vector2 right;

        /// <summary>
        ///     Position relative to the center where the 
        ///     snake AI starts looking for vertical barriers
        ///     when the snake is facing right.
        /// </summary>
        /// <remarks>
        ///     The center of the snake is slightly more toward the left and top.
        /// </remarks>
        protected Vector2 downRight;

        /// <summary>
        ///     Position relative to the center where the snake AI starts looking 
        ///     for horizontal barriers when it is facing left.
        /// </summary>
        /// <remarks>
        ///     The center of the snake is slightly more toward the left and top.
        /// </remarks>
        protected Vector2 left;

        /// <summary>
        ///     Position relative to the center where the 
        ///     snake AI starts looking for vertical barriers
        ///     when the snake is facing left.
        /// </summary>
        /// <remarks>
        ///     The center of the snake is slightly more toward the left and top.
        /// </remarks>
        protected Vector2 downLeft;

        /// <summary>
        ///     Length of raycasts used to look for obstacle's in the snakes way.
        /// </summary>
        protected Vector2 raycast;

        /// <summary>
        ///     Method to execute when the snake is killed.
        /// </summary>
        protected abstract void KillSnake();

        #endregion

        #region Settings

        [Header("Settings")]

        /// <summary>
        ///     Is the snake sprite currently facing right when it spawns.
        /// </summary>
        [SerializeField,
            Tooltip("Is the snake sprite currently facing right?")]
        private bool facingRight;

        /// <summary>
        ///     Used during execution.
        /// </summary>
        private bool isFacingRight;

        /// <summary>
        ///     The speed at which the snake moves.
        /// </summary>
        [SerializeField,
            Range(1f, 10f),
            Tooltip("The speed at which the snake moves.")]
        private float speed;

        /// <summary>
        ///     Is the snake normal sized?
        /// </summary>
        [SerializeField,
            Tooltip("Is the snake normal sized?")]
        private bool isNormal;

        #endregion

        #region References

        [Header("Object References")]
        [SerializeField] private LayerMask boundary;
        [SerializeField] private Transform trans;
        [SerializeField] private Transform artifactTrans;
        [SerializeField] private GameObject artifact;
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
            if (isFacingRight)
            {
                forwardVector2 = new Vector2(trans.position.x + right.x, trans.position.y + right.y);
                downVector2 = new Vector2(trans.position.x + downRight.x, trans.position.y + downRight.y);
                forwardDirection = Vector2.right;
            }
            else
            {
                forwardVector2 = new Vector2(trans.position.x + left.x, trans.position.y + left.y);
                downVector2 = new Vector2(trans.position.x + downLeft.x, trans.position.y + downLeft.y);
                forwardDirection = Vector2.left;
            }

            // Check if there's anything in front of or bellow the snake
            RaycastHit2D[] forwardHits = Physics2D.RaycastAll(forwardVector2, forwardDirection, raycast.x, boundary);
            RaycastHit2D[] downHits = Physics2D.RaycastAll(downVector2, Vector2.down, raycast.y);

            // DEBUG
            Debug.DrawRay(forwardVector2, Vector2.left * raycast.x, Color.green);
            Debug.DrawRay(downVector2, Vector2.down * raycast.y, Color.green);

            // Checks if there's nothing in front of the snake and that there's a platform in front of the snake
            if (forwardHits.Length == 0 && downHits.Length != 0)
            {
                return true;
            }
            else
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
            isFacingRight = !isFacingRight;
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

        /// <summary>
        ///     Randomize the speed so the snakes are not all in sync.
        /// </summary>
        private void Start()
        {
            if (isNormal)
            {
                right = new Vector2(1.05f, -0.45f);
                downRight = new Vector2(0.9f, -0.7f);
                left = new Vector2(-0.7f, -0.45f);
                downLeft = new Vector2(-0.9f, -0.7f);
                raycast = new Vector2(0.3f, 0.75f);
            }
            else
            {
                right = new Vector2(0.7f, -0.25f);
                downRight = new Vector2(0.45f, -0.5f);
                left = new Vector2(-0.35f, -0.25f);
                downLeft = new Vector2(-0.45f, -0.5f);
                raycast = new Vector2(0.2f, 0.5f);
            }

            speed *= Random.Range(0.9f, 1.1f);
        }

        /// <summary>
        ///     Reset the direction the snake is facing each time it is reused.
        /// </summary>
        private void OnEnable()
        {
            isFacingRight = facingRight;
        }

        #endregion

        /// <summary>
        ///     When the snake is killed.
        /// </summary>
        public void OnDamage()
        {
            // Drop an artifact
            artifact.SetActive(true);
            artifactTrans.position = trans.position;
            // The snake needs to hand off the artifact so that it doesn't 
            // Get deleted with the snake.
            artifactTrans.parent = trans.parent;
            // Reset artifact size.
            artifactTrans.localScale = new Vector3(1.75f, 1.75f, 1f);

            SoundManager.Instance.PlaySound(SoundNames.KillSnake);

            if (GameData.IsSingleplayer) GameManager.Instance.SnakeKilled();

            KillSnake();
        }

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
                    OnDamage();
                }
                else
                {
                    collision.gameObject.GetComponent<IDestroyable>().OnDamage();
                }
            }
        }
    }

}
