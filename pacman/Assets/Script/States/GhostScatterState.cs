﻿using UnityEngine;
using UnityEngine.AI;

namespace pacman
{
    /// <summary>
    /// Ghost scatter state. Ghosts scatter to the corners.
    /// </summary>
    [CreateAssetMenu(menuName = "GhostState/Ghost Scatter State")]
    public class GhostScatterState : GhostState
    {
        public override void Init(GhostConsumable g)
        {
            g.meshRenderer.material.color = g.ghost.initialColour;
        }

        public override void Execute(GhostConsumable g)
        {
        }

    }
}