using UnityEngine;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Keeps track of main menu panels.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/18/2022
    /// </remarks>
    [System.Serializable]
    internal class Panel
    {
        [field: Header("References")]

        /// <summary>
        ///     The panel <see cref="GameObject"/>.
        /// </summary>
        [field: SerializeField]
        internal GameObject GameObject { get; private set; }
        
        /// <summary>
        ///     Its <see cref="RectTransform"/>.
        /// </summary>
        [field: SerializeField]
        internal RectTransform Transform { get; private set; }
        
        /// <summary>
        ///     Its <see cref="CanvasGroup"/>.
        /// </summary>
        [field: SerializeField]
        internal CanvasGroup Canvas { get; private set; }
        
        /// <summary>
        ///     An array of <see cref="MonoBehaviour"/>
        ///     (custom scripts).
        /// </summary>
        [field: SerializeField]
        internal MonoBehaviour[] MonoBehaviours { get; private set; }
    }
}