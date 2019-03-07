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
        Coroutine flashPowerPelletCoroutine;
        GameManager gameManager;

        void Start()
        {
            gameManager = FindObjectOfType<GameManager>();
            meshRenderer = GetComponent<MeshRenderer>();
            flashPowerPelletCoroutine = StartCoroutine(FlashPowerPellet());
        }

        void OnDestroy()
        {
            if (flashPowerPelletCoroutine != null)
                StopCoroutine(flashPowerPelletCoroutine);
        }

        protected override void OnTriggerEnter(Collider col)
        {
            PacmanController p = col.GetComponent<PacmanController>();
            if (p != null)
            {
                p.Score += constants.powerPelletScoreValue;
                //Set frightenedLoopCount to its initial value
                variables.frightenedLoopCount = constants.initFrightenedLoopCount;
                GhostConsumable.SetState(frightenedState);
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
            
    }
}