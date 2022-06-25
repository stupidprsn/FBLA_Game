namespace JonathansAdventure.Game
{
    /// <summary>
    ///     All destroyable objects (players & sankes)
    ///     should have a method for how they take damage.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/21/2022
    /// </remarks>
    public interface IDestroyable
    {
        /// <summary>
        ///     How the destroyable object takes damage.
        /// </summary>
        public void OnDamage();
    }
}
