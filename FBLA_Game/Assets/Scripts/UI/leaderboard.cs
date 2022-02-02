/*
 * Hanlin Zhang
 * Purpose: Used to update and display the leaderboard
 */

using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class leaderboard : MonoBehaviour {
    // Referance to the json file containing the leaderboard data
    [SerializeField] private TextAsset jsonFile;

    // Referances to the leaderboard display
    [SerializeField] private Transform content;

    // Referance to the prefab that we can instantiate
    [SerializeField] private GameObject rankingPrefab;

    // List of all leaderboard entries.
    private List<Rank> rankings;

    public void updateScreen() {
        //rankings = gameManager.theRankings.OrderByDescending(x => x.score).ToList();
        //rankings = rankings.Distinct().ToList();
        Debug.Log("Update Screen Called");
        // Read the json file, then organize the list by the highest score. 
        rankings = JsonUtility.FromJson<Rankings>(jsonFile.text).rankings.OrderByDescending(x => x.score).ToList();

        foreach (Rank item in rankings) {
            Debug.Log(item.name);
        }

        // For each ranking, create a visual display.
        for (int i = 0; i < rankings.Count; i++) {
            GameObject newRank = Instantiate(rankingPrefab, content);
            Debug.Log(newRank);
            // Show the rank on the leaderboard.
            // We need this as 1st, 2nd, and 3rd aren't standard, after that, we can use a standard n + "th".
            if(i == 0) {
                newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = "1st";
            } else if (i == 1) {
                newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = "2nd";
            } else if (i == 2) {
                newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = "3rd";
            } else {
                newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = (i + 1).ToString() + "th";
            }

            // Update the name and points on the screen.
            newRank.transform.Find("name").gameObject.GetComponent<TMP_Text>().text = rankings[i].name;
            newRank.transform.Find("points").gameObject.GetComponent<TMP_Text>().text = rankings[i].score.ToString();
        }
    }

    // Allows the user to scroll up and down with arrow keys or with w and d.
    private void Update() {
        FindObjectOfType<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition += Input.GetAxis("Vertical");
    }

    // Update the screen once when the panel loads. 
    void Start() {
        updateScreen();

        //Rankings temp = JsonUtility.FromJson<Rankings>(jsonFile.text);
        //rankings = temp.rankings.ToList();

        //rankings.Add(new Rank("Test", 600 ));

        //temp.rankings = rankings;

        //string newJasonString = JsonUtility.ToJson(temp);
        //JsonUtility.FromJsonOverwrite(jsonFile.text, newJasonString);
    }
}
