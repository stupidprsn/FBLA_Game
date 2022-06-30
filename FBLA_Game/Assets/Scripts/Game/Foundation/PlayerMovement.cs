using UnityEngine;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Creates movement and animation for player.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/23/2022
    ///     </para>
    ///     <para>
    ///         <see cref="FlipPlayer"/> was inspired by
    ///         "Unity Tutorial Quick Tip: The BEST way to flip your character sprite in Unity"
    ///         by "Nick Hwang" 2020.
    ///         <seealso cref="https://www.youtube.com/watch?v=ccxXxvlS4mI"/>
    ///     </para>
    /// </remarks>
    public class PlayerMovement : MonoBehaviour
    {
        #region Controls

        [Header("Controls")]

        [SerializeField,
            Tooltip("Set the axis that controlls this player.")]
        private Axis horizontal;

        /// <summary>
        ///     The string that is used for getting horizontal axis input.
        /// </summary>
        /// <remarks>
        ///     Determined by <see cref="horizontal"/>.
        /// </remarks>
        private string horizontalAxis;

        /// <summary>
        ///     Property for getting the user's horizontal movement input.
        /// </summary>
        private float HorizontalAxis
        {
            get => Input.GetAxis(horizontalAxis);
        }

        [SerializeField,
            Tooltip("Set the jump button(s) for this player.")]
        private protected KeyCode[] jumpKeys;

        /// <summary>
        ///     If the user has inputted the signal to jump.
        /// </summary>
        private bool JumpKey
        {
            get
            {
                // Iterate through all of the jump keys,
                // if any are pressed, return true.
                foreach (KeyCode keyCode in jumpKeys)
                {
                    if (Input.GetKeyDown(keyCode)) return true;
                }

                // If none of them have been pressed, return false.
                return false;
            }
        }

        /// <summary>
        ///     The interaction key(s) for this player.
        /// </summary>
        [field:
            SerializeField,
            Tooltip("Set the interaction button(s) for this player.")]
        public KeyCode[] InteractKey { get; private set; }

        #endregion

        #region Settings

        [Header("Settings")]

        [SerializeField,
            Range(1f, 10f),
            Tooltip("The horizontal movement speed.")]
        private float speed;

        [SerializeField,
            Range(1f, 10f),
            Tooltip("The vertical jump power.")]
        private float jumpHeight;

        [SerializeField,
            Tooltip("Is the player sprite currently facing right?")]
        private bool facingRight;

        #endregion

        #region References

        [Header("Object References")]
        [SerializeField] private PlayerAnimation playerAnimation;
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform trans;
        [SerializeField] private Transform groundCheckerObj;
        [SerializeField] private LayerMask jumpableLayer;
        [SerializeField] private Animator animator;

        private GameManager gameManager;
        private SoundManager soundManager;

        #endregion

        #region Motion

        /// <summary>
        ///     If the player jumps the next <see cref="FixedUpdate"/> loop.
        /// </summary>
        private bool jump = false;

        /// <summary>
        ///     How much the player should move the next <see cref="FixedUpdate"/> loop.
        /// </summary>
        private float movement;

        /// <summary>
        ///     Checks if the player is grounded.
        /// </summary>
        /// <returns> If the player is grounded or not. </returns>
        private bool GroundChecker()
        {
            // Get all ground objects that the player is currently colliding with.
            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckerObj.position, 0.2f, jumpableLayer);

            if (colliders.Length != 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        ///     Flip the player horizontally.
        /// </summary>
        private void FlipPlayer()
        {
            // If the player is facing the opposite way from which their sprite is facing.
            if ((movement < 0 && facingRight) || (movement > 0 && !facingRight))
            {
                transform.Rotate(new Vector3(0, 180, 0));
                facingRight = !facingRight;
            }
        }

        /// <summary>
        ///     Collect user input.
        /// </summary>
        private void Update()
        {
            // Get horizontal input.
            movement = HorizontalAxis * speed;

            // Check if the player is idle.
            if (movement == 0)
            {
                playerAnimation.IsWalking = false;
            }
            else

            {
                playerAnimation.IsWalking = true;
            }

            // Check for space to jump.
            if (JumpKey && GroundChecker()) jump = true;
        }

        /// <summary>
        ///     Update player physics.
        /// </summary>
        private void FixedUpdate()
        {
            // Check if the player needs to be flipped
            FlipPlayer();

            // Flip the direction of the movement if the user is facing the opposite direction
            if (!facingRight)
            {
                movement *= -1;
            }

            // Move the player horizontally
            transform.Translate(
                new Vector2(movement * Time.fixedDeltaTime, rb.velocity.y)
            );

            // Check if the player should jump 
            if (jump)
            {
                rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
                soundManager.PlaySound(SoundNames.Jump);
                playerAnimation.IsJumping = true;
                jump = false;
            }
        }
        #endregion

        /// <summary>
        ///     Turn the horizontal axis to a usuable string.
        /// </summary>
        private void Start()
        {
            soundManager = SoundManager.Instance;
            gameManager = GameManager.Instance;
            horizontalAxis = horizontal.ToString();
        }
    }
}

