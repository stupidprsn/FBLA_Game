using UnityEngine;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Manages the artifact holder, as in the object that
    ///     holds the artifacts after they are collected.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/14/2022
    /// </remarks>
    public class ArtifactHolder : MonoBehaviour
    {

        #region References
        [Header("Object/Prefab References")]

        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private OpenedDoor openedDoor;

        #endregion

        #region Settings

        [field: Header("Settings")]

        /// <summary>
        ///     Total number of artifacts in the stage.
        /// </summary>
        [field:
            SerializeField,
            Tooltip("Set the total number of artifacts in the stage.")]
        internal int TotalArtifacts { get; }

        #endregion

        /// <summary>
        ///     Number of artifacts that the user has collected.
        /// </summary>
        internal int CollectedArtifacts { get; private set; }

        /// <summary>
        ///     Updates <see cref="CollectedArtifacts"/> and checks to see
        ///     if all the artifacts have been collected.
        /// </summary>
        internal void ArtifactCollected()
        {
            CollectedArtifacts++;
            if (CollectedArtifacts == TotalArtifacts)
            {
                SoundManager.Instance.PlaySound(SoundNames.DoorOpen);
                // Open the door.
                openedDoor.enabled = true;
                
            }

        }

        /// <summary>
        ///     Change the number of artifact slots displayed to match 
        ///     <see cref="TotalArtifacts"/>.
        /// </summary>
        private void OnValidate()
        {
            // Each "slot" is 0.16f.
            spriteRenderer.size = new Vector2(0.16f * TotalArtifacts, 0.16f);
        }
    }
}