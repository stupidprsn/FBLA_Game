using UnityEngine;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     Creates laser bullet.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/21/2022
    /// </remarks>
    public class Laser : MonoBehaviour
    {
        #region References

        [Header("References")]
        [SerializeField] private LayerMask killable;

        #endregion

        /// <summary>
        ///     When the laser collides with another object.
        /// </summary>
        /// <param name="collision"> Information regarding the object the player has collided with. </param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Layermask is in binary while layer is in decimal, the bitwise operation converts
            // the decimal to binary and checks them.
            if ((killable.value & (1 << collision.gameObject.layer)) > 0) {   // Checks that the object is destroyable.

                collision.gameObject.GetComponent<IDestroyable>().OnDamage();
            } else
            {
                // Turn off laser so that it can be reused by the object pool.
                gameObject.SetActive(false);
            }
        }
    }

}
