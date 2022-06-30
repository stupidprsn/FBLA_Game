using UnityEngine;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Manages animating the player.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/23/2022
    /// </remarks>
    public class PlayerAnimation : MonoBehaviour
    {

        /// <summary>
        ///     Is the player Jonathan.
        /// </summary>
        [SerializeField,
            Tooltip("Select if the player is Jonathan (not Jonathana).")]
        private bool isJonathan;

        #region Reference

        [SerializeField] private Transform trans;
        [SerializeField] private Animator animator;
        [SerializeField] private LayerMask jumpableLayer;
        private SoundManager soundManager;

        #endregion

        #region Animation IDs

        internal int JumpID { get; private set; }
        internal int PushID { get; private set; }
        internal int StillPushID { get; private set; }
        internal int WalkID { get; private set; }
        internal int IdleID { get; private set; }
        internal int DiedID { get; private set; }

        #endregion

        #region Properties

        private bool isJumping;

        /// <summary>
        ///     If the player is currently jumping.
        /// </summary>
        /// <remarks>
        ///     When set, if the value changes, 
        ///     <see cref="CalcAnimation"/> will be
        ///     called.
        /// </remarks>
        internal bool IsJumping
        {
            get => isJumping;
            set
            {
                if (value == isJumping) return;
                isJumping = value;
                CalcAnimation();
            }
        }

        private bool isWalking;

        /// <summary>
        ///     If the player is currently walking.
        /// </summary>
        /// <remarks>
        ///     When set, if the value changes, 
        ///     <see cref="CalcAnimation"/> will be
        ///     called.
        /// </remarks>
        internal bool IsWalking
        {
            get => isWalking;
            set
            {
                if (value == isWalking) return;
                isWalking = value;
                CalcAnimation();
            }
        }

        private bool isPushing;

        /// <summary>
        ///     If the player is currently pushing a box.
        /// </summary>
        /// <remarks>
        ///     When set, if the value changes, 
        ///     <see cref="CalcAnimation"/> will be
        ///     called.
        /// </remarks>
        internal bool IsPushing
        {
            get => isPushing;
            set
            {
                if (value == isPushing) return;
                isPushing = value;
                CalcAnimation();
            }
        }

        #endregion

        /// <summary>
        ///     The current animation being played.
        /// </summary>
        private int currentAnimation;

        /// <summary>
        ///     Used to differentiate between player 1 and
        ///     2 walk sounds.
        /// </summary>
        private SoundNames walkSound;

        /// <summary>
        ///     Is the walk sound currently playing.
        /// </summary>
        private bool walkSoundPlaying = false;

        /// <summary>
        ///     Calculates which animation to play and then plays it.
        /// </summary>
        private void CalcAnimation()
        {
            // Jumping animation takes precedent.
            if (IsJumping)
            {
                PlayAnimation(JumpID);
            }
            else
            {
                // Switches between the two pushing states.
                if (IsPushing)
                {
                    if (IsWalking)
                    {
                        PlayAnimation(PushID);
                    }
                    else
                    {
                        PlayAnimation(StillPushID);
                    }
                }
                else
                {
                    // Switches between the two "normal" states.
                    if (IsWalking)
                    {
                        PlayAnimation(WalkID);
                    }
                    else
                    {
                        PlayAnimation(IdleID);
                    }
                }
            }
        }

        /// <summary>
        ///     Plays a player animation.
        /// </summary>
        /// <param name="newAnimation"> The animation to play </param>
        internal void PlayAnimation(int newAnimation)
        {
            // Check if the animation is already playing.
            // Return if it is so that it doesn't play the animation twice.
            if (currentAnimation == newAnimation) return;

            animator.Play(newAnimation);
            currentAnimation = newAnimation;

            // Walking sound effects.
            // Jumping overrides walking.
            if (IsJumping) return;

            // If the player is walking but the sound effect is not playing.
            if (IsWalking && !walkSoundPlaying)
            {
                soundManager.PlaySound(walkSound);
                walkSoundPlaying = true;
                return;
            }
             
            // If the player is not walking bu the sound effect is playing.
            if (!IsWalking && walkSoundPlaying)
            {
                soundManager.StopSound(walkSound);
                walkSoundPlaying = false;
            }
        }

        /// <summary>
        ///     Checks if the player comes in contact with a box or the ground.
        /// </summary>
        /// <param name="collision"> Information regarding the object the player has collided with. </param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Cache positions.
            Vector2 pos = trans.position;
            Vector2 colliderPos = collision.transform.position;

            // Check if player is grounded.
            if (
                // Layermask is in binary while layer is in decimal, the bitwise operation converts
                // the decimal to binary and checks them.
                ((jumpableLayer.value & (1 << collision.gameObject.layer)) > 0) && // The collision is with the ground.
                colliderPos.y + 0.1f < pos.y) // The player is above the ground.
            {
                IsJumping = false;
            }

            // Check if the user has hit a box
            if (collision.collider.CompareTag("Box"))
            {
                // Check if the player is to either side of the box
                // by comparing the y positions of the player and box.
                if (pos.y - colliderPos.y < 0.9)
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

        /// <summary>
        ///     Set a reference to the soundmanager and
        ///     cache animation IDs.
        /// </summary>
        private void Start()
        {
            soundManager = SoundManager.Instance;

            if(isJonathan)
            {
                JumpID = Animator.StringToHash("JonathanJumping");
                PushID = Animator.StringToHash("JonathanPushing");
                StillPushID = Animator.StringToHash("JonathanPushingStill");
                WalkID = Animator.StringToHash("JonathanWalking");
                IdleID = Animator.StringToHash("JonathanIdle");
                DiedID = Animator.StringToHash("JonathanDead");

                walkSound = SoundNames.Walk;
            }
            else
            {
                JumpID = Animator.StringToHash("JonathanaJumping");
                PushID = Animator.StringToHash("JonathanaPushing");
                StillPushID = Animator.StringToHash("JonathanaPushingStill");
                WalkID = Animator.StringToHash("JonathanaWalking");
                IdleID = Animator.StringToHash("JonathanaIdle");
                DiedID = Animator.StringToHash("JonathanDead");

                walkSound = SoundNames.JonathanaWalking;
            }

        }

    }
}
