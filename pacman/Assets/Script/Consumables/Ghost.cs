using UnityEngine;
using System;
using System.Collections;

namespace pacman
{
    /// <summary>
    /// A ghost
    /// </summary>
    [RequireComponent(typeof(MeshRenderer))]
    public class Ghost : Consumable
    {
        //Coroutine to time unfrightening of ghosts
        public static Coroutine unfrightenCoroutine;
        //Initial state of the ghosts
        [AssertNotNull]public GhostState initialState;
        //Sound effect to play when ghost is eaten
        [AssertNotNull]public AudioClip eatGhostSFX;
        //Sound effect to play when ghost kills pacman
        [AssertNotNull]public AudioClip pacmanDiesSFX;
        //Default colour of ghost
        public Color defaultColour;

        //MeshRenderer of the ghost
        [NonSerialized]public MeshRenderer meshRenderer;

        void Awake()
        {
            meshRenderer = GetComponent<MeshRenderer>();
            meshRenderer.material.color = defaultColour;
        }

        void Start()
        {
            variables.currentGhostState = initialState;
        }

        void Update()
        {
            //Execute the ghost's current state
            if (variables.currentGhostState != null)
                variables.currentGhostState.Execute(this);
        }

        protected override void OnTriggerEnter(Collider col)
        {
            PacmanController p = col.GetComponent<PacmanController>();

            if (p != null)
            {
                //If ghost is in frightened state, ghost is eaten, otherwise pacman dies
                if (variables.currentGhostState.GetType() == typeof(GhostFrightenedState))
                    p.StartCoroutine(p.ConsumeGhost(this, eatGhostSFX));
                else
                    p.StartCoroutine(p.PacmanDies(pacmanDiesSFX));
            }
        }

        /// <summary>
        /// Sets the state of the ghosts
        /// </summary>
        /// <param name="state">State to switch ghosts to</param>
        public void SetState(GhostState state)
        {
            state.Init(this);
            variables.currentGhostState = state;
        }

    }
}