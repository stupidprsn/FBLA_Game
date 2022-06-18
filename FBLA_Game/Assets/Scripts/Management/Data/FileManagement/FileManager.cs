/*
 * Hanlin Zhang
 * Purpose: Manages encrypting and decrypting data
 * 
 * Part of this script was inspired by "SAVE & LOAD SYSTEM in Unity" 
 * by "Asbjørn Thirslund (Brackeys)" 2018.
 * Credits for SaveData and LoadData to "Brackeys"
 * https://www.youtube.com/watch?v=XOjd_qU2Ido
 */

using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace JonathansAdventure.Data
{
    /// <summary>
    ///     Manages serializing and deserializing data 
    ///     to and from a non volatile state.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This class was inspired by "SAVE & LOAD SYSTEM in Unity" 
    ///         by "Asbjørn Thirslund (Brackeys)" 2018.
    ///         
    ///         The members: <see cref="SaveData{T}(string, T)"/> and part of 
    ///         <see cref="LoadData{T}(string)"/> are creddited to Brackeys.
    ///         
    ///         <seealso href="https://www.youtube.com/watch?v=XOjd_qU2Ido"/>
    ///     </para>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/13/2022
    ///     </para>
    /// </remarks>
    public class FileManager : MonoBehaviour
    {
        /// <summary>
        ///     Leaderboard data for singleplayer.
        /// </summary>
        public LeaderboardDataFile SingleLeaderboard { get; private set; }

        /// <summary>
        ///     Leaderboard data for multiplayer.
        /// </summary>
        public LeaderboardDataFile MultiLeaderboard { get; private set; }

        /// <summary>
        ///     Data regarding what level the user has reached.
        /// </summary>
        public DataFile<UserProgress> UserProgressData { get; private set; }

        private void Awake()
        {
            SingleLeaderboard = new LeaderboardDataFile("singleLeaderboard.fbla");
            MultiLeaderboard = new LeaderboardDataFile("multiLeaderboard");
            UserProgressData = new DataFile<UserProgress>("userProgress.fbla");
        }

        /// <summary>
        ///     Load the default settings if no settings have been set yet.
        /// </summary>
        public void LoadDefaults()
        {
            SingleLeaderboard.SaveDefault();
            MultiLeaderboard.SaveDefault();
            UserProgressData.SaveDefault();
        }

        /// <summary>
        ///     Serializes and saves data.
        /// </summary>
        /// <typeparam name="T"> Type of data </typeparam>
        /// <param name="path"> Path to save it to </param>
        /// <param name="data"> The data to save </param>
        public static void SaveData<T>(string path, T data)
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Create);

            formatter.Serialize(stream, data);
            stream.Close();
        }

        /// <summary>
        ///     Deserializes and loads data.
        /// </summary>
        /// <typeparam name="T"> Type of data </typeparam>
        /// <param name="path"> Path where data is saved at </param>
        /// <returns> The data </returns>
        public static T LoadData<T>(string path) where T : class, new()
        {
            if (File.Exists(path))
            {
                BinaryFormatter formatter = new BinaryFormatter();
                FileStream stream = new FileStream(path, FileMode.Open);

                T data;

                // Make sure the leaderboard is not empty, if it is, create an empty data
                if (stream.Length != 0)
                {
                    data = formatter.Deserialize(stream) as T;
                }
                else
                {
                    data = new T();
                    Debug.LogError($"Save file: \"{path}\" is empty");
                }

                stream.Close();
                return data;
            }
            else
            {
                Debug.LogError($"Save file: \"{path}\" not found");
                return new T();
            }
        }
    }
}