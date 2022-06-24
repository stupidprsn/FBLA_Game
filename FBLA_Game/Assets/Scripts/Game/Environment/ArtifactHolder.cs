using UnityEngine;
using System.Collections.Generic;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Manages the artifact holder, as in the object that
    ///     holds the artifacts after they are collected.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/23/2022
    /// </remarks>
    public class ArtifactHolder : MonoBehaviour
    {
        /// <summary>
        ///     A reference to the artifact holder.
        /// </summary>
        public static ArtifactHolder Instance;

        #region References

        private OpenedDoor openedDoor;
        private SoundManager soundManager;

        #endregion

        #region Settings

        [field: Header("Settings")]

        /// <summary>
        ///     Total number of artifacts in the stage.
        /// </summary>
        [field:
            SerializeField,
            Tooltip("Set the total number of artifacts in the stage.")]
        private int totalArtifacts;

        #endregion

        /// <summary>
        ///     Number of artifacts that the user has collected.
        /// </summary>
        private int collectedArtifacts;

        /// <summary>
        ///     The x coordinates of the remaining slots in the holder.
        /// </summary>
        /// <remarks>
        ///     Only the x coordinate has to be saved as y and z are both 0.
        /// </remarks>
        private Queue<float> xPositions = new Queue<float>();

        /// <summary>
        ///     calculates and returns the position the next artifact should go to.
        /// </summary>
        /// <returns>
        ///     The next artifact position.
        /// </returns>
        internal Vector3 GetNextPosition()
        {
            return new Vector3(xPositions.Dequeue(), 0, 0);
        }

        /// <summary>
        ///     Updates <see cref="CollectedArtifacts"/> and checks to see
        ///     if all the artifacts have been collected.
        /// </summary>
        internal void ArtifactCollected()
        {
            collectedArtifacts++;
            if (collectedArtifacts == totalArtifacts)
            {
                soundManager.PlaySound(SoundNames.DoorOpen);
                // Open the door.
                openedDoor.enabled = true;
                
            }

        }

        /// <summary>
        ///     Calculate and fill <see cref="xPositions"/>.
        /// </summary>
        private void Awake()
        {
            // Create singleton.
            Instance = this;

            xPositions = new Queue<float>();

            // Keep track of the x coordinate.
            float x;

            // If there are an odd number of slots, the coordinate 0 is the center.
            // Otherwise, if there are an even number of slots,
            // start at 0.08 (half of a slot) which is the slot just right of the center.
            if(totalArtifacts % 2 == 1)
            {
                x = 0;
            } else
            {
                x = 0.08f;
            }

            // Subtract a slot for each 2 total slots to reach the leftmost slot.
            x -= 0.16f * Mathf.FloorToInt(totalArtifacts / 2);
            xPositions.Enqueue(x);

            // For each remaining slot, add 0.16.
            for(int i = 0; i < totalArtifacts - 1; i++)
            {
                x += 0.16f;
                xPositions.Enqueue(x);
            }
        }

        /// <summary>
        ///     Cache reference.
        /// </summary>
        private void Start()
        {
            soundManager = SoundManager.Instance;
            openedDoor = GameObject.FindWithTag("Door").GetComponent<OpenedDoor>();
        }
    }
}