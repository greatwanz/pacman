using UnityEngine;

namespace pacman
{
    /// <summary>
    /// ScriptableObject to define a ghost
    /// </summary>
    [CreateAssetMenu(menuName = "Ghost")]
    public class Ghost : ScriptableObject
    {
        //Sound effect to play when ghost is eaten
        [AssertNotNull]public AudioClip eatGhostSFX;
        //Initial state ghost is in
        [AssertNotNull]public GhostState initialState;
        //Chase state of the ghost
        [AssertNotNull]public GhostState chaseState;
        //Initial colour of the ghost
        public Color initialColour;
        //Ghost released after score reached
        public int releaseAfterScore;
        //Speed of ghost
        public float chaseSpeed;
    }
}