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

        public override void Init(GhostConsumable g)
        {
            g.meshRenderer.material.color = frightenedColour;
            if (unfrightenCoroutine == null)
            {
                g.StartCoroutine(Frighten());
            }
        }

        public override void Execute(GhostConsumable g)
        {

        }

        /// <summary>
        /// Unfrighten ghosts
        /// </summary>
        public IEnumerator Frighten()
        {
            AudioManager.PlayMusic(frightenedMusic);

            while (variables.frightenedLoopCount > 0)
            {
                yield return new WaitForSeconds(constants.shortDelay);
                variables.frightenedLoopCount--;
            }
            AudioManager.PlayMusic(audioResources.sirenMusic);
            unfrightenCoroutine = null;
            GhostConsumable.SetState(ghostChaseState);
        }
    }
}