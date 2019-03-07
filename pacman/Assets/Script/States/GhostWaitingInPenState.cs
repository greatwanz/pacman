using UnityEngine;
using UnityEngine.AI;

namespace pacman
{
    /// <summary>
    /// Ghost scatter state. Ghosts scatter to the corners.
    /// </summary>
    [CreateAssetMenu(menuName = "GhostState/Ghost Waiting In Pen State")]
    public class GhostWaitingInPenState : GhostState
    {
        public override void Init(GhostConsumable g)
        {
            
        }

        public override void Execute(GhostConsumable g)
        {
        }

    }
}