using UnityEngine;
using UnityEngine.AI;

namespace pacman
{
    /// <summary>
    /// Ghost's current state
    /// </summary>
    public abstract class GhostState : ScriptableObject
    {
        //Global constants
        public Constants constants;
        //Global variables
        public Variables variables;
        //Global audio resources
        public AudioResources audioResources;

        /// <summary>
        /// Initialises a ghost state. Executes when state is switched to.
        /// </summary>
        /// <param name="g">The ghost the state is currently applied on</param>
        public abstract void Init(GhostConsumable g);

        /// <summary>
        /// Execute the specified ghost state. Continues executing until ghost's state is switched.
        /// </summary>
        /// <param name="g">The ghost the state is currently applied on</param>
        public abstract void Execute(GhostConsumable g);

       
    }
}