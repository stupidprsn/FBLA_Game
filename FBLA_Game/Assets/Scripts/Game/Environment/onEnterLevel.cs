using UnityEngine;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Used to run code when a new scene is loaded.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     6/15/2022
    /// </remarks>
    public class OnEnterLevel : MonoBehaviour
    {
        [SerializeField,
            Tooltip("The music to be played for this stage.")] 
        private string music;

        private void Awake()
        {
            // Call the method in the game play manager for when a new stage is loaded
            FindObjectOfType<GameManager>().OnStageEnter(music, GameObject.Find("Jonathan").transform.position);
        }
    }

}
