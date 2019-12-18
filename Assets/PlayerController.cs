using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    public float slowDownFactor;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();   
    }

    void FixedUpdate()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            Vector3 force = Vector3.right * acceleration;
            Vector3 velocity = rb.velocity + force;
            rb.velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 force = Vector3.left * acceleration;
            Vector3 velocity = rb.velocity + force;
            rb.velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        else
        {
            // slow down the object
            rb.velocity *= slowDownFactor;
        }
    }
}
