using UnityEngine;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;

namespace JonathansAdventure.Management
{
    /// <summary>
    ///     Misc methods used to manage the program.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/7/2022
    /// </remarks>
    public class ProgramManager : MonoBehaviour
    {
        /// <summary>
        ///     Singleton Check to ensure only one <see cref="ProgramManager"/> exists.
        /// </summary>
        [HideInInspector] public static ProgramManager singletonCheck;

        [Header("Object/Prefab References")]
        [SerializeField] private GameObject gameManagerPrefab;
        [SerializeField] private GameObject fileManagerPrefab;
        [SerializeField] private SoundManager soundManager;

        // Applys the user's saved settings
        public void SetSettings(UserSettings userSettings)
        {
            Screen.SetResolution(userSettings.display.x, userSettings.display.y, userSettings.display.fullScreen);
            soundManager.SetVolume(userSettings.volume);
        }

        private void Awake()
        {
            // Singleton Check
            if (singletonCheck == null)
            {
                singletonCheck = this;
            } else
            {
                Destroy(gameObject);
            }

            // Keep the game manager in all scenes
            DontDestroyOnLoad(transform.parent.gameObject);

            // Set the screen resolution and lock the mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            // Allow the application to quit with the esc key
            if (Input.GetKeyDown("escape"))
            {
                Application.Quit();
            }
        }
    }

}
