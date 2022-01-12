using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class leaderboard : MonoBehaviour {
    public GameObject scrollRect;
    public GameObject rankingPrefab;
    public Transform content;
    public gameManager gameManager;

    public List<Rank> rankings;

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
        FindObjectOfType<UnityEngine.UI.ScrollRect>().verticalNormalizedPosition = Input.GetAxis("Vertical");
    }
    void Start() {
        updateScreen();
    }
}
