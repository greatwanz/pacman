using UnityEngine;

namespace pacman
{
    /// <summary>
    /// Ghost waiting in pen state
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