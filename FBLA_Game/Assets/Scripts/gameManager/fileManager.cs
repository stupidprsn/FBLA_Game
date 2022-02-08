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
    // File location for storing the leaderboard
    // It is stored in %userprofile%\AppData\LocalLow\PeriwinkleGames\Jonathans_Adventure
    private string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "leaderboard.fbla";

    // Method for adding a new placement to the leaderboard
    // Takes in a name and its corresponding score to add
    // Returns true if the name doesn't already exist in the leaderboard
    public bool saveLeaderboard(string name, int score) {
        // Variable for storing the leaderboard
        Leaderboard leaderboard;

        // Boolean for keeping track of if the name already exists
        bool isNewEntry;

        // Checks if the leaderboard already exists, 
        if (File.Exists(path)) {
            // Load the current leaderboard
            leaderboard = loadLeaderboard();

        } else {
            // Define a new leaderboard
            leaderboard = new Leaderboard();
        }

        // Add the new rank and keep track of if the name already exists
        isNewEntry = leaderboard.newEntry(name, score);

        // Create a binary formatter to encrypt our leaderboard
        BinaryFormatter formatter = new BinaryFormatter();
        // Create a filestream to write to our file
        FileStream stream = new FileStream(path, FileMode.Create);

        // Save the file and close the string
        formatter.Serialize(stream, leaderboard);
        stream.Close();

        // Return true if a new entry was added
        // Return false if a preexisting entry was updated
        return isNewEntry;
    }

    // Method for loading the leaderboard
    public Leaderboard loadLeaderboard() {
        // Check if the file exists
        if(File.Exists(path)) {
            // Create a binary formatter for encrypting our leaderboard
            BinaryFormatter formatter = new BinaryFormatter();
            // Create a filestreaem for reading the file
            FileStream stream = new FileStream(path, FileMode.Open);

            // Variable for storing our leaderboard
            Leaderboard leaderboard;
            // Make sure the leaderboard is not empty, if it is, create an empty leaderboard
            if(stream.Length != 0) {
                // Read the leaderboard and store it in the leaderboard variable
                leaderboard = formatter.Deserialize(stream) as Leaderboard;
            } else {
                leaderboard = new Leaderboard();
            }

            // Close the file stream and return the leaderboard
            stream.Close();
            return leaderboard;
        } else {
            // Return nothing and log an error
            Debug.LogError("Save file not found");
            return null;
        }
    }
}
