/*
 * Hanlin Zhang
 * Purpose: Sets up classes used to manage data
 */

using UnityEngine;
using System.IO;

/// <summary>
///     Manages path and creates save/load methods
/// </summary>
/// <typeparam name="T">
///     The data class stored in the file
/// </typeparam>
public class DataFile<T> where T : class, new() {
    /// <summary>
    ///     The file path
    /// </summary>
    public string Path { get; private set; }

    /// <summary>
    ///     The data class defined by "T"
    /// </summary>
    public System.Type getDataType => typeof(T);

    /// <summary>
    ///     Constructor for DataFile class. Generates path name
    ///     by finding the persistent data path folder (LocalLow in windows)
    /// </summary>
    /// <param name="fileName">
    ///     Name of the file is stored in the persistent data path folder
    /// </param>
    public DataFile(string fileName) {
        Path = System.IO.Path.Combine(Application.persistentDataPath, fileName);
    }

    /// <summary>
    ///     Saves the data to the file 
    /// </summary>
    /// <param name="data">
    ///     Data to be saved
    /// </param>
    public void save(T data) {
        FileManager.SaveData(Path, data);
    }

    /// <summary>
    ///     Returns the data from the file
    /// </summary>
    /// <returns>
    ///     Data in data class T
    /// </returns>
    public T load() => FileManager.LoadData<T>(Path);

    /// <summary>
    ///     Checks if the file exists yet, and if it doesn't
    ///     it creates the file with the default data
    /// </summary>
    public void saveDefault() {
        if (!File.Exists(Path)) {
            save(new T());
        }
    }
}

/// <summary>
///     Special data file for the leaderboard data.
///     It incorporates an extra method that allows
///     a new ranking to be appended
/// </summary>
public class LeaderboardDataFile : DataFile<Leaderboard> {
    /// <summary>
    ///     Same constructor as base DataFile class <see cref="DataFile{T}"/>
    /// </summary>
    /// <param name="fileName"></param>
    public LeaderboardDataFile(string fileName) : base(fileName) { }

    /// <summary>
    ///     Adds a new ranking to the leaderboard
    /// </summary>
    /// <param name="name"> User Name </param>
    /// <param name="score"> User Score </param>
    /// <returns> 
    ///     Returns true if the player has recieved a 
    ///     new high score or if the name is new
    /// </returns>
    public bool SaveEntry(string name, int score) {
        Leaderboard leaderboard;

        bool isNewEntry;

        // Checks if the leaderboard already exists, 
        if (File.Exists(base.Path)) {
            leaderboard = base.load();
        } else {
            leaderboard = new Leaderboard();
        }

        isNewEntry = leaderboard.NewEntry(name, score);

        FileManager.SaveData(base.Path, leaderboard);

        return isNewEntry;
    }
}