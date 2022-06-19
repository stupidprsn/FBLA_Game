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
        HomeScreen = 0,
        GameSelectScreen = 1,
        SingleLevelScreen = 2,
        MultiLevelScreen = 3,
        LeaderbaordScreen = 4,
        InstructionsScreen = 5
    }
}
