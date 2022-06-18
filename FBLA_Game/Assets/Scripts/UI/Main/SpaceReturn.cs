using UnityEngine;
using JonathansAdventure.Sound;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Allows the user to press space to return to a previous panel.
    /// </summary>
    public class SpaceReturn : MonoBehaviour
    {
        #region References

        [SerializeField,
            Tooltip("Set the panel to return to when the user presses space.")]
        private GameObject returnTo;
        
        [SerializeField] private MainMenuTransitions transitions;
        
        #endregion

        private void Update()
        {
            // Wait until the user presses space.
            if (!Input.GetKeyDown(KeyCode.Space)) return;
            SoundManager.Instance.PlaySound(SoundNames.SpaceContinue);
            transitions.Transition(gameObject, returnTo, false);
            
        }
    }

}