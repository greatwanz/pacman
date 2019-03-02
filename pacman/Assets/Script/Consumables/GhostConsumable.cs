using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Ghost))]
public class GhostConsumable : Consumable
{
    [AssertNotNull]public AudioClip eatGhostSFX;
    [AssertNotNull]public AudioClip pacmanDiesSFX;

    Ghost ghost;

    void Start()
    {
        ghost = GetComponent<Ghost>();
    }

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Pacman")
        {
            PacmanController p = col.GetComponent<PacmanController>();

            if (ghost.currentState.GetType() == typeof(GhostFrightenedState))
                p.StartCoroutine(p.ConsumeGhost(eatGhostSFX));
            else
                p.StartCoroutine(p.PacmanDies(pacmanDiesSFX));
        }
    }
}
