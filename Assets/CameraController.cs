using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;
    public float xOffset;


    private void Start()
    {
        UpdateCameraPosition();
    }

    void Update()
    {
        UpdateCameraPosition();
    }

    private void UpdateCameraPosition()
    {
        transform.position = new Vector3(playerTransform.position.x + xOffset, transform.position.y, transform.position.z);
    }

    public void SetTransformToFollow(Transform transform) {
        playerTransform = transform;
    }
}
