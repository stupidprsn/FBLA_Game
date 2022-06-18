using UnityEngine;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Allows the user to "enter" the door
    ///     after it has been opened.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/15/2022
    /// </remarks>
    public class OpenedDoor : MonoBehaviour
    {

        #region References

        [Header("Object References")]
        [SerializeField] private BoxCollider2D doorCollider;
        private CapsuleCollider2D playerCollider;
        private GameManager gameManager;

        #endregion

        private void Start()
        {
            gameManager = GameManager.Instance;
            playerCollider = gameManager.PlayerCollider;
        }

        private void Update()
        {
            bool recievedInput = Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow);
            
            if (recievedInput && playerCollider.IsTouching(doorCollider))
            {
                gameManager.winStage();
            }
        }
    }

}
