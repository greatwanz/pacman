using UnityEngine;
using System;
using UnityEngine.UI;
using System.Collections.Generic;
using System.Collections;


/// <summary>
/// Player controller that controls pacman
/// </summary>
[RequireComponent(typeof(CharacterController))]
public class PacmanController : MonoBehaviour
{
    //[ReadOnlyAttribute]public GameObject currentHitObject;
    [AssertNotNull]public AudioResources audioResources;
    [AssertNotNull]public Constants constants;
    [AssertNotNull]public Text scoreText;
    [AssertNotNull]public AudioSource controllerAudioSource;
    [AssertNotNull]public SphereCollider sphereCollider;
    [NonSerialized]public CharacterController controller;

    public float thrust;
    public float maxDistance;
    public int lives;
    public int freightenedLoopCount;
    public LayerMask layerMask;
    public Coroutine frightenedCoroutine;
    public Vector3 lastDir;

    public int Score
    {
        set
        { 
            score_backing = value;
            scoreText.text = "High Score\n" + score_backing;
        }
        get
        {
            return score_backing;
        }
    }

    int score_backing;
    // float currentHitDistance;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lives = constants.startingLives;
        GameManager.controlState = GameManager.CONTROL_STATE.ACTIVE;
    }

    void FixedUpdate()
    {
        if (GameManager.controlState == GameManager.CONTROL_STATE.INACTIVE)
            return;
        
        SetDirection();
        if (lastDir != Vector3.zero && !Collision(lastDir))
            controller.SimpleMove(lastDir * thrust);
    }

    public void SetDirection()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            lastDir = -transform.right;
        else if (Input.GetKey(KeyCode.RightArrow))
            lastDir = transform.right;
        else if (Input.GetKey(KeyCode.DownArrow))
            lastDir = -transform.forward;
        else if (Input.GetKey(KeyCode.UpArrow))
            lastDir = transform.forward;
    }

    bool Collision(Vector3 dir)
    {
        RaycastHit hit;
        if (Physics.SphereCast(transform.position, sphereCollider.radius, dir, out hit, maxDistance, layerMask, QueryTriggerInteraction.UseGlobal))
        {
            //currentHitObject = hit.transform.gameObject;
            // currentHitDistance = hit.distance;
            return true;
        }
        else
        {
            // currentHitDistance = maxDistance;
            //  currentHitObject = null;
            return false;
        }
    }

    public IEnumerator PlaySiren(GhostState state)
    {
        while (constants.frightenedLoopCount > 0)
        {
            yield return new WaitForSeconds(1);
            constants.frightenedLoopCount--;
        }
        AudioManager.PlayMusic(state.audioResources.sirenSFX);
        frightenedCoroutine = null;
    }

    public IEnumerator Spawn()
    {
        transform.localPosition = constants.pacmanRespawnPosition;
        yield return new WaitForSeconds(1);
        GameManager.controlState = GameManager.CONTROL_STATE.ACTIVE;
        AudioManager.PlayMusic(audioResources.sirenSFX);
    }

    public IEnumerator PacmanDies(AudioClip audioClip)
    {
        GameManager.controlState = GameManager.CONTROL_STATE.INACTIVE;
        lives--;
        lastDir = Vector3.zero;
        AudioManager.musicSource.Stop();
        yield return new WaitForSeconds(1);
        AudioManager.PlaySFX(audioClip);
        yield return new WaitForSeconds(audioClip.length);
        yield return new WaitForSeconds(1);
        yield return Spawn();
    }

    public IEnumerator ConsumeGhost(Ghost g, AudioClip audioClip)
    {
        g.transform.localPosition = constants.ghostRespawnPosition;
        AudioManager.musicSource.Pause();
        AudioManager.PlaySFX(audioClip);
        Vector3 l = lastDir;
        lastDir = Vector3.zero;
        yield return new WaitForSeconds(audioClip.length);
        AudioManager.musicSource.UnPause();
        lastDir = l;
    }
    //    private void OnDrawGizmosSelected()
    //    {
    //        Gizmos.color = Color.red;
    //        Debug.DrawLine(transform.position, transform.position + lastDir * currentHitDistance);
    //        Gizmos.DrawWireSphere(transform.position + lastDir * currentHitDistance, sphereCollider.radius);
    //    }
}
