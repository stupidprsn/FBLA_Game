using UnityEngine;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Creates navigation for buttons in a horizontal layout.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/24/2022
    /// </remarks>
    public class HorizontalLayout : ButtonLayout
    {
        /// <summary>
        ///     Watch for user input.
        /// </summary>
        private protected virtual void Update()
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

