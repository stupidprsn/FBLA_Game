namespace JonathansAdventure.Data
{
    /// <summary>
    ///     Associates the panels in the main menu to their child number.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/5/2022
    ///     </para>
    ///     <para>
    ///         Used to determine which panel to open in the main menu.
    ///     </para>
    /// </remarks>
    public enum MenuPanels
    {
        TitleScreen = 0,
        HomeScreen = 1,
        InstructionsScreen = 2,
        CreditsScreen = 3,
        LeaderboardScreen = 4
    }
}
