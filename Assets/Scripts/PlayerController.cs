using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    public float slowDownFactor;
    public bool inCollider;
    // The distance that should be seperated when possession is released
    public float releaseOffsetX;
    public float releaseOffsetY;
    
    public Rigidbody possessionTarget;
    private Rigidbody playerRb; // Solely here as a reference
    private Rigidbody rbToControl;
    public new CameraController camera;

    public GameObject smokeEffect;

    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        rbToControl = playerRb;
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
            Vector3 velocity = rbToControl.velocity + force;
            // NOTE: The player's gravity is slowed by this as well. Undecided if I want to keep this or not
            rbToControl.velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            Debug.Log(rbToControl.velocity);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 force = Vector3.left * acceleration;
            Vector3 velocity = rbToControl.velocity + force;
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
            // Check if the currently active rigidbody is the players. If so,
            // and we are able to change it, do so.
            if(inCollider && rbToControl == playerRb) {
                // Freezing velocity courtesy of: https://answers.unity.com/questions/12878/how-do-i-zero-out-the-velocity-of-an-object.html
                // That way, we don't run into the back of the object in front of it
                rbToControl.velocity = Vector3.zero;
                rbToControl.angularVelocity = Vector3.zero;

                rbToControl = possessionTarget;
                Instantiate(smokeEffect, transform.position, Quaternion.identity);
                // Hide object from the scene
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider>().enabled = false;
            }
            else if (rbToControl != playerRb) {
                var possesedPosition = rbToControl.position;
                rbToControl = playerRb;
                transform.position = new Vector3(possesedPosition.x + releaseOffsetX, possesedPosition.y + releaseOffsetY, possesedPosition.z);
                Instantiate(smokeEffect, transform.position, Quaternion.identity);
                GetComponent<Renderer>().enabled = true;
                GetComponent<Collider>().enabled = true;
            }
        }
        camera.SetTransformToFollow(rbToControl.GetComponent<Transform>());
    }

    public void SetPosessionTarget(Rigidbody target)
    {
        possessionTarget = target;
        inCollider = true;
    }

    public void RemovePossessionTarget()
    {
        possessionTarget = null;
        inCollider = false;
    }
}
