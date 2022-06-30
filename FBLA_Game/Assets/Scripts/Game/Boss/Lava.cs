using System.Collections;
using UnityEngine;

namespace JonathansAdventure.Game.Boss
{
    /// <summary>
    ///     Kills objects when they come in contact with lava.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/26/2022
    /// </remarks>
    public class Lava : MonoBehaviour
    {
        [SerializeField] private LayerMask destroyable;
        /// <summary>
        ///     The amount of time an object that comes in contact with the lava sinks,
        ///     before it gets damaged.
        /// </summary>
        [SerializeField,
            Range(0f, 1f),
            Tooltip("The amount of time an object that comes in contact with the lava sinks, " +
            "before it gets damaged.")] 
        private float sinkTime;

        /// <summary>
        ///     When the an object collides with the lava.
        /// </summary>
        /// <param name="collision"> Information regarding the object the player has collided with. </param>
        private void OnTriggerEnter2D(Collider2D collision)
        {
            // Layermask is in binary while layer is in decimal, the bitwise operation converts
            // the decimal to binary and checks them.
            if ((destroyable.value & (1 << collision.gameObject.layer)) > 0)
            {
                StartCoroutine(Wait(collision));
            }
        }

        /// <summary>
        ///     Kills the object after waiting the <see cref="sinkTime"/>.
        /// </summary>
        /// <param name="collision"> Object to damage. </param>
        /// <returns></returns>
        private IEnumerator Wait(Collider2D collision)
        {
            yield return new WaitForSeconds(sinkTime);
            collision.gameObject.GetComponent<IDestroyable>().OnDamage();
        }
    }
}
