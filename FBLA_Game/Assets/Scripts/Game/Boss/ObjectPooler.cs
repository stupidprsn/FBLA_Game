using System.Collections.Generic;
using UnityEngine;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     Manages object pooling.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Used to save memory and enhance performance in 
    ///         boss levels.
    ///     </para>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/18/2022
    ///     </para>
    ///     <para>
    ///         This class was inspired by "OBJECT POOLING in Unity"
    ///         by "Asbjørn Thirslund (Brackeys)" 2018.
    ///         <seealso href="https://www.youtube.com/watch?v=tdSmKaJvCoA"/>
    ///     </para>
    /// </remarks>
    public class ObjectPooler : MonoBehaviour
    {

        [SerializeField,
            Tooltip("Set up the object pools. If the tag cannot be found, add it to PoolTags.cs")] 
        private Pool[] pools;
        
        /// <summary>
        ///     A dictionary associating a <see cref="PoolTags"/> to
        ///     a <see cref="Queue{T}"/> of those objects.
        /// </summary>
        private Dictionary<PoolTags, Queue<GameObject>> poolDict;

        /// <summary>
        ///     Fill up the object pools.
        /// </summary>
        private void Awake()
        {
            // Assign a new dictionary.
            poolDict = new Dictionary<PoolTags, Queue<GameObject>>(pools.Length);

            // Traverse through the pools.
            foreach(Pool pool in pools)
            {
                // Create a queue for the pool.
                Queue<GameObject> objectPool = new Queue<GameObject>();

                // Fill the pool with its specified prefab and size.
                for (int i = 0; i < pool.Size; i++)
                {
                    GameObject obj = Instantiate(pool.Prefab);
                    obj.SetActive(false);
                    objectPool.Enqueue(obj);
                }

                // Add the pool to the dictionary.
                poolDict.Add(pool.Tag, objectPool);
            }
        }

        /// <summary>
        ///     Spawns an object from its pool.
        /// </summary>
        /// <param name="tag"> The tag that references the object's pool. </param>
        /// <param name="position"> The position to spawn the object at. </param>
        /// <param name="parent"> The parent object to place the spawned object under. </param>
        /// <returns> A reference to the spawned object. </returns>
        internal GameObject Spawn(PoolTags tag, Vector3 position, Transform parent)
        {
            // Take the object from the queue.
            GameObject objToSpawn = poolDict[tag].Dequeue();

            objToSpawn.SetActive(true);

            // Cache transform.
            Transform objTransform = objToSpawn.transform;
            objTransform.SetParent(parent, false);
            objTransform.SetPositionAndRotation(position, Quaternion.identity);

            // Return object to the pool
            poolDict[tag].Enqueue(objToSpawn);

            return objToSpawn;
        }
    }
}
