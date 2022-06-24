using UnityEngine;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Base class for all player interactable objects.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/21/2022
    /// </remarks>
    public abstract class Interactable : MonoBehaviour
    {
        /// <summary>
        ///     The method to call when the object is interacted with.
        /// </summary>
        abstract protected void OnInteract(PlayerMovement player);

        /// <summary>
        ///     If the object is currently in contact with a player.
        /// </summary>
        private bool inContact;

        /// <summary>
        ///     The player the object is currently in contact with.
        /// </summary>
        private PlayerMovement player;

        /// <summary>
        ///     The interaction key(s) for the player the object is
        ///     currently in contact with.
        /// </summary>
        private KeyCode[] interactKey;

        /// <summary>
        ///     Check when a player comes in contact with the object.
        /// </summary>
        /// <remarks>
        ///     Record that the object is currently in contact with a player
        ///     and the interact key for that player.
        /// </remarks>
        /// <param name="collision"> Information regarding the collision object. </param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Guard clause to check that the collision was with a player.
            if (!collision.CompareTag("Player")) return;
            inContact = true;
            player = collision.gameObject.GetComponent<PlayerMovement>();
            interactKey = player.InteractKey;
        }
        /// <summary>
        ///     Check when the interactable object is no longer in contact with the player.
        /// </summary>
        /// <remarks>
        ///     Record that the object is no longer in contact with a player.
        /// </remarks>
        /// <param name="collision"> Information regarding the collision object. </param>
        private void OnTriggerExit2D(Collider2D collision)
        {
            // Guard clause to check that the collision was with a player.
            if (!collision.CompareTag("Player")) return;
            inContact = false;
            player = null;
            interactKey = null;
        }

        /// <summary>
        ///     If the object is currently in contact with a player,
        ///     and the player's interact key is pressed,
        ///     then call <see cref="OnInteract"/>.
        /// </summary>
        private void Update()
        {
            if (!inContact) return;
            foreach(KeyCode keyCode in interactKey)
            {
                if (Input.GetKeyDown(keyCode)) OnInteract(player);
            }
        }
    }
}
