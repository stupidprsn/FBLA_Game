/*
 * Hanlin Zhang
 * Purpose: Used to update and display the leaderboard
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class leaderboardScreen : MonoBehaviour {
    // Referances to the leaderboard display
    [SerializeField] private Transform content;

    // Referance to message that pops up if the leaderboard is empty
    [SerializeField] private GameObject noLeaderboardMsg;

    // Referance to a template that we can instantiate to use for the different leaderboard tiers
    [SerializeField] private GameObject rankingPrefab;

    public void updateScreen() {
        // Get the leaderboard from the fileManager as a list and order it by placement
        List<Rank> leaderboard = FindObjectOfType<fileManager>().loadLeaderboard().leaderboard;

        // Check if a leaderboard has been created
        if(leaderboard != null) {
            // Sort the leaderboard
            leaderboard.OrderByDescending(x => x.score).ToList();

            // For each ranking, create a visual display using our rankingPrefab template
            for (int i = 0; i < leaderboard.Count; i++) {
                GameObject newRank = Instantiate(rankingPrefab, content);

                // Show the placement on the leaderboard
                // We need this as 1st, 2nd, and 3rd aren't standard
                // After that, we can use the conjugation  n + "th" where n is the numberic placement
                if (i == 0) {
                    newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = "1st";
                } else if (i == 1) {
                    newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = "2nd";
                } else if (i == 2) {
                    newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = "3rd";
                } else {
                    newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = (i + 1).ToString() + "th";
                }

                // Update the name and points on the screen
                newRank.transform.Find("name").gameObject.GetComponent<TMP_Text>().text = leaderboard[i].name;
                newRank.transform.Find("points").gameObject.GetComponent<TMP_Text>().text = leaderboard[i].score.ToString();
            }
        } else {
            // Show the message that says the leaderboard is empty
            noLeaderboardMsg.SetActive(true);
        }
    }

    // Allows the user to scroll up and down with arrow keys or with w and d.
    private void Update() {
        FindObjectOfType<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition += Input.GetAxis("Vertical");
    }

    // Update the screen once when the panel loads. 
    void Start() {
        updateScreen();
    }
}
