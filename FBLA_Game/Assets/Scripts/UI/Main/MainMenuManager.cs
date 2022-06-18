using UnityEngine;
using JonathansAdventure.Sound;
using JonathansAdventure.Data;

/// <summary>
///     Manages which panel to load first and 
///     transitions in the main menu.
/// </summary>
/// <remarks>
///     Hanlin Zhang
///     Last Modified: 6/17/2022
/// </remarks>
public class MainMenuManager : MonoBehaviour
{
    #region References

    [Header("Object Reference")]

    [SerializeField] private GameObject homeScreen;
    [SerializeField] private GameObject[] panels;

    #endregion

    private void Start()
    {
        // Check which panel the main menu should open on.
        panels[(int)GameData.MenuPanel].SetActive(true);

        // Find the sound manager
        SoundManager soundManager = SoundManager.Instance;
        soundManager.PlaySound(SoundNames.MusicMainMenu);
    }
}
