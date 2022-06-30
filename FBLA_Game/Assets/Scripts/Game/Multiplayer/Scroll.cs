using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace JonathansAdventure.Game.Multiplayer
{
    /// <summary>
    ///     Manages scrolling in endless mode.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/26/2022
    /// </remarks>
    public class Scroll : MonoBehaviour
    {
        #region Settings

        [Header("Settings")]

        /// <summary>
        ///     The delay before the platforms start moving.
        /// </summary>
        [SerializeField,
            Range(0f, 10f),
            Tooltip("The delay before the platforms start moving.")]
        private float delay;

        /// <summary>
        ///     The distance the platform moves down each second.
        /// </summary>
        [SerializeField,
            Range(0f, 0.001f),
            Tooltip("The distance the platform moves down each second.")]
        private float scrollSpeed;

        [SerializeField,
            Range(0f, 0.1f),
            Tooltip("The max speed that ramping stops at.")]
        private float maxSpeed;

        #endregion

        /// <summary>
        ///     This transform.
        /// </summary>
        private Transform trans;

        /// <summary>
        ///     Scrolls down.
        /// </summary>
        /// <returns> null </returns>
        private IEnumerator Move()
        {
            yield return new WaitForSeconds(delay);
            StartCoroutine(Ramping());
            while (true)
            {
                trans.position = new Vector3(0, trans.position.y - scrollSpeed, 0);
                yield return null;
            }
        }

        /// <summary>
        ///     Ramps up the difficulty by slowly, but exponentially,
        ///     increasing the speed of the scroll.
        /// </summary>
        /// <returns></returns>
        private IEnumerator Ramping()
        {
            while(true)
            {
                yield return new WaitForSeconds(1);
                scrollSpeed *= 1.005f;
                if (scrollSpeed >= maxSpeed) break;
            }
        }

        /// <summary>
        ///     Set reference.
        /// </summary>
        private void Awake()
        {
            trans = transform;
        }

        /// <summary>
        ///     Start the scroll.
        /// </summary>
        private void Start()
        {
            StartCoroutine(Move());
        }
    }

}
