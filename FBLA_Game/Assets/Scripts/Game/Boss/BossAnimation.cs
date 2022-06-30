using UnityEngine;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     Manages animating the boss.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/25/2022
    ///     </para>
    ///     <para>
    ///         <see cref="Flip"/> was inspired by
    ///         "Unity Tutorial Quick Tip: The BEST way to flip your character sprite in Unity"
    ///         by "Nick Hwang" 2020.
    ///         <seealso cref="https://www.youtube.com/watch?v=ccxXxvlS4mI"/>
    ///     </para>
    /// </remarks>
    public class BossAnimation : MonoBehaviour, IDestroyable
    {
        #region Settings

        /// <summary>
        ///     Is the player sprite currently facing right?
        /// </summary>
        [SerializeField,
            Tooltip("Is the player sprite currently facing right?")]
        private bool facingRight;

        /// <summary>
        ///     How much the boss should be nudged once it is killed.
        /// </summary>
        [SerializeField,
            Tooltip("How much the boss should be nudged once it is killed.")]
        private Vector2 deathNudge;

        #endregion

        #region References

        [SerializeField] private Rigidbody2D bossRB;
        [SerializeField] private Animator bossAnimator;

        private Transform playerTrans;

        #endregion

        /// <summary>
        ///     The ID for the trigger that plays the boss's damanged animation.
        /// </summary>
        private int damageAnimation;

        /// <summary>
        ///     Plays the damage animation for the boss.
        /// </summary>
        internal void DamageAnimation()
        {
            bossAnimator.SetTrigger(damageAnimation);
        }

        /// <summary>
        ///     Plays the death animation for the boss.
        /// </summary>
        internal void DeathAnimation()
        {
            // Allow the boss to move
            bossRB.constraints = RigidbodyConstraints2D.None;

            // Give the boss a slight nudge
            bossRB.AddForce(new Vector2(deathNudge.x, deathNudge.y), ForceMode2D.Impulse);
        }

        /// <summary>
        ///     Called by the lasers when they hit the boss.
        /// </summary>
        public void OnDamage()
        {
            bossAnimator.SetTrigger(damageAnimation);
        }

        /// <summary>
        ///     Cache animation ID.
        /// </summary>
        private void Awake()
        {
            damageAnimation = Animator.StringToHash("Damage");
            playerTrans = GameObject.FindWithTag("Player").transform;
        }

        /// <summary>
        ///     Method for flipping the direction the boss is facing to match the player.
        /// </summary>
        private void Update()
        {
            // Cache x coordinate.
            float x = playerTrans.position.x;

            // If the user is to the left and the boss is facing right,
            // or if the user is to the right and the boss is facing left...
            if ((x < 0 && facingRight) || (x > 0 && !facingRight))
            {
                transform.Rotate(new Vector3(0, 180, 0));
                facingRight = !facingRight;
            }
        }
    }
}

