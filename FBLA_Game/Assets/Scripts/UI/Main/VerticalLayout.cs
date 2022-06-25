using UnityEngine;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Creates navigation for buttons in a vertical layout.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/24/2022
    /// </remarks>
    public class VerticalLayout : ButtonLayout
    {
        /// <summary>
        ///     Watch for user input.
        /// </summary>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Increment(); 
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Decrement();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Select();
            }
        }
    }
}

