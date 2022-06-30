using UnityEngine;
using UnityEngine.UI;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Scrolls up and down the leaderboard
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/25/2022
    /// </remarks>
    public class LeaderboardScroll : MonoBehaviour
    {
        #region Settings

        [SerializeField,
            Range(0.001f, 0.01f),
            Tooltip("The speed at which the user can scroll through" +
            "the leaderboard. This should be a really small number.")]
        private float speed;

        #endregion

        [SerializeField] private ScrollRect scrollRect;

        /// <summary>
        ///     The directional axis for scrolling through the leaderboard.
        /// </summary>
        private string axis;

        /// <summary>
        ///     Allows the user to scroll up and down.
        /// </summary>
        private void Update()
        {
            scrollRect.verticalNormalizedPosition += Input.GetAxis(axis) * speed;
        }

        /// <summary>
        ///     Get string from axis.
        /// </summary>
        private void Awake()
        {
            axis = Axis.Vertical.ToString();
        }
    }
}
