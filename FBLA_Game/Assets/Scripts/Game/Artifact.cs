using System;
using System.Collections;
using UnityEngine;
using JonathansAdventure.Sound;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Base class for all artifacts.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/18/2022
    /// </remarks>
    public abstract class Artifact : MonoBehaviour
    {

        private protected abstract void afterCollect();
        private protected Transform finalParent;
        private protected Vector3 endPos;

        #region Settings

        [Header("Settings")]

        [SerializeField,
            Tooltip("The sorting layer the artifact goes to during its transition animation.")]
        private SortingLayer preSortingLayer;

        [SerializeField,
            Tooltip("The sorting layer the artifact goes to after it is done transitioning.")]
        private SortingLayer postSortingLayer;

        [SerializeField,
            Range(0f, 2f),
            Tooltip("The time the transition animation takes.")]
        private float duration;

        #endregion

        #region References

        [Header("Object/Prefab References")]

        [SerializeField] private Transform trans;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] sprites = new Sprite[3];
        [SerializeField] private BoxCollider2D artifactCollider;

        private SoundManager soundManager;
        private CapsuleCollider2D playerCollider;

        #endregion

        /// <summary>
        ///     Animates the artifact's movement after it is collected.
        /// </summary>
        /// <param name="parent"> The parent object to place the artifact under. </param>
        /// <param name="endPos"> The position the artifact should be by the end of the animation. </param>
        /// <param name="callback"> Callback action to call once the animation is finished. </param>
        private IEnumerator OnCollectCoroutine()
        {
            soundManager.PlaySound(SoundNames.ArtifactCollect);
            // Edit sorting layer to prepare for animation.
            spriteRenderer.sortingLayerID = preSortingLayer.id;

            // Change the artifact's parent object.
            trans.SetParent(finalParent);

            // Set starting position relative to the parent.
            Vector3 startPos = trans.localPosition;

            // Set time to 0.
            float t = 0;

            // Loop until the duration time is met.
            while (t < duration)
            {
                // Add elapsed time.
                t += Time.deltaTime;
                // Move teh artifact
                trans.localPosition = Vector3.Lerp(startPos, endPos, t / duration);
                // Needed for loop
                yield return null;
            }

            // Edit sorting layer for the termination of the animation.
            spriteRenderer.sortingLayerID = postSortingLayer.id;
            // Calls the callback while guarding against null references.
            afterCollect();
        }

        private protected virtual void Start()
        {
            // Select a random sprite
            spriteRenderer.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];

            // Find player
            playerCollider = FindObjectOfType<CapsuleCollider2D>();
        }

        private protected virtual void Update()
        {
            if (
                (Input.GetKeyDown(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow)) &&
                artifactCollider.IsTouching(playerCollider)
            )
            {
                StartCoroutine(OnCollectCoroutine());
            }
        }
    }
}