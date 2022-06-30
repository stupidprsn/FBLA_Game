using UnityEngine;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Creates navigation for buttons in a vertical layout.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/25/2022
    /// </remarks>
    public class VerticalLayout : ButtonLayout
    {
        /// <summary>
        ///     Watch for user input.
        /// </summary>
        /// <remarks>
        ///     <see cref="ButtonLayout.Increment"/> and <see cref="ButtonLayout.Decrement"/> are inversed 
        ///     as in a vertical layout, going from top to bottom, the "higher" index is lower on the page.
        /// </remarks>
        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            {
                Decrement();
            }

            if (Input.GetKeyDown(KeyCode.S) || Input.GetKeyDown(KeyCode.DownArrow))
            {
                Increment();
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Select();
            }
        }
    }
}

