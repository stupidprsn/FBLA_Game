using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using JonathansAdventure.Data;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Displays the leaderboard.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/17/2022
    /// </remarks>
    public class LeaderboardScreen : MonoBehaviour
    {

        #region Settings

        [SerializeField,
            Range(0.001f, 0.01f),
            Tooltip("The speed at which the user can scroll through" +
            "the leaderboard. This should be a really small number.")]
        private float speed;

        #endregion

        #region References

        [Header("Object/Prefab References")]
        [SerializeField] private Transform content;
        [SerializeField] private GameObject noLeaderboardMsg;
        [SerializeField] private GameObject rankingPrefab;

        #endregion

        /// <summary>
        ///     Updates the leaderboard display.
        /// </summary>
        public void UpdateScreen()
        {
            // Get the leaderboard from the fileManager.
            List<Rank> leaderboard = FindObjectOfType<FileManager>().SingleLeaderboard.Load().Data;

            // Check if there is content in the leaderboard.
            if (leaderboard.Count > 0)
            {
                // Sort the leaderboard
                leaderboard.OrderByDescending(x => x.Score).ToList();

                // For each ranking, create a visual display using our rankingPrefab template
                for (int i = 0; i < leaderboard.Count; i++)
                {
                    GameObject newRank = Instantiate(rankingPrefab, content);
                    TMP_Text text = newRank.transform.Find("place").GetComponent<TMP_Text>();

                    // Show the placement on the leaderboard
                    // We need this as "1st", "2nd", and "3rd" aren't standard
                    // After that, we can use the conjugation n + "th" where n is the numberic placement
                    if (i == 0)
                    {
                        text.text = "1st";
                    }
                    else if (i == 1)
                    {
                        text.text = "2nd";
                    }
                    else if (i == 2)
                    {
                        text.text = "3rd";
                    }
                    else
                    {
                        text.text = (i + 1).ToString() + "th";
                    }

                    // Update the name and points on the screen
                    newRank.transform.Find("name").gameObject.GetComponent<TMP_Text>().text = leaderboard[i].Name;
                    newRank.transform.Find("points").gameObject.GetComponent<TMP_Text>().text = leaderboard[i].Score.ToString();
                }
            }
            else
            {
                // Show the message that says the leaderboard is empty
                noLeaderboardMsg.SetActive(true);
            }
        }

        /// <summary>
        ///     Allows the user to scroll up and down.
        /// </summary>
        private void Update()
        {
            FindObjectOfType<ScrollRect>().verticalNormalizedPosition += Input.GetAxis(nameof(Axis.Vertical)) * speed;
        }

        void Start()
        {
            UpdateScreen();
        }
    }

}

