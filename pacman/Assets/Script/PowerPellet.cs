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
        [AssertNotNull]public AudioClip frightenedMusic;
        [AssertNotNull]public Constants constants;
        [AssertNotNull]public Variables variables;

        //MeshRenderer of the power pellet
        MeshRenderer meshRenderer;
        Coroutine flashPowerPelletCoroutine;
        GameManager gameManager;
        GhostController[] ghosts;

        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
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
                foreach (GhostController g in ghosts)
                {
                    //Set frightenedLoopCount to its initial value
                    g.frightenedLoopCount = constants.initFrightenedLoopCount;
                }
                GhostController.SetState(frightenedState);
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
                yield return new WaitUntil(() => PacmanController.pacmanControlState);
                meshRenderer.enabled = false;
                yield return new WaitForSeconds(constants.powerPelletFlashRate);
                yield return new WaitUntil(() => PacmanController.pacmanControlState);
                meshRenderer.enabled = true;
            }
        }
            
    }
}