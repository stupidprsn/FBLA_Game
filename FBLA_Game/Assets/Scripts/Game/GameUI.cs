using System.Collections;
using UnityEngine;
using TMPro;

namespace Assets.Scripts.Game
{
    /// <summary>
    ///     Manages the UI during gameplay.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/14/2022
    /// </remarks>
    public class GameUI : MonoBehaviour
    {
        /// <summary>
        ///     Updates the health bar to reflect the user's health.
        /// </summary>
        internal void UpdateHealthDisplay(RectTransform heartImage, int health)
        {
            // The image for displaying the health repeats the heart sprite which is 50 pixels.
            // We can dictate how many hearts are shown by editing the size of this image.
            heartImage.sizeDelta = new Vector2((health * 50), 50);
        }
    }
}