using UnityEngine;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Lets boxes damage any object that
    ///     can be destroyed.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/24/2022
    /// </remarks>
    public class Box : MonoBehaviour
    {
        #region References

        [Header("References")]
        private Transform trans;
        [SerializeField] private LayerMask destroyable;

        #endregion

        /// <summary>
        ///     Cache reference.
        /// </summary>
        private void Awake()
        {
            trans = transform;
        }

        /// <summary>
        ///     When the box collides with another object.
        /// </summary>
        /// <param name="collision"> Information regarding the object the player has collided with. </param>
        private void OnCollisionEnter2D(Collision2D collision)
        {
            // Cache positions.
            Vector2 pos = trans.position;
            Vector2 colliderPos = collision.transform.position;

            // Layermask is in binary while layer is in decimal, the bitwise operation converts
            // the decimal to binary and checks them.
            if (((destroyable.value & (1 << collision.gameObject.layer)) > 0) &&   // Checks that the object is destroyable.
                (pos.y > colliderPos.y + 0.4f)) {                               // Checks that the box is on top.
                collision.gameObject.GetComponent<IDestroyable>().OnDamage();
            }
        }
    }

}
