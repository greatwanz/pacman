using UnityEngine;

namespace pacman
{
    /// <summary>
    /// Define constants to be used in game
    /// </summary>
    [CreateAssetMenu(menuName = "Constants")]
    public class Constants : ScriptableObject
    {
        //Score gained for eating a pacdot
        public int pacdotScoreValue;
        //Score gained for eating a power pellet
        public int powerPelletScoreValue;
        //Score gained for eating a ghost
        public int ghostEatenValue;
        //Number of lives pacman starts with
        public int startingLives;
        //Length of universal short delays
        public int shortDelay;
        //Number of loops to play frightenedSFX
        public int initFrightenedLoopCount;
        //Rate power pellet flashes
        public float powerPelletFlashRate;
        //Position to indicate ghost has finished respawning
        public Vector3 respawnCompletePosition;
    }
}