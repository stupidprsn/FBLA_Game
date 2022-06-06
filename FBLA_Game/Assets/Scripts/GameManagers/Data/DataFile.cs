using System.IO;
using UnityEngine;

namespace JonathansAdventure.GameManagers.Data
{
    /// <summary>
    ///     Manages path and creates save/load methods.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/5/2022
    /// </remarks>
    /// <typeparam name="T">
    ///     The data class stored in the file.
    /// </typeparam>
    internal class DataFile<T> where T : class, new()
    {
        /// <summary>
        ///     The absolute file path.
        /// </summary>
        private protected readonly string path;

        /// <summary>
        ///     Constructor for DataFile class. 
        /// </summary>
        /// <param name="fileName">
        ///     Name of the file.
        /// </param>
        internal DataFile(string fileName)
        {
            // Generates path name in the persistent data path folder (LocalLow in windows)
            path = Path.Combine(Application.persistentDataPath, fileName);
        }

        /// <summary>
        ///     Saves data to the file.
        /// </summary>
        /// <param name="data">
        ///     Data to be saved.
        /// </param>
        internal void save(T data)
        {
            FileManager.SaveData(path, data);
        }

        /// <summary>
        ///     Returns the data from the file.
        /// </summary>
        /// <returns>
        ///     Data in the format of data class <see cref="T"/>.
        /// </returns>
        internal T load() => FileManager.LoadData<T>(path);

        /// <summary>
        ///     Checks if the file exists yet, and if it doesn't
        ///     it creates the file with the default data.
        /// </summary>
        internal void saveDefault()
        {
            if (!File.Exists(path))
            {
                save(new T());
            }
        }
    }
}