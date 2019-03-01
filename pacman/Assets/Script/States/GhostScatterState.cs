using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GhostState/Ghost Scatter State")]
public class GhostScatterState : GhostState
{
    public override void Init(Ghost g)
    {
        g.meshRenderer.material.color = g.defaultColour;
    }

    public override void Execute(Ghost g)
    {
    }
}
