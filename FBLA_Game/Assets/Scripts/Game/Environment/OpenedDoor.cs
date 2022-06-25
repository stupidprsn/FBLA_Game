using UnityEngine;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Allows the user to "enter" the door
    ///     after it has been opened.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/15/2022
    /// </remarks>
    public class OpenedDoor : Interactable
    {
        [SerializeField] private GameObject doorLock;

        /// <summary>
        ///     Let the user exit the stage when they interact
        ///     with the opened door.
        /// </summary>
        protected override void OnInteract(PlayerMovement player)
        {
            GameManager.Instance.StageCleared();
        }

        /// <summary>
        ///     Disable the door lock sprite to "open" the door.
        /// </summary>
        private void OnEnable()
        {
            doorLock.SetActive(false);
        }
    }

}
