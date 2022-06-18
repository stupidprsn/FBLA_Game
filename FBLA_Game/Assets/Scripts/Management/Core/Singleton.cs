using UnityEngine;

namespace JonathansAdventure
{
    /// <summary>
    ///     Incorporates singleton pattern.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/13/2022
    /// </remarks>
    /// <typeparam name="T"> Derived class </typeparam>
    public abstract class Singleton<T> : MonoBehaviour where T : class
    {
        /// <summary>
        ///     Reference to the <see cref="T"/>.
        /// </summary>
        [HideInInspector] public static T Instance { get; private set; }

        /// <summary>
        ///     Singleton pattern check to make sure only
        ///     one singleton exists.
        /// </summary>
        /// <remarks>
        ///     Call from <c> Awake() </c>.
        /// </remarks>
        /// <param name="thisObject"> Pass in <see cref="this"/> </param>
        protected void SingletonCheck(T thisObject)
        {
            if (Instance != null && Instance != thisObject)
            {
                Destroy(gameObject);
            }
            else
            {
                Instance = thisObject;
                DontDestroyOnLoad(gameObject);
            }
        }
    }
}