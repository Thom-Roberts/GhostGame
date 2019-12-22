using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    public float slowDownFactor;
    public bool inCollider;
    public Rigidbody possessionTarget;
    private Rigidbody rb;
    private Rigidbody rbToControl;
    public GameObject gameObject;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rbToControl = rb;
    }

    void Update() {
        HandlePossesion();
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
            rbToControl.velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 force = Vector3.left * acceleration;
            Vector3 velocity = rb.velocity + force;
            rbToControl.velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        else
        {
            // slow down the object
            rbToControl.velocity *= slowDownFactor;
        }
    }

    void HandlePossesion()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            if(inCollider && rbToControl == rb) {
                rbToControl = possessionTarget;
                gameObject.GetComponent<Renderer>().enabled = false;
                
            }
            else {
                rbToControl = rb;
                gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
    }

    public void SetPosessionTarget(Rigidbody target)
    {
        Debug.Log("Set new possesion target");
        possessionTarget = target;
        inCollider = true;
    }

    public void RemovePossessionTarget()
    {
        Debug.Log("Removing possession target");
        possessionTarget = null;
        inCollider = false;
    }
}
