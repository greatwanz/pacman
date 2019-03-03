using UnityEngine;

namespace pacman
{
    /// <summary>
    /// An item that can be consumed by pacman
    /// </summary>
    public abstract class Consumable : MonoBehaviour
    {
        //Global constants
        [AssertNotNull]public Constants constants;
        //Global variables
        [AssertNotNull]public Variables variables;

        /// <summary>
        /// Action that occurs when collider enter consumable (is consumed)
        /// </summary>
        /// <param name="col">Collided object</param>
        protected abstract void OnTriggerEnter(Collider col);
    }
}