using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Controller : MonoBehaviour
{
    public float thrust;
    Rigidbody rb;

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
