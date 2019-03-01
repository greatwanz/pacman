using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerPelletConsumable : Consumable
{
    public AudioClip powerPelletSFX;

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Pacman")
        {
            PacmanController p = col.GetComponent<PacmanController>();
            p.Score += constants.powerPelletScoreValue;
            p.StartCoroutine(AudioManager.PlayLoopedSFX(powerPelletSFX, 10));
            Destroy(gameObject);
        }
    }

}
