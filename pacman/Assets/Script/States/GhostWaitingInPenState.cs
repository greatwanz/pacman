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
        public override void Init(GhostController g)
        {
            g.meshRenderer.material.color = g.ghost.initialColour;
        }

        public override void Execute(GhostController g)
        {
        }

    }
}