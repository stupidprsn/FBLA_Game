using UnityEngine;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Manages player movement.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/15/2022
    ///     </para>
    ///     <para>
    ///         <see cref="flipPlayer"/> was inspired by
    ///         "Unity Tutorial Quick Tip: The BEST way to flip your character sprite in Unity"
    ///         by "Nick Hwang" 2020.
    ///         <seealso cref="https://www.youtube.com/watch?v=ccxXxvlS4mI"/>
    ///     </para>
    /// </remarks>
    public class PlayerMovement : MonoBehaviour
    {

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
        [SerializeField] private Rigidbody2D rb;
        [SerializeField] private Transform trans;
        [SerializeField] private Transform groundCheckerObj;
        [SerializeField] private LayerMask jumpableLayer;
        [SerializeField] private Animator animator;

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
        ///     If <see cref="SoundNames.Walk"/> is currently playing.
        /// </summary>
        private bool walkSoundPlaying = false;

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

        private void Update()
        {
            // Get horizontal input.
            movement = Input.GetAxis(nameof(Axis.Horizontal)) * speed;

            // Check if the player is idle.
            if (movement == 0)
            {
                IsWalking = false;
            }
            else
            {
                IsWalking = true;
            }

            // Check for space to jump.
            if (Input.GetKeyDown(KeyCode.Space) && GroundChecker()) jump = true;


            // Checks if the user has fallen out out of the game.
            if (trans.position.y < -5.5)
            {
                GameManager.Instance.onDeath();
            }
        }

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
                IsJumping = true;
                jump = false;
            }
        }
        #endregion

        #region Animation

        /// <summary>
        ///     The different animations for Jonathan.
        /// </summary>
        private enum AnimationState
        {
            JonathanJumping,
            JonathanPushing,
            JonathanPushingStill,
            JonathanWalking,
            JonathanIdle
        }

        /// <summary>
        ///     The current animation being played.
        /// </summary>
        private AnimationState currentAnimation;

        /// <summary>
        ///     If the player is currently jumping.
        /// </summary>
        /// <remarks>
        ///     When set, if the value changes, 
        ///     <see cref="CalcAnimation"/> will be
        ///     called.
        /// </remarks>
        private bool IsJumping
        {
            get => IsJumping;
            set
            {
                if (value == IsJumping) return;
                CalcAnimation();
            }
        }

        /// <summary>
        ///     If the player is currently walking.
        /// </summary>
        /// <remarks>
        ///     When set, if the value changes, 
        ///     <see cref="CalcAnimation"/> will be
        ///     called.
        /// </remarks>
        private bool IsWalking
        {
            get => IsWalking;
            set
            {
                if (value == IsWalking) return;
                CalcAnimation();
            }
        }

        /// <summary>
        ///     If the player is currently pushing a box.
        /// </summary>
        /// <remarks>
        ///     When set, if the value changes, 
        ///     <see cref="CalcAnimation"/> will be
        ///     called.
        /// </remarks>
        private bool IsPushing
        {
            get => IsPushing;
            set
            {
                if (value == IsPushing) return;
                CalcAnimation();
            }
        }

        /// <summary>
        ///     Calculates which animation to play and then plays it.
        /// </summary>
        private void CalcAnimation()
        {
            // Jumping animation takes precedent.
            if (IsJumping)
            {
                PlayAnimation(AnimationState.JonathanJumping);
            }
            else
            {
                // Switches between the two pushing states.
                if (IsPushing)
                {
                    if (IsWalking)
                    {
                        PlayAnimation(AnimationState.JonathanPushing);
                    }
                    else
                    {
                        PlayAnimation(AnimationState.JonathanPushingStill);
                    }
                }
                else
                {
                    // Switches between the two "normal" states.
                    if (IsWalking)
                    {
                        PlayAnimation(AnimationState.JonathanWalking);
                    }
                    else
                    {
                        PlayAnimation(AnimationState.JonathanIdle);
                    }
                }
            }
        }

        /// <summary>
        ///     Plays a player animation.
        /// </summary>
        /// <param name="newAnimation"> The animation to play </param>
        private void PlayAnimation(AnimationState newAnimation)
        {
            // Check if we are already playing the animation.
            // Return if we are so that it doesn't play the animation twice.
            if (currentAnimation == newAnimation) return;

            animator.Play(nameof(newAnimation));
            currentAnimation = newAnimation;
            
            // Walking sound effects.
            // Jumping overrides walking.
            if (IsJumping) return;
            // If the player is walking but the sound effect is not playing.
            if (IsWalking && !walkSoundPlaying)
            {
                soundManager.PlaySound(SoundNames.Walk);
                walkSoundPlaying = true;
                return;
            }
            // If the player is not walking bu the sound effect is playing.
            if (!IsWalking && walkSoundPlaying)
            {
                soundManager.StopSound(SoundNames.Walk);
                walkSoundPlaying = false;
            }
        }

        /// <summary>
        ///     Checks if the player comes in contact with a box or the ground.
        /// </summary>
        /// <param name="collision"> Information regarding the object the player has collided with. </param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Check if player is grounded.
            if (collision.collider.gameObject.layer == jumpableLayer.value && // The collision is with the ground.
                collision.collider.transform.position.y + 0.1f < transform.position.y) // The player is above the ground.
            {
                IsJumping = false;
            }

            // Check if the user has hit a box
            if (collision.collider.CompareTag("Box"))
            {
                // Check if the player is to either side of the box
                // by comparing the y positions of the player and box.
                if (transform.position.y - collision.gameObject.transform.position.y < 0.9)
                {
                    IsPushing = true;
                }
            }
        }
        
        /// <summary>
        ///     Checks when the player stops being in contact with a box.
        /// </summary>
        /// <param name="collision"> Information regarding the object the player has collided with. </param>
        private void OnCollisionExit2D(Collision2D collision)
        {
            // Check if it was a box
            if (collision.collider.CompareTag("Box"))
            {
                IsPushing = false;
            }
        }

        #endregion

        private void Awake()
        {
            soundManager = SoundManager.Instance;
        }


    }
}

