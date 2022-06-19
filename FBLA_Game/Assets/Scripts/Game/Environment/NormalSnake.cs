using UnityEngine;

namespace JonathansAdventure.Game.Normal
{
    /// <summary>
    ///     Snake for normal levels.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/18/2022
    /// </remarks>
    public class NormalSnake : Snake
    {
        private protected override void KillSnake()
        {
            Destroy(gameObject);
        }

        private void Awake()
        {
            right = new Vector2(1.05f, -0.45f);
            left = new Vector2(-0.7f, -0.45f);
            down = new Vector2(-0.9f, -0.7f);
            raycast = new Vector2(0.3f, 0.75f);
        }

        
    }

}
