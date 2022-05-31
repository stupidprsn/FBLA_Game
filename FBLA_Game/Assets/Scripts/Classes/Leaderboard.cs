/*
 * Hanlin Zhang
 * Purpose: Sets up classes used to store data regarding the leaderboard
 */

using System.Collections.Generic;

/// <summary>
///     Rank Class for each individual ranking on the leaderboard
/// </summary>
[System.Serializable]
public class Rank {

    public string Name { get; set; }
    public int Score { get; set; }

    public Rank(string name, int score) {
        Name = name;
        Score = score;
    }
}

// Leaderboard class contains a list of ranks
[System.Serializable]
public class Leaderboard {
    public List<Rank> leaderboard;

    public Leaderboard() {
        leaderboard = new List<Rank>();
    }

    // Method for adding a new Rank to the list
    // Returns a boolean that is true if the name is already in the leaderboard and false if it's a new entry
    public bool NewEntry(string name, int score) {
        // Checks if the name already exists
        // If it does, update the score for the corresponding name if the score is higher
        foreach (Rank rank in leaderboard) {
            if (rank.Name == name) {
                // Checks if the score is higher
                if(score > rank.Score) {
                    rank.Score = score;
                }
                return false;
            }
        }

        // If the name doesn't exist in the leaderboard yet, add it
        leaderboard.Add(
            new Rank(name, score)
        );
        return true;
    }
}