using UnityEngine;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;

namespace JonathansAdventure.UI.Title
{
    /// <summary>
    ///     Manages the title screen and executes code that needs to be ran at the beggining of the program.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/6/2022
    /// </remarks>
    public class TitleScreen : MonoBehaviour {
        #region References

        [Header("Object/Prefab References")]
        [SerializeField]
        private FileManager fileManager;
        [SerializeField]
        private SoundManager soundManager;

        #endregion

        /// <summary>
        ///     Sets up the program.
        /// </summary>
        private void Awake()
        {
            fileManager.LoadDefaults();
            fileManager.UserSettingsData.
        }

        private void Start()
        {
            
        }

        private void Update() 
        {
            
        }
    }
}
