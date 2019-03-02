using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPelletConsumable : Consumable
{
    public GhostState frightenedState;

    MeshRenderer meshRenderer;

    void Start()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        StartCoroutine(FlashPowerPellet());
    }

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Pacman")
        {
            PacmanController p = col.GetComponent<PacmanController>();
            p.Score += constants.powerPelletScoreValue;

            constants.frightenedLoopCount = 5;
            if (p.frightenedCoroutine == null)
            {
                AudioManager.PlayMusic(frightenedState.audioResources.powerPelletSFX);
                p.frightenedCoroutine = p.StartCoroutine(p.PlaySiren(frightenedState));
            }

            foreach (Ghost g in GameManager.ghosts)
            {
                g.SetState(frightenedState);
            }
            Destroy(gameObject);
        }
    }

    IEnumerator FlashPowerPellet()
    {
        while (true)
        {
            yield return new WaitForSeconds(constants.powerPelletFlashRate);
            yield return new WaitUntil(() => GameManager.controlState == GameManager.CONTROL_STATE.ACTIVE);
            meshRenderer.enabled = false;
            yield return new WaitForSeconds(constants.powerPelletFlashRate);
            yield return new WaitUntil(() => GameManager.controlState == GameManager.CONTROL_STATE.ACTIVE);
            meshRenderer.enabled = true;
        }
    }
}
