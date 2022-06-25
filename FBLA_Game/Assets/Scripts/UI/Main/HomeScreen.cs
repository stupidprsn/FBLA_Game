using UnityEngine;
using TMPro;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;
using JonathansAdventure.Transitions;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Manages naming the homescreen
    ///     and changing its button functions depending
    ///     on the gamemode.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/24/2022
    /// </remarks>
    public class HomeScreen : MonoBehaviour
    {
        #region References

        [SerializeField] private TMP_Text title;
        [SerializeField] private Animator animator;

        private SoundManager soundManager;
        private MainMenuTransitions transitions;
        private TransitionManager transitionManager;

        #endregion

        /// <summary>
        ///     Set references.
        /// </summary>
        private void Start()
        {
            soundManager = SoundManager.Instance;
            transitions = MainMenuTransitions.Instance;
            transitionManager = TransitionManager.Instance;
        }

        /// <summary>
        ///     Sets the title text.
        /// </summary>
        private void OnEnable()
        {
            if (GameData.IsSingleplayer)
            {
                title.SetText("SINGLEPLAYER");
            }
            else
            {
                title.SetText("MULTIPLAYER");
            }
        }

        /// <summary>
        ///     Transitions to another panel.
        /// </summary>
        /// <param name="panel"> The panel to transition to. </param>
        private void Transition(MenuPanels panel)
        {
            soundManager.PlaySound(SoundNames.ButtonSelect);
            transitions.Transition(panel, true);
        }

        /// <summary>
        ///     Used by the play button.
        /// </summary>
        public void Play()
        {
            // Singleplayer has level select.
            if (GameData.IsSingleplayer)
            {
                Transition(MenuPanels.LevelSelectScreen);
            }
            // Multiplayer goes directly to the level.
            else
            {
                transitionManager.CrossFade(Scenes.Multiplayer);
            }
        }

        /// <summary>
        ///     Used by the leaderboard button.
        /// </summary>
        public void Leaderboard()
        {
            Transition(MenuPanels.LeaderboardScreen);
        }

        /// <summary>
        ///     Used by the instructions button.
        /// </summary>
        public void Instructions()
        {
            if (GameData.IsSingleplayer)
            {
                Transition(MenuPanels.SingleInstructionScreen);
            }
            else
            {
                Transition(MenuPanels.MultiInstructionScreen);
            }
        }
    }

}

