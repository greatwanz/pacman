using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Constants")]
public class Constants : ScriptableObject
{
    public int pacdotScoreValue;
    public int powerPelletScoreValue;
    public int startingLives;
    public Color freightenedColour;
    public int frightenedLoopCount;
    public Vector3 ghostRespawnPosition;
    public Vector3 pacmanRespawnPosition;
    public float powerPelletFlashRate;
}
