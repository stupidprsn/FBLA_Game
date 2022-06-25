namespace JonathansAdventure.Data
{
    /// <summary>
    ///     Stores which level the user has reached.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     6/13/2022
    /// </remarks>
    [System.Serializable]
    public class UserProgress
    {
        /// <summary>
        ///     The level the user has reached.
        /// </summary>
        public int Level { get; set; }

        /// <summary>
        ///     Primary constructor.
        /// </summary>
        /// <param name="level"> The level the user has reached. </param>
        public UserProgress(int level)
        {
            this.Level = level;
        }

        /// <summary>
        ///     Sets level to default (1).
        /// </summary>
        public UserProgress()
        {
            this.Level = 1;
        }
    }
}

