using UnityEngine;
using UnityEngine.UI;
using JonathansAdventure.Sound;
using JonathansAdventure.Transitions;

namespace JonathansAdventure.UI.Main
{
    /// <summary>
    ///     Level select button that loads a level. 
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/24/2022
    /// </remarks>
    [RequireComponent(typeof(Button))]
    public class LevelButton : MonoBehaviour
    {
        /// <summary>
        ///     The scene to load.
        /// </summary>
        [SerializeField,
            Tooltip("The scene to load.")] 
        private Scenes scene;

        /// <summary>
        ///     Crossfade to <see cref="scene"/> when the button is pressed.
        /// </summary>
        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(() =>
            {
                SoundManager.Instance.PlaySound(SoundNames.ButtonSelect);
                TransitionManager.Instance.CrossFade(scene);
            });
        }
    }

}

