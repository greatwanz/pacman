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
    [ReadOnlyAttribute]public GameObject currentHitObject;
    [ReadOnlyAttribute]public AudioClip currentSFX;
    [AssertNotNull]public Constants constants;
    [AssertNotNull]public Text scoreText;
    [AssertNotNull]public AudioSource controllerAudioSource;
    [NonSerialized]public CharacterController controller;
    [NonSerialized]public int sfxPlayCount;
    public float thrust;
    public int lives;
    public LayerMask layerMask;
    public float maxDistance;
    public SphereCollider sphereCollider;


    Vector3 lastDir;

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
    float currentHitDistance;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lives = constants.startingLives;
    }

    void FixedUpdate()
    {
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
            currentHitObject = hit.transform.gameObject;
            currentHitDistance = hit.distance;
            return true;
        }
        else
        {
            currentHitDistance = maxDistance;
            currentHitObject = null;
            return false;
        }
    }

    //    private void OnDrawGizmosSelected()
    //    {
    //        Gizmos.color = Color.red;
    //        Debug.DrawLine(transform.position, transform.position + lastDir * currentHitDistance);
    //        Gizmos.DrawWireSphere(transform.position + lastDir * currentHitDistance, sphereCollider.radius);
    //    }
}
