using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public Vector3 offset;
    public float dampingSpeed;
    public float messageDisplayTime = 3f;
    private Vector3 velocity = Vector3.zero; // Used by the smooth damp function

    void Start() {
        transform.position = playerTransform.position + offset;
        Invoke("HideExplanationMessage", messageDisplayTime);
    }

    // Much of the learning on how to do this was from: https://www.youtube.com/watch?v=MFQhpwc6cKE
    // Final implementation came from a message in the comments
    void LateUpdate()
    {
        var desiredPosition = playerTransform.position + offset;
        var smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, dampingSpeed);
        transform.position = new Vector3(smoothedPosition.x, transform.position.y, transform.position.z);
    }

    public void SetTransformToFollow(Transform transform) {
        playerTransform = transform;
    }

    private void HideExplanationMessage() {
        var gameObject = GameObject.FindGameObjectWithTag("Information");
        Debug.Log("Found game object: " + gameObject.name);
        Destroy(gameObject);
    }
}
