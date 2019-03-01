using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GhostState/Ghost Chase State")]
public class GhostChaseState : GhostState
{
    public override void Init(Ghost g)
    {
        g.meshRenderer.material.color = g.defaultColour;
    }

    public override void Execute(Ghost g)
    {
    }
}
