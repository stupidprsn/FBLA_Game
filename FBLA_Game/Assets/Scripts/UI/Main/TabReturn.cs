using UnityEngine;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Allows the user to press space to return to a previous panel.
    /// </summary>
    public class TabReturn : MonoBehaviour
    {
        #region References

        [SerializeField,
            Tooltip("Set the panel to return to when the user presses space.")]
        private MenuPanels panel;
        
        #endregion

        private void Update()
        {
            // Wait until the user presses space.
            if (!Input.GetKeyDown(KeyCode.Tab)) return;
            SoundManager.Instance.PlaySound(SoundNames.SpaceContinue);
            MainMenuTransitions.Instance.Transition(panel, false);
            
        }
    }

}