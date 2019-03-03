using System.Collections;
using UnityEngine;

namespace pacman
{
    /// <summary>
    /// A consumable power pellet
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class PowerPelletConsumable : Consumable
    {
        //Frightened state of ghosts
        [AssertNotNull]public GhostState frightenedState;
        //Scatter state of ghosts
        [AssertNotNull]public GhostState scatterState;
        [AssertNotNull]public AudioClip frightenedMusic;

        //MeshRenderer of the power pellet
        MeshRenderer meshRenderer;

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            StartCoroutine(FlashPowerPellet());
        }

        protected override void OnTriggerEnter(Collider col)
        {
            PacmanController p = col.GetComponent<PacmanController>();
            if (p != null)
            {
                p.Score += constants.powerPelletScoreValue;

                //Set ghosts to frightened state
                foreach (Ghost g in GameManager.ghosts)
                {
                    g.SetState(frightenedState);
                }

                //Set frightenedLoopCount to its initial value
                variables.frightenedLoopCount = constants.initFrightenedLoopCount;

                //If frightened music is not currently being played, play it, and start coroutine to
                //schedule switching back to siren music
                if (Ghost.unfrightenCoroutine == null)
                {
                    AudioManager.PlayMusic(frightenedMusic);
                    Ghost.unfrightenCoroutine = p.StartCoroutine(Unfrighten(scatterState));
                }

                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Flashes the power pellet.
        /// </summary>
        IEnumerator FlashPowerPellet()
        {
            while (true)
            {
                yield return new WaitForSeconds(constants.powerPelletFlashRate);
                //Wait until user has control before flashing power pellet
                yield return new WaitUntil(() => variables.pacmanControlState);
                meshRenderer.enabled = false;
                yield return new WaitForSeconds(constants.powerPelletFlashRate);
                yield return new WaitUntil(() => variables.pacmanControlState);
                meshRenderer.enabled = true;
            }
        }


        /// <summary>
        /// Unfrighten ghosts
        /// </summary>
        /// <param name="state">State to return to</param>
        public IEnumerator Unfrighten(GhostState state)
        {
            while (variables.frightenedLoopCount > 0)
            {
                yield return new WaitForSeconds(constants.shortDelay);
                variables.frightenedLoopCount--;
            }
            AudioManager.PlayMusic(state.audioResources.sirenMusic);
            //Set ghosts to scatter state
            foreach (Ghost g in GameManager.ghosts)
            {
                g.SetState(state);
            }
            Ghost.unfrightenCoroutine = null;
        }
    }
}