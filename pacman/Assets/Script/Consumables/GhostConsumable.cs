using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostConsumable : Consumable
{
    public AudioClip eatGhostSFX;

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Pacman")
        {
            AudioManager.PlaySFX(eatGhostSFX);
            Destroy(gameObject);
        }
    }
}
