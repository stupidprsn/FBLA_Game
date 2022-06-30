using UnityEngine;
using TMPro;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     References to the elements of the "ranking" prefab.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/25/2022
    /// </remarks>
    public class Ranking : MonoBehaviour
    {
        /// <summary>
        ///     The rank's placement.
        /// </summary>
        [field: SerializeField]
        internal TMP_Text Place { get; private set; }

        /// <summary>
        ///     The rank's name.
        /// </summary>
        [field: SerializeField]
        internal TMP_Text Name { get; private set; }

        /// <summary>
        ///     The rank's score.
        /// </summary>
        [field: SerializeField]
        internal TMP_Text Score { get; private set; }
    }
}
