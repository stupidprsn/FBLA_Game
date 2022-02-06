using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class fileManager {
    private static string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "leaderboard.ilovesophie";

    public static bool saveLeaderboard(string name, int score) {
        Leaderboard leaderboard;

        bool isNewEntry;

        // Call previous leaderboard and stuff
        if (File.Exists(path)) {
            leaderboard = loadLeaderboard();
            isNewEntry = leaderboard.newEntry(name, score);
        } else {
            leaderboard = new Leaderboard();
            isNewEntry = leaderboard.newEntry(name, score);
        }

        BinaryFormatter formatter = new BinaryFormatter();
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, leaderboard);
        stream.Close();

        return isNewEntry;
    }

    public static Leaderboard loadLeaderboard() {
        if(File.Exists(path)) {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            Leaderboard leaderboard;
            if(stream.Length != 0) {
                leaderboard = formatter.Deserialize(stream) as Leaderboard;
            } else {
                leaderboard = new Leaderboard();
            }


            stream.Close();
            return leaderboard;
        } else {
            Debug.LogError("Save file not found");
            return null;
        }
    }
}
