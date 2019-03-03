using UnityEngine;


namespace pacman
{
    /// <summary>
    /// Audio manager, manages playing of audio
    /// </summary>
    public class AudioManager : MonoBehaviour
    {
        /// <summary>
        /// A music source. Music sources loop.
        /// </summary>
        public static AudioSource musicSource;
        /// <summary>
        /// A SFX source. SFX sources play once.
        /// </summary>
        public static AudioSource sfxSource;

        void Awake()
        {
            musicSource = gameObject.AddComponent<AudioSource>();
            sfxSource = gameObject.AddComponent<AudioSource>();
            musicSource.playOnAwake = false;
            sfxSource.playOnAwake = false;
            musicSource.loop = true;
        }

        /// <summary>
        /// Plays the music with name clipName
        /// </summary>
        /// <param name="clip">AudioClip to play</param>
        public static void PlayMusic(AudioClip clip)
        {
            if (clip == null)
                return;
            musicSource.clip = clip;
            musicSource.Play();
        }

        /// <summary>
        /// Plays the sfx with name clipName
        /// </summary>
        /// <param name="clip">AudioClip to play</param>
        public static void PlaySFX(AudioClip clip)
        {
            if (clip == null)
                return;
            sfxSource.PlayOneShot(clip);
        }
    }
}