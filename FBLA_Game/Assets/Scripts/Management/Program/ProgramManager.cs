using UnityEngine;

namespace JonathansAdventure.Management
{
    /// <summary>
    ///     Misc methods used to manage the program.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/7/2022
    /// </remarks>
    public class ProgramManager : Singleton<ProgramManager>
    {
        private void Awake()
        {
            SingletonCheck(this);

        }

        private void Update()
        {
            // Allow the application to quit with the esc key
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
            }
        }
    }

}
