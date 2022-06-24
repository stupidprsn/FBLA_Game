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
    public abstract class Artifact : Interactable
    {

        protected abstract void AfterCollect();

        protected abstract Transform FinalParent { get; }
        protected abstract Vector3 EndPos { get; }

        #region Settings

        [Header("Settings")]

        [SerializeField,
            Tooltip("The sorting layer the artifact goes to during its transition animation.")]
        private string preSortingLayer;


        [SerializeField,
            Tooltip("The sorting layer the artifact goes to after it is done transitioning.")]
        private string postSortingLayer;

        [SerializeField,
            Range(0f, 2f),
            Tooltip("The time the transition animation takes.")]
        private float duration;

        #endregion

        #region References

        [Header("Object/Prefab References")]

        [SerializeField] protected Transform trans;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Sprite[] sprites = new Sprite[3];
        [SerializeField] private BoxCollider2D artifactCollider;

        protected SoundManager soundManager;

        #endregion

        /// <summary>
        ///     Animates the artifact's movement after it is collected.
        /// </summary>
        private IEnumerator OnCollectCoroutine()
        {
            soundManager.PlaySound(SoundNames.ArtifactCollect);
            // Edit sorting layer to prepare for animation.
            spriteRenderer.sortingLayerName = preSortingLayer;

            // Change the artifact's parent object.
            trans.SetParent(FinalParent);

            // Set positions relative to the parent.
            Vector3 startPos = trans.localPosition;
            Vector3 endPos = EndPos;

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
            spriteRenderer.sortingLayerName = postSortingLayer;
            // Calls the callback while guarding against null references.
            AfterCollect();
        }

        protected override void OnInteract(PlayerMovement player)
        {
            StartCoroutine(OnCollectCoroutine());
        }

        /// <summary>
        ///     Select a random sprite for the artifact.
        /// </summary>
        protected virtual void Awake()
        {
            spriteRenderer.sprite = sprites[UnityEngine.Random.Range(0, sprites.Length)];
        }

        /// <summary>
        ///     Cache Reference.
        /// </summary>
        protected virtual void Start()
        {
            soundManager = SoundManager.Instance;
        }
    }
}