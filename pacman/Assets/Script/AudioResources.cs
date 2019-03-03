using UnityEngine;

namespace pacman
{
    /// <summary>
    /// AudioResources class to manage audio resources. Define resources in ScriptableObject.
    /// </summary>
    [CreateAssetMenu(menuName = "Audio Resources")]
    public class AudioResources : ScriptableObject
    {
        //Siren music during game
        [AssertNotNull]public AudioClip sirenMusic;
        //Intro music before game
        [AssertNotNull]public AudioClip introMusic;
        //Intermission music between levels
        [AssertNotNull]public AudioClip intermissionMusic;
    }
}