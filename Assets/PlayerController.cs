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
    private Rigidbody playerRb; // Solely here as a reference
    private Rigidbody rbToControl;
    public new GameObject gameObject;
    public CameraController camera;

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
            Vector3 velocity = playerRb.velocity + force;
            rbToControl.velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 force = Vector3.left * acceleration;
            Vector3 velocity = playerRb.velocity + force;
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
                gameObject.GetComponent<Renderer>().enabled = false;
            }
            else {
                rbToControl = playerRb;
                gameObject.GetComponent<Renderer>().enabled = true;
            }
        }
        camera.SetTransformToFollow(rbToControl.GetComponent<Transform>());
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
