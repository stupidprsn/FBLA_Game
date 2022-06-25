using UnityEngine;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Used to run code when a new scene is loaded.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     6/22/2022
    /// </remarks>
    public class OnEnterLevel : MonoBehaviour
    {
        [SerializeField,
            Tooltip("The music to be played for this stage.")] 
        private SoundNames music;

        /// <summary>
        ///     Inform the game manager that a new stage has been loaded.
        /// </summary>
        private void Start()
        {
            GameManager gameManager = GameManager.Instance;
            gameManager.OnStageEnter(music);
        }
    }

}
