using UnityEngine;
using System;
using UnityEngine.UI;

/// <summary>
/// Player controller that controls pacman
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour
{
    [AssertNotNull]public Text scoreText;
    public float thrust;
    Rigidbody rb;

    public int Score
    {
        set
        { 
            score_backing = value;
            scoreText.text = "Score: " + score_backing;
        }
        get
        {
            return score_backing;
        }
    }

    private int score_backing;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            SetForce(-transform.right * thrust);
        else if (Input.GetKey(KeyCode.RightArrow))
            SetForce(transform.right * thrust);
        else if (Input.GetKey(KeyCode.UpArrow))
            SetForce(transform.forward * thrust);
        else if (Input.GetKey(KeyCode.DownArrow))
            SetForce(-transform.forward * thrust);
    }

    void SetForce(Vector3 force)
    {
        rb.velocity = Vector3.zero;
        rb.AddForce(force, ForceMode.VelocityChange);
    }

}
