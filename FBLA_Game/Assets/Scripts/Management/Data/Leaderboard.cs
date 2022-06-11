using System.Collections.Generic;

namespace JonathansAdventure.Data
{
    /// <summary>
    ///     A list of <see cref="Rank"/>.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/5/2022
    /// </remarks>
    [System.Serializable]
    public class Leaderboard
    {
        /// <summary>
        ///     A list of <see cref="Rank"/> that stores the leaderboard data.
        /// </summary>
        internal List<Rank> Data { get; private set; }

        /// <summary>
        ///     Primary constructor for <see cref="leaderboard"/>.
        /// </summary>
        public Leaderboard()
        {
            Data = new List<Rank>();
        }

        /// <summary>
        ///     Adds a new rank to the leaderboard
        /// </summary>
        /// <param name="name"> The display name of the new rank </param>
        /// <param name="score"> The game score of the new rank </param>
        /// <returns>
        ///     If the user has recieved a new high score.
        /// </returns>
        internal bool NewEntry(string name, int score)
        {
            // Checks if the name already exists
            // If it does, update the score for the corresponding name if the score is higher
            foreach (Rank rank in Data)
            {
                if (rank.Name == name)
                {
                    // Checks if the score is higher
                    if (score > rank.Score)
                    {
                        rank.Score = score;
                    }
                    return false;
                }
            }

            // If the name doesn't exist in the leaderboard yet, add it
            Data.Add(
                new Rank(name, score)
            );
            return true;
        }
    }
}