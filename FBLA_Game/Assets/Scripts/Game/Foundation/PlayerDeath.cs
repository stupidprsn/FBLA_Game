using System.Collections;
using UnityEngine;
using JonathansAdventure.Data;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Manages death for the player
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/30/2022
    /// </remarks>
    public class PlayerDeath : MonoBehaviour, IDestroyable
    {
        [SerializeField,
            Range(1f, 10f),
            Tooltip("Set the amount of time (in seconds) it takes for the user to respawn.")]
        private int respawnTime;

        [SerializeField] private GameObject playerObject;
        [SerializeField] private Transform trans;
        [SerializeField] private Rigidbody2D playerRB;
        [SerializeField] private CapsuleCollider2D playerCollider;
        [SerializeField] private PlayerMovement playerMovement;
        [SerializeField] private PlayerAnimation playerAnimation;

        /// <summary>
        ///     The position to respawn the player after death.
        /// </summary>
        /// <remarks>
        ///     Unlike other fields, this is a property as in multiplayer mode,
        ///     the spawn position gets overriden a lot.
        /// </remarks>
        private protected virtual Vector3 SpawnPosition { get; set; }

        private SoundManager soundManager;
        private GameManager gameManager;

        /// <summary>
        ///     If the player's death animation is currently playing.
        /// </summary>
        /// <remarks>
        ///     Used to make sure the player can't lose multiple hearts
        ///     from one interaction.
        /// </remarks>
        private bool isDying = false;

        /// <summary>
        ///     Disable the player's movement.
        /// </summary>
        public void DisableMovement()
        {
            playerMovement.enabled = false;
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
        }

        /// <summary>
        ///     Death for player. 
        /// </summary>
        /// <remarks>
        ///     Wrapper method for <see cref="OnDeathCoroutine">.
        /// </remarks>
        public void OnDamage()
        {
            // Check to make sure OnDeath isn't called multiple times at once.
            if (isDying) return;
            isDying = true;
            StartCoroutine(OnDeathCoroutine());
        }

        /// <summary>
        ///     <see cref="OnDeath"/>.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator OnDeathCoroutine()
        {
            int health;
            // Update life count.
            if (GameData.IsSingleplayer)
            {
                gameManager.Health--;
                health = gameManager.Health;
            }
            else
            {
                MultiplayerManager.Instance.Health--;
                health = MultiplayerManager.Instance.Health;
            }

            // Show the death animation and lock the player.
            playerAnimation.PlayAnimation(playerAnimation.DiedID);
            DisableMovement();

            soundManager.PlaySound(SoundNames.Died);

            yield return new WaitForSeconds(respawnTime);

            // Checks if the player still has lives left.
            if (health > 0)
            {
                // Singleplayer has set respawn points
                // while multiplayer has to be randomized.
                if (GameData.IsSingleplayer)
                {
                    trans.position = SpawnPosition;
                }
                else
                {
                    MultiplayerManager.Instance.Respawn(trans);
                }
                // Reset the player position and unlock the player.
                playerAnimation.PlayAnimation(playerAnimation.IdleID);
                playerMovement.enabled = true;
                playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

                soundManager.PlaySound(SoundNames.Revive);

                isDying = false;
            }
            else
            {
                if (GameData.IsSingleplayer)
                {
                    GameManager.Instance.DisplayScore(SoundNames.GameOver, false);
                }
                else
                {
                    MultiplayerManager.Instance.DisplayScore();
                }
            }
        }

        /// <summary>
        ///     Set the spawn position to where
        ///     the player first starts in the stage
        ///     and cache references.
        /// </summary>
        private void Start()
        {
            gameManager = GameManager.Instance;
            soundManager = SoundManager.Instance;
            SpawnPosition = trans.position;
        }
    }

}

