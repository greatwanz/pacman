﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacdotConsumable : Consumable
{
    public AudioClip wakaSFX;

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Pacman")
        {
            PacmanController p = col.GetComponent<PacmanController>();
            p.Score += constants.pacdotScoreValue;
            p.controllerAudioSource.PlayOneShot(wakaSFX);
            Destroy(gameObject);
        }
    }
}
