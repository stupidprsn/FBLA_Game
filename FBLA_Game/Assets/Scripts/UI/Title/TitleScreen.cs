 using UnityEngine;
using JonathansAdventure.Data;

namespace JonathansAdventure.UI.Title
{
    /// <summary>
    ///     Manages the title screen and executes code that needs to be ran at the beggining of the program.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/17/2022
    /// </remarks>
    public class TitleScreen : MonoBehaviour {
        #region References

        [Header("Object/Prefab References")]
        [SerializeField] private Animator animator;
        [SerializeField] private FileManager fileManager;
        [SerializeField] private TransitionManager transitionManager;

        #endregion

        /// <summary>
        ///     Lock user mouse controls.
        /// </summary>
        private void Awake()
        {
            // Set the screen resolution and lock the mouse
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        /// <summary>
        ///     Load default files.
        /// </summary>
        /// <remarks>
        ///     This runs in <c> Start() </c> so <see cref="FileManager"/> 
        ///     can create the references first in <c> Awake() </c>.
        /// </remarks>
        private void Start()
        {
            fileManager.LoadDefaults();
        }

        private void Update() 
        {
            // Wait until user inputs space.
            if (!Input.GetKeyDown(KeyCode.Space)) return;
            transitionManager.CrossFade(animator);
        }
    }
}
