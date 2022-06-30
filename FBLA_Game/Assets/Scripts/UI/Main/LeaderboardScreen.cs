using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;
using JonathansAdventure.Data;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Displays the leaderboard.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Don't disable during transitions.
    ///     </para>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/25/2022
    ///     </para>
    /// </remarks>
    public class LeaderboardScreen : MonoBehaviour
    {

        #region References

        [Header("Object/Prefab References")]
        [SerializeField] private GameObject singleContentObject;
        [SerializeField] private GameObject multiContentObject;
        [SerializeField] private Transform singleContent;
        [SerializeField] private Transform multiContent;

        [SerializeField] private GameObject noLeaderboardMsg;
        [SerializeField] private GameObject rankingPrefab;
        [SerializeField] private TMP_Text title;

        private FileManager fileManager;

        #endregion

        /// <summary>
        ///     Is singleplayer currently shown.
        /// </summary>
        private bool? singleShown = null;

        /// <summary>
        ///     If the singleplayer leaderboard is empty.
        /// </summary>
        private bool singleEmpty;

        /// <summary>
        ///     If the multiplayer leaderboard is empty.
        /// </summary>
        private bool multiEmpty;

        /// <summary>
        ///     Sorts and fills the leaderboard.
        /// </summary>
        /// <param name="leaderboard"> The data </param>
        /// <param name="content"> The parent object to place the rankings under. </param>
        /// <returns> If the leaderboard is empty. </returns>
        private bool CreateBoard(List<Rank> leaderboard, Transform content)
        {
            // Return if the leaderboard is empty.
            if (leaderboard.Count == 0) return true;

            // Sort the leaderboard
            leaderboard = leaderboard.OrderByDescending(x => x.Score).ToList();

            // For each ranking, create a visual display using our rankingPrefab template
            for (int i = 0; i < leaderboard.Count; i++)
            {
                GameObject newRank = Instantiate(rankingPrefab, content);
                Ranking texts = newRank.GetComponent<Ranking>();

                // Show the placement on the leaderboard
                // We need this as "1st", "2nd", and "3rd" aren't standard
                // After that, we can use the conjugation n + "th" where n is the numberic placement
                if (i == 0)
                {
                    texts.Place.SetText("1st");
                }
                else if (i == 1)
                {
                    texts.Place.SetText("2st");
                }
                else if (i == 2)
                {
                    texts.Place.SetText("3st");
                }
                else
                {
                    texts.Place.SetText((i + 1).ToString() + "th");
                }

                // Update the name and points on the screen
                texts.Name.SetText(leaderboard[i].Name);
                texts.Score.SetText(leaderboard[i].Score.ToString());

            }

            return false;
        }

        /// <summary>
        ///     Update title and data.
        /// </summary>
        private void OnEnable()
        {
            fileManager = FileManager.Instance;

            // If no leaderboard is shown OR if the shown one is not equal (XOR to) the one selected.
            if (singleShown == null || (singleShown.Value ^ GameData.IsSingleplayer))
            {
                // Set the title, show the selected leaderboard, and the no content message.
                if (GameData.IsSingleplayer)
                {
                    title.SetText("SINGLEPLAYER LEADERBOARD");

                    multiContentObject.SetActive(false);
                    singleContentObject.SetActive(true);

                    if (singleEmpty)
                    {
                        noLeaderboardMsg.SetActive(true);
                    }
                    else
                    {
                        noLeaderboardMsg.SetActive(false);
                    }

                    singleShown = true;
                }
                else
                {
                    title.SetText("MULTIPLAYER LEADERBOARD");

                    singleContentObject.SetActive(false);
                    multiContentObject.SetActive(true);

                    if (multiEmpty)
                    {
                        noLeaderboardMsg.SetActive(true);
                    }
                    else
                    {
                        noLeaderboardMsg.SetActive(false);
                    }

                    singleShown = false;
                }
            }
        }

        /// <summary>
        ///     Fill the leaderboards.
        /// </summary>
        private void Start()
        {
            List<Rank> single = fileManager.SingleLeaderboard.Load().Data;
            List<Rank> multi = fileManager.MultiLeaderboard.Load().Data;

            singleEmpty = CreateBoard(single, singleContent);
            multiEmpty = CreateBoard(multi, multiContent);
        }
    }

}

