/*
 * Hanlin Zhang
 * Purpose: Manages audio and music
 * 
 * This script was inspired by "SAVE & LOAD SYSTEM in Unity" by "Brackeys" 2018.
 * https://www.youtube.com/watch?v=XOjd_qU2Ido
 */

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class fileManager : MonoBehaviour {
    public void loadDefaults() {
        if(!File.Exists(Paths.leaderboard)) {
            saveData<Leaderboard>(Paths.leaderboard, new Leaderboard());
        }

        if (!File.Exists(Paths.userSettings)) {
            saveData<UserSettings>(Paths.userSettings, new UserSettings());
        }

        if (!File.Exists(Paths.userProgress)) {
            saveData<UserProgress>(Paths.userProgress, new UserProgress());
        }
    }

    // Method for adding a new placement to the leaderboard
    // Takes in a name and its corresponding score to add
    // Returns true if the name doesn't already exist in the leaderboard
    public bool saveLeaderboard(string name, int score) {
        // Variable for storing the leaderboard
        Leaderboard leaderboard;

        // Boolean for keeping track of if the name already exists
        bool isNewEntry;

        // Checks if the leaderboard already exists, 
        if (File.Exists(Paths.leaderboard)) {
            // Load the current leaderboard
            leaderboard = loadLeaderboard();
        } else {
            // Define a new leaderboard
            leaderboard = new Leaderboard();
        }

        // Add the new rank and keep track of if the name already exists
        isNewEntry = leaderboard.NewEntry(name, score);

        saveData<Leaderboard>(Paths.leaderboard, leaderboard);

        // Return true if a new entry was added
        // Return false if a preexisting entry was updated
        return isNewEntry;
    }

    // Method for loading the leaderboard
    public Leaderboard loadLeaderboard() {
        return loadData<Leaderboard>(Paths.leaderboard);
    }

    public void saveUserSettings(UserSettings userSettings) {
        saveData<UserSettings>(Paths.userSettings, userSettings);
    }

    public UserSettings loadUserSettings() {
        return loadData<UserSettings>(Paths.userSettings);
    }

    public void saveUserProgress(UserProgress userProgress) {
        saveData<UserProgress>(Paths.userProgress, userProgress);
    }

    public UserProgress loadUserProgress() {
        return loadData<UserProgress>(Paths.userProgress);
    }

    private void saveData<T>(string path, T data) {
        // Create a binary formatter to encrypt our leaderboard
        BinaryFormatter formatter = new BinaryFormatter();
        // Create a filestream to write to our file
        FileStream stream = new FileStream(path, FileMode.Create);

        // Save the file and close the string
        formatter.Serialize(stream, data);
        stream.Close();
    }

    private T loadData<T>(string path) where T : class, new() {
        // Check if the file exists
        if (File.Exists(path)) {
            // Create a binary formatter for encrypting our data
            BinaryFormatter formatter = new BinaryFormatter();
            // Create a filestreaem for reading the file
            FileStream stream = new FileStream(path, FileMode.Open);

            // Variable for storing retrieved data
            T data;
            // Make sure the leaderboard is not empty, if it is, create an empty data
            if (stream.Length != 0) {
                // Read the leaderboard and store it in the leaderboard variable
                data = formatter.Deserialize(stream) as T;
            } else {
                data = new T();
                Debug.LogError($"Save file: \"{path}\" is empty");
            }

            // Close the file stream and return the leaderboard
            stream.Close();
            return data;
        } else {
            // Return nothing and log an error
            Debug.LogError($"Save file: \"{path}\" not found");
            return new T();
        }
    }
}

public static class Paths {
    public static readonly string leaderboard = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "leaderboard.fbla";
    public static readonly string userSettings = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "userSettings.fbla";
    public static readonly string userProgress = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "userProgress.fbla";
}