using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float acceleration;
    public float maxSpeed;
    [Range(0, 1)]
    public float slowDownFactor = 0.9f;
    public bool inCollider;
    // The distance that should be seperated when possession is released
    public float releaseOffsetX;
    public float releaseOffsetY;
    
    public List<Rigidbody> possessionTargets;
    private Rigidbody playerRb; // Solely here as a reference
    private Rigidbody rbToControl;
    public new CameraController camera;
    public Material defaultMaterial;
    public Material possessMaterial;
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
            // We don't want to clamp the falling y speed, just how fast we can go right->left.
            var xSpeed = velocity.x;
            xSpeed = Mathf.Clamp(xSpeed, -maxSpeed, maxSpeed);
            rbToControl.velocity = new Vector3(xSpeed, velocity.y, velocity.z);
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            Vector3 force = Vector3.left * acceleration;
            Vector3 velocity = rbToControl.velocity + force;
            var xSpeed = velocity.x;
            xSpeed = Mathf.Clamp(xSpeed, -maxSpeed, maxSpeed);
            rbToControl.velocity = new Vector3(xSpeed, velocity.y, velocity.z);
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

                // I decided to take control of the last element we added, guessing that if the player is moving across the screen they want to posses
                // the one they most recently came into contact with.
                // I clear the targets afterwards so that we don't have lingering possessions and can be warped across the map
                rbToControl = possessionTargets.Last();
                possessionTargets.Clear();

                Instantiate(smokeEffect, transform.position, Quaternion.identity);

                // Hide object from the scene
                GetComponent<Renderer>().enabled = false;
                GetComponent<Collider>().enabled = false;

                // Apply the new shader to our possesed object
                var newRenderer = rbToControl.GetComponent<MeshRenderer>();
                newRenderer.material = possessMaterial;

            }
            else if (rbToControl != playerRb) {
                // Reset the possesed object's color
                var oldRenderer = rbToControl.GetComponent<MeshRenderer>();
                oldRenderer.material = defaultMaterial;

                // The player position is at a fixed offset from the possesed object.
                // After the player finishes the possesion, I want them to maintain the velocity, in case they're trying to fling themselves somewhere
                var possesedPosition = rbToControl.position;
                var previousVelocity = rbToControl.velocity;
                rbToControl = playerRb;
                transform.position = new Vector3(possesedPosition.x + releaseOffsetX, Mathf.Abs(possesedPosition.y) + releaseOffsetY, possesedPosition.z);
                playerRb.velocity = previousVelocity;

                Instantiate(smokeEffect, transform.position, Quaternion.identity);
                
                // Return the object to view
                GetComponent<Renderer>().enabled = true;
                GetComponent<Collider>().enabled = true;
            }
            camera.SetTransformToFollow(rbToControl.GetComponent<Transform>());
        }
    }

    public void SetPosessionTarget(Rigidbody target)
    {
        if(!possessionTargets.Contains(target)) {
            possessionTargets.Add(target);
            inCollider = true;
        }
    }

    public void RemovePossessionTarget(Rigidbody target)
    {
        possessionTargets.Remove(target);
        if(possessionTargets.Count == 0) {
            inCollider = false;
        }
    }
}
