using UnityEngine;
using System;
using System.Collections;

/// <summary>
/// Audio manager, manages playing of audio and
/// storing of audio files
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// The music source and sfx source
    /// </summary>
    public static AudioSource musicSource, sfxSource;

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

    /// <summary>
    /// Waits until a condition is fulfilled before playing the SFX.
    /// </summary>
    /// <param name="clip">Clip to play</param>
    /// <param name="condition">Condition to fulfill</param>
    public static IEnumerator PlaySFXDelayed(AudioClip clip, Func<bool> condition)
    {
        yield return new WaitUntil(condition.Invoke);
        sfxSource.PlayOneShot(clip);
    }

    public static IEnumerator PlayLoopedMusic(AudioClip clip, int count)
    {
        AudioClip prevClip = null;
        if (musicSource.clip != null)
            prevClip = musicSource.clip;
        
        musicSource.clip = clip;
        musicSource.Play();
        while (count > 0)
        {
            yield return new WaitForSeconds(clip.length);
            count--;
        }
        musicSource.clip = prevClip;
        musicSource.Play();
    }

    public static IEnumerator PlayLoopedSFX(AudioClip clip, int count)
    {
        for (int i = 0; i < count; ++i)
        {
            sfxSource.PlayOneShot(clip);
            yield return new WaitForSeconds(clip.length);
        }
    }
}
