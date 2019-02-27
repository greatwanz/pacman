using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PacdotConsumable : Consumable
{
    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Pacman")
        {
            col.GetComponent<PlayerController>().Score += constants.pacdotScoreValue;
            Destroy(gameObject);
        }
    }
}
