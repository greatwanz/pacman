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
        public override void Init(GhostController g)
        {
            g.meshRenderer.material.color = g.ghost.initialColour;
            GhostController[] controller = FindObjectsOfType<GhostController>();

            //If siren isn't being played and all ghosts are in chase state, play siren
            if (AudioManager.musicSource.clip != audioResources.sirenMusic && controller.All(c => c.currentGhostState.GetType() == typeof(GhostChaseState)))
                AudioManager.PlayMusic(audioResources.sirenMusic);
        }

        public override void Execute(GhostController g)
        {
            if (g.currentTargetObject != null && g.transform.position == g.currentTargetObject.transform.position)
                g.FindNewDirection();

            g.MoveToTarget(g.ghost.chaseSpeed);
        }
    }
}