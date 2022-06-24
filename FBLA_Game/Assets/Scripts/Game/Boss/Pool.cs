using UnityEngine;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     Pool for a specific object.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/18/2022
    /// </remarks>
    [System.Serializable]
    internal class Pool
    {
        /// <summary>
        ///     Reference to the pool.
        /// </summary>
        [field: 
            SerializeField,
            Tooltip("If the name is not present, add it to PoolTags.cs")]
        internal PoolTags Tag { get; private set; }

        /// <summary>
        ///     The gameobject that the pool contains.
        /// </summary>
        [field: SerializeField]
        internal GameObject Prefab { get; private set; }

        /// <summary>
        ///     The size of the pool; how many objects it contains.
        /// </summary>
        [field: 
            SerializeField,
            Tooltip("The size of the pool; how many objects it contains.")]
        internal int Size { get; private set; }
    }
}

