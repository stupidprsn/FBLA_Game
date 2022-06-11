﻿namespace JonathansAdventure.Data
{
    /// <summary>
    ///     Stores data that is used throughout the runtime of the program.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/7/2022
    /// </remarks>
    public static class GameData
    {
        /// <summary> 
        ///     The panel to open on when the main menu is loaded.
        /// </summary>
        public static MenuPanels MenuPanel { get; set; } = MenuPanels.HomeScreen;
    }
}