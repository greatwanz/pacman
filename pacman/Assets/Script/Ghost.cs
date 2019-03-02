using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshRenderer))]
public class Ghost : Consumable
{
    [ReadOnlyAttribute]public GhostState currentState;
    [AssertNotNull]public AudioResources audioResources;
    [AssertNotNull]public GhostState initialState;
    [AssertNotNull]public AudioClip eatGhostSFX;
    [AssertNotNull]public AudioClip pacmanDiesSFX;
    [NonSerialized]public MeshRenderer meshRenderer;
    public Color defaultColour;

    void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        meshRenderer.material.color = defaultColour;
    }

    void Start()
    {
        currentState = initialState;
    }

    void Update()
    {
        if (currentState != null)
            currentState.Execute(this);
    }

    protected override void OnTriggerEnter(Collider col)
    {
        if (col.gameObject.name == "Pacman")
        {
            PacmanController p = col.GetComponent<PacmanController>();

            if (currentState.GetType() == typeof(GhostFrightenedState))
                p.StartCoroutine(p.ConsumeGhost(this, eatGhostSFX));
            else
                p.StartCoroutine(p.PacmanDies(pacmanDiesSFX));
        }
    }

    public void SetState(GhostState state)
    {
        state.Init(this);
        currentState = state;
    }
}
