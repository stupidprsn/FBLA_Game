using UnityEngine;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     Creates laser bullet.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/27/2022
    /// </remarks>
    public class Laser : MonoBehaviour
    {
        #region References

        [Header("References")]
        [SerializeField] private LayerMask destroyable;
        [SerializeField] private LayerMask boundry;
        #endregion

        /// <summary>
        ///     When the laser collides with another object.
        /// </summary>
        /// <param name="collision"> Information regarding the object the player has collided with. </param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Layermask is in binary while layer is in decimal, the bitwise operation converts
            // the decimal to binary and checks them.

            // Lasers can pass through boundries.
            if ((boundry.value & (1 << collision.gameObject.layer)) > 0) return;

            // Lasers damange anything destroyable.
            if ((destroyable.value & (1 << collision.gameObject.layer)) > 0) 
                collision.gameObject.GetComponent<IDestroyable>().OnDamage();

            // Turn off laser so that it can be reused by the object pool.
            gameObject.SetActive(false);
        }
    }

}
