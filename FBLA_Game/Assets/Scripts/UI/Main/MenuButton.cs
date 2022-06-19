using UnityEngine;
using UnityEngine.UI;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Adds an onclick listener that transitions to
    ///     a panel.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         This class is needed because Unity's inspector does 
    ///         not allow <see cref="System.Enum"/> parameters. 
    ///     </para>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/18/2022
    ///     </para>
    /// </remarks>
    [RequireComponent(typeof(MenuButton))]
    public class MenuButton : MonoBehaviour
    {
        #region References

        [SerializeField] private Button button;
        [SerializeField] private MainMenuTransitions transitions;
        [SerializeField,
            Tooltip("Set the panel to transition to.")] 
        private MenuPanels panel;

        #endregion

        /// <summary>
        ///     Add an onclick listener that plays a sound
        ///     and transitions.
        /// </summary>
        private void Awake()
        {
            button.onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundNames.ButtonSelect);
                transitions.Transition(panel, true);
            });
        }
    }
}