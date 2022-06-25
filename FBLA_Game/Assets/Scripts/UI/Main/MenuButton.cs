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
    [RequireComponent(typeof(Button))]
    public class MenuButton : MonoBehaviour
    {
        [SerializeField,
            Tooltip("Set the panel to transition to.")] 
        private MenuPanels panel;

        /// <summary>
        ///     Add an onclick listener that plays a sound
        ///     and transitions.
        /// </summary>
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundNames.ButtonSelect);
                MainMenuTransitions.Instance.Transition(panel, true);
            });
        }
    }
}