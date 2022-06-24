using UnityEngine;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Stores various <see cref="Component"/> of the player.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Used to cache references as <see cref="GameObject.GetComponent(System.Type)"/>
    ///         is expensive.
    ///     </para>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/23/2022
    ///     </para>
    /// </remarks>
    [System.Serializable]
    internal class Player
    {
        internal GameObject PlayerObject { get; private set; }
        internal Transform Transform { get; private set; }
        internal Rigidbody2D PlayerRB { get; private set; }
        internal CapsuleCollider2D PlayerCollider { get; private set; }
        internal PlayerMovement PlayerMovement { get; private set; }
        internal PlayerAnimation PlayerAnimation { get; private set; }
        internal Vector3 SpawnPosition { get; private set; }

        /// <summary>
        ///     Main constructor that finds the current player object.
        /// </summary>
        internal Player() { }

        /// <summary>
        ///     Find and cache references to the parts of the player.
        /// </summary>
        internal void SetReferences()
        {
            PlayerObject = GameObject.FindWithTag("Player");
            Transform = PlayerObject.transform;
            PlayerRB = PlayerObject.GetComponent<Rigidbody2D>();
            PlayerCollider = PlayerObject.GetComponent<CapsuleCollider2D>();
            PlayerMovement = PlayerObject.GetComponent<PlayerMovement>();
            PlayerAnimation = PlayerObject.GetComponent<PlayerAnimation>();
            SpawnPosition = Transform.position;
        }
    }
}
