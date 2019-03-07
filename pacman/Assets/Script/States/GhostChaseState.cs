using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

namespace pacman
{
    /// <summary>
    /// Ghost chase state. Ghost chases Pacman.
    /// </summary>
    [CreateAssetMenu(menuName = "GhostState/Ghost Chase State")]
    public class GhostChaseState : GhostState
    {
        public override void Init(GhostConsumable g)
        {
            g.meshRenderer.material.color = g.ghost.initialColour;
        }

        public override void Execute(GhostConsumable g)
        {
            PathRequestManager.RequestPath(g.transform.position, g.target.position, g.OnPathFound);
        }



    }
}