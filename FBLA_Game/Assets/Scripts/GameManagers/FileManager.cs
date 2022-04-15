/*
 * Hanlin Zhang
 * Purpose: Manages encrypting and decrypting data
 * 
 * This script was inspired by "SAVE & LOAD SYSTEM in Unity" by "Brackeys" 2018.
 * Credits for SaveData and LoadData to "Brackeys"
 * https://www.youtube.com/watch?v=XOjd_qU2Ido
 */

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class FileManager : MonoBehaviour {
    // Method for loading the default settings if no settings have been set yet
    public void LoadDefaults() {
        if(!File.Exists(Paths.leaderboard)) {
            SaveData<Leaderboard>(Paths.leaderboard, new Leaderboard());
        }

        if (!File.Exists(Paths.userSettings)) {
            SaveData<UserSettings>(Paths.userSettings, new UserSettings());
        }

        if (!File.Exists(Paths.userProgress)) {
            SaveData<UserProgress>(Paths.userProgress, new UserProgress());
        }
    }

    // Method for adding a new placement to the leaderboard
    // Params: name (user name), score (user score)
    // Returns true if the name doesn't already exist in the leaderboard
    public bool SaveLeaderboard(string name, int score) {
        Leaderboard leaderboard;

        bool isNewEntry;

        // Checks if the leaderboard already exists, 
        if (File.Exists(Paths.leaderboard)) {
            leaderboard = LoadLeaderboard();
        } else {
            leaderboard = new Leaderboard();
        }

        isNewEntry = leaderboard.NewEntry(name, score);

        SaveData<Leaderboard>(Paths.leaderboard, leaderboard);

        // Return true if a new entry was added
        // Return false if a preexisting entry was updated
        return isNewEntry;
    }

    // Method for loading the leaderboard
    public Leaderboard LoadLeaderboard() {
        return LoadData<Leaderboard>(Paths.leaderboard);
    }
    
    // Method for saving user settings
    public void SaveUserSettings(UserSettings userSettings) {
        SaveData<UserSettings>(Paths.userSettings, userSettings);
    }

    // Method for loading user settings
    public UserSettings LoadUserSettings() {
        return LoadData<UserSettings>(Paths.userSettings);
    }

    // Method for saving user progress
    public void SaveUserProgress(UserProgress userProgress) {
        SaveData<UserProgress>(Paths.userProgress, userProgress);
    }

    // Method for loading user progress
    public UserProgress LoadUserProgress() {
        return LoadData<UserProgress>(Paths.userProgress);
    }

    // Method for saving data
    // Params: T (data class type), path (file path and name), data (data to save)
    private void SaveData<T>(string path, T data) {
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, data);
        stream.Close();
    }

    // Method for loading data
    // Params: T (data type to load), path (file path and name)
    // Return: data from path
    // The data type, T, has to be a class and has to have a new() constructor
    private T LoadData<T>(string path) where T : class, new() {
        if (File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            T data;

            // Make sure the leaderboard is not empty, if it is, create an empty data
            if (stream.Length != 0) {
                data = formatter.Deserialize(stream) as T;
            } else {
                data = new T();
                Debug.LogError($"Save file: \"{path}\" is empty");
            }

            stream.Close();
            return data;
        } else {
            Debug.LogError($"Save file: \"{path}\" not found");
            return new T();
        }
    }
}

// static class of strings for the different file paths
public static class Paths {
    public static readonly string leaderboard = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "leaderboard.fbla";
    public static readonly string userSettings = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "userSettings.fbla";
    public static readonly string userProgress = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "userProgress.fbla";
}