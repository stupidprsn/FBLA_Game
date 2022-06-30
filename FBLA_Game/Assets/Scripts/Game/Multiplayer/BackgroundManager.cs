using UnityEngine;

namespace JonathansAdventure.Game.Multi
{
    /// <summary>
    ///     Manages alternating the backgrounds to keep
    ///     them scrolling infinitely.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/27/2022
    /// </remarks>
    public class BackgroundManager : MonoBehaviour
    {
        /// <summary>
        ///     The background images.
        /// </summary>
        [SerializeField,
            Tooltip("Drag in the backgrounds from bottom up.")] 
        private Transform[] backgrounds;

        /// <summary>
        ///     The index in <see cref="backgrounds"/> of the active background.
        /// </summary>
        private int activeBackground = 0;

        /// <summary>
        ///     Puts the active background on top when it goes out of view.
        /// </summary>
        private void Update()
        {
            // Cache position.
            Vector2 pos = backgrounds[activeBackground].position;

            // Wait for it to go out of view.
            if (!(pos.y < -20)) return;

            // Set it to the top
            backgrounds[activeBackground].position = new Vector2(0, pos.y + 42.15f);

            // Flip active background.
            if(activeBackground == 0)
            {
                activeBackground = 1;
            }
            else
            {
                activeBackground = 0;
            }
        }
    }
}
