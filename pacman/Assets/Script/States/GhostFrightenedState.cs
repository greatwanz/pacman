using UnityEngine;
using System.Collections;

namespace pacman
{
    /// <summary>
    /// Ghost runs away from Pacman, while turning randomly.
    /// </summary>
    [CreateAssetMenu(menuName = "GhostState/Ghost Frightened State")]
    public class GhostFrightenedState : GhostState
    {
        //GhostState to switch to after frightened state expires
        public GhostState ghostChaseState;
        //Colour ghosts turn into when frightened
        public Color frightenedColour;
        //Music to play during frightened state
        public AudioClip frightenedMusic;

        public override void Init(GhostController g)
        {
            g.meshRenderer.material.color = frightenedColour;
            AudioManager.PlayMusic(frightenedMusic);
            g.StartCoroutine(TransitionFlash(g));
            //Reverse direction of ghosts
            g.SetDirection(-g.currentDir);
        }

        public override void Execute(GhostController g)
        {
            if (g.currentTargetObject != null && g.transform.position == g.currentTargetObject.transform.position)
            {
                g.FindNewDirection();
            }

            g.MoveToTarget(g.ghost.chaseSpeed / 2);
        }

        /// <summary>
        /// Flash to warn user that ghost frighten state is about to end
        /// </summary>
        /// <param name="g">The ghost controller to flash</param>
        IEnumerator TransitionFlash(GhostController g)
        {
            while (GhostController.frightenedLoopCount > 0 && g.currentGhostState.GetType() == typeof(GhostFrightenedState))
            {
                if (GhostController.frightenedLoopCount < 3)
                {
                    g.meshRenderer.material.color = Color.white;
                    yield return new WaitForSeconds(.2f);
                    g.meshRenderer.material.color = frightenedColour;
                    yield return new WaitForSeconds(.2f);
                }
                yield return null;
            }
            yield return new WaitUntil(() => PacmanController.pacmanControlState);
            g.SetState(ghostChaseState);
        }
    }
}