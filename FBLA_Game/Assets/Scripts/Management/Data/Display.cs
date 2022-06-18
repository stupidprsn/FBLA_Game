namespace JonathansAdventure.Data
{
    /// <summary>
    ///     Display settings
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/13/2022
    /// </remarks>
    [System.Serializable]
    public class Display
    {
        /// <summary>
        ///     Horizontal pixel count of window.
        /// </summary>
        public int X { get; set; }
        /// <summary>
        ///     Veritcal pixel count of window.
        /// </summary>
        public int Y { get; set; }
        /// <summary>
        ///     If the game opens in <see cref="UnityEngine.FullScreenMode.FullScreenWindow"/>
        /// </summary>
        public bool FullScreen { get; set; }

        /// <summary>
        ///     Main Constructor
        /// </summary>
        /// <param name="inputX"></param>
        /// <param name="inputY"></param>
        /// <param name="inputFullScreen"></param>
        public Display(int x, int y, bool fullScreen)
        {
            X = x;
            Y = y;
            FullScreen = fullScreen;
        }
    }
}