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
    public TextAsset jsonFile;

    // Referances to the leaderboard display
    public GameObject scrollRect;
    public Transform content;

    // Referance to the prefab that we can instantiate
    public GameObject rankingPrefab;

    // List of all leaderboard entries.
    private List<Rank> rankings;

    public void updateScreen() {
        rankings = gameManager.theRankings.OrderByDescending(x => x.score).ToList();
        rankings = rankings.Distinct().ToList();
        for (int i = 0; i < rankings.Count; i++) {
            GameObject newRank = Instantiate(rankingPrefab, content);
            if(i == 0) {
                newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = "1st";
            } else if (i == 1) {
                newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = "2nd";
            } else if (i == 2) {
                newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = "3rd";
            } else {
                newRank.transform.Find("place").gameObject.GetComponent<TMP_Text>().text = (i + 1).ToString() + "th";
            }

            newRank.transform.Find("name").gameObject.GetComponent<TMP_Text>().text = rankings[i].name;
            newRank.transform.Find("points").gameObject.GetComponent<TMP_Text>().text = rankings[i].score.ToString();
        }
    }

    private void Update() {
        FindObjectOfType<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition += Input.GetAxis("Vertical");
    }

    void Start() {
        updateScreen();

        Rankings temp = JsonUtility.FromJson<Rankings>(jsonFile.text);
        rankings = temp.rankings.ToList();

        rankings.Add(new Rank("Test", 600 ));

        temp.rankings = rankings;

        string newJasonString = JsonUtility.ToJson(temp);
        JsonUtility.FromJsonOverwrite(jsonFile.text, newJasonString);
    }
}
