using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GhostState/Ghost Frightened State")]
public class GhostFrightenedState : GhostState
{
    public GhostState ghostChaseState;

    public override void Init(Ghost g)
    {
        g.meshRenderer.material.color = constants.freightenedColour;
    }

    public override void Execute(Ghost g)
    {
        if (constants.frightenedLoopCount == 0)
        {
            g.SetState(ghostChaseState);
        }
    }
}
