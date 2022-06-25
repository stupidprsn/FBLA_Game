namespace JonathansAdventure.Data
{
    /// <summary>
    ///     Rank Class for each individual ranking on the leaderboard
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/5/2022
    /// </remarks>
    [System.Serializable]
    public class Rank
    {
        /// <summary>
        ///     The display name.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        ///     The game score.
        /// </summary>
        public int Score { get; set; }

        /// <summary>
        ///     Primary constructor for <see cref="Rank"/>.
        /// </summary>
        /// <param name="name"> The display name </param>
        /// <param name="score"> The game score </param>
        public Rank(string name, int score)
        {
            Name = name;
            Score = score;
        }
    }
}
