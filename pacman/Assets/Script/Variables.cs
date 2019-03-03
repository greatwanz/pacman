using UnityEngine;

namespace pacman
{
    /// <summary>
    /// Define constants to be used in game
    /// </summary>
    [CreateAssetMenu(menuName = "Variables")]
    public class Variables : ScriptableObject
    {
        //Whether pacman can be controlled
        public bool pacmanControlState;
        //Number of loops the frightened SFX has left to play
        public int frightenedLoopCount;
        //Current state of the ghosts
        public GhostState currentGhostState;
        //Move speed of pacman
        public float pacmanSpeed;
    }
}