﻿using System.IO;

namespace JonathansAdventure.GameManagers.Data
{
    /// <summary>
    ///     Special data file for the data of the <see cref="Leaderboard"/> class.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/5/2022
    ///     </para>
    ///     <para>
    ///         It incorporates an extra method that allows a new ranking to be appended
    ///     </para>
    /// </remarks>
    internal class LeaderboardDataFile : DataFile<Leaderboard>
    {
        /// <summary>
        ///     Same constructor as base DataFile class: <see cref="DataFile{T}"/>
        /// </summary>
        /// <param name="fileName">
        ///     See: <see cref="DataFile{T}"/>
        /// </param>
        public LeaderboardDataFile(string fileName) : base(fileName) { }

        /// <summary>
        ///     Adds a new ranking to the leaderboard.
        /// </summary>
        /// <param name="name"> User Name </param>
        /// <param name="score"> User Score </param>
        /// <returns> 
        ///     Returns true if the player has recieved a 
        ///     new high score or if the name is new.
        /// </returns>
        public bool SaveEntry(string name, int score)
        {
            Leaderboard leaderboard;

            bool isNewEntry;

            // Checks if the leaderboard already exists, 
            if (File.Exists(base.path))
            {
                leaderboard = base.load();
            } else
            {
                leaderboard = new Leaderboard();
            }

            isNewEntry = leaderboard.NewEntry(name, score);

            FileManager.SaveData(base.path, leaderboard);

            return isNewEntry;
        }
    }
}