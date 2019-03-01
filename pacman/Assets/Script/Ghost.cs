using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[RequireComponent(typeof(MeshRenderer))]
public class Ghost : MonoBehaviour
{
    [AssertNotNull]public AudioResources audioResources;
    [AssertNotNull]public GhostState initialState;
    public Color defaultColour;
    //Coroutine freightenedCoroutine;

    [NonSerialized]public MeshRenderer meshRenderer;

    GhostState currentState;

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

    public void SetState(GhostState state)
    {
        state.Init(this);
        currentState = state;
    }

    //    public IEnumerator FreightenedState(AudioClip powerPelletSFX)
    //    {
    //        meshRenderer.material.color = constants.freightenedColour;
    //        yield return new WaitForSeconds(constants.freightenedDuration);
    //        if (freightenedCoroutine != null)
    //        {
    //            StopCoroutine(freightenedCoroutine);
    //            AudioManager.musicSource.Stop();
    //            AudioManager.musicSource.clip = audioResources.sirenSFX;
    //        }
    //
    //        freightenedCoroutine = StartCoroutine(AudioManager.PlayLoopedMusic(powerPelletSFX, 10));
    //      yield return null;
    //  }
}
