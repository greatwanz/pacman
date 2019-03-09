using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace pacman
{
    [CreateAssetMenu(menuName = "Ghost")]
    public class Ghost : ScriptableObject
    {
        //Sound effect to play when ghost is eaten
        [AssertNotNull]public AudioClip eatGhostSFX;
        [AssertNotNull]public ChaseTarget chaseTarget;

        public Color initialColour;
        public GhostState initialState;
        public GhostState chaseState;
        public int releaseAfterNumPacdots;
        public float chaseSpeed;
    }
}