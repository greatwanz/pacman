using System.Collections;
using UnityEngine;

namespace pacman
{
    /// <summary>
    /// A consumable power pellet
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class PowerPellet : MonoBehaviour
    {
        //Frightened state of ghosts
        [AssertNotNull]public GhostState frightenedState;
        //Scatter state of ghosts
        [AssertNotNull]public GhostState scatterState;
        //Global constants
        [AssertNotNull]public Constants constants;

        //MeshRenderer of the power pellet
        MeshRenderer meshRenderer;
        //Coroutine to flash power pellets
        Coroutine flashPowerPelletCoroutine;
        //Reference to all ghosts
        GhostController[] ghosts;

        void Start()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            ghosts = FindObjectsOfType<GhostController>();

            flashPowerPelletCoroutine = StartCoroutine(FlashPowerPellet());
        }

        void OnDestroy()
        {
            if (flashPowerPelletCoroutine != null)
                StopCoroutine(flashPowerPelletCoroutine);
        }

        void OnTriggerEnter(Collider col)
        {
            PacmanController p = col.GetComponent<PacmanController>();

            if (p != null)
            {
                p.Score += constants.powerPelletScoreValue;

                //Set loop count and start return to chase state countdown
                GhostController.frightenedLoopCount = constants.initFrightenedLoopCount;
                p.StartCoroutine(CountdownReturnToChaseState());

                foreach (GhostController g in ghosts)
                {
                    //Set ghost's state to frightened as long as it is not in pen
                    if (g.currentGhostState.GetType() != typeof(GhostWaitingInPenState))
                        g.SetState(frightenedState);
                }
                Destroy(gameObject);
            }
        }

        /// <summary>
        /// Countdown of time remaining in frightened state
        /// </summary>
        IEnumerator CountdownReturnToChaseState()
        {
            while (GhostController.frightenedLoopCount > 0)
            {
                yield return new WaitForSeconds(constants.shortDelay);
                GhostController.frightenedLoopCount--;
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
                yield return new WaitUntil(() => PacmanController.pacmanControlState);
                meshRenderer.enabled = false;
                yield return new WaitForSeconds(constants.powerPelletFlashRate);
                yield return new WaitUntil(() => PacmanController.pacmanControlState);
                meshRenderer.enabled = true;
            }
        }
    }
}