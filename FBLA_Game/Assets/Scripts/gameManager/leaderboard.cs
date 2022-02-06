using System.Collections.Generic;

[System.Serializable]
public class Rank {

    public string name;
    public int score;

    public Rank(string inputName, int inputScore) {
        name = inputName;
        score = inputScore;
    }
}

[System.Serializable]
public class Leaderboard {
    public List<Rank> leaderboard;

    public Leaderboard() {
        leaderboard = new List<Rank>();
    }

    public bool newEntry(string name, int score) {
        if(leaderboard.Count != 0) {
            foreach (Rank rank in leaderboard) {
                if (rank.name == name) {
                    rank.score = score;
                    return false;
                }
            }
        }

        leaderboard.Add(
            new Rank(name, score)
        );
        return true;
    }
}