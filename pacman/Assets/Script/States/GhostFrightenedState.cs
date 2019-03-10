using UnityEngine;
using UnityEngine.AI;
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
        public AudioClip frightenedMusic;
        static Coroutine unfrightenCoroutine;

        public override void Init(GhostController g)
        {
            g.meshRenderer.material.color = frightenedColour;
            if (unfrightenCoroutine == null)
            {
                unfrightenCoroutine = g.StartCoroutine(Frighten(g));
            }
        }

        public override void Execute(GhostController g)
        {

        }

        /// <summary>
        /// Unfrighten ghosts
        /// </summary>
        public IEnumerator Frighten(GhostController g)
        {
            AudioManager.PlayMusic(frightenedMusic);
            g.StartCoroutine(TransitionFlash(g));
            while (g.frightenedLoopCount > 0)
            {
                yield return new WaitForSeconds(constants.shortDelay);
                g.frightenedLoopCount--;
            }
            AudioManager.PlayMusic(audioResources.sirenMusic);
            unfrightenCoroutine = null;
            GhostController.SetState(ghostChaseState);
        }

        public IEnumerator TransitionFlash(GhostController g)
        {
            while (g.frightenedLoopCount != 0)
            {
                if (g.frightenedLoopCount < 3)
                {
                    g.meshRenderer.material.color = Color.white;
                    yield return new WaitForSeconds(.2f);
                    if (g.frightenedLoopCount == 0)
                        break;
                    g.meshRenderer.material.color = frightenedColour;
                    yield return new WaitForSeconds(.2f);
                }
                yield return null;
            }
        }
    }
}