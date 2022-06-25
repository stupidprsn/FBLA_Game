namespace JonathansAdventure.Data
{
    /// <summary>
    ///     Associates the panels in the main menu to their child number.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/24/2022
    ///     </para>
    ///     <para>
    ///         Used to determine which panel to open in the main menu.
    ///     </para>
    /// </remarks>
    public enum MenuPanels
    {
        GameSelectScreen = 0,
        HomeScreen = 1,
        LeaderboardScreen = 2,
        SingleInstructionScreen = 3,
        MultiInstructionScreen = 4,
        LevelSelectScreen = 5
    }
}
