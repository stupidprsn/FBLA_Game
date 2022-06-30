using UnityEngine;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Creates navigation for buttons in a horizontal layout.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/25/2022
    /// </remarks>
    public class HorizontalLayout : ButtonLayout
    {
        /// <summary>
        ///     Watch for user input.
        /// </summary>
        private protected virtual void Update()
        {
            if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
            {
                Increment(); 
            }

            if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
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

