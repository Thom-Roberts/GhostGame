using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Configuring to only get Player Controllers. Other collisions will have no effect
        PlayerController temp = other.GetComponent<PlayerController>();

        if(temp != null)
        {
            temp.SetPosessionTarget(GetComponent<Rigidbody>());
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PlayerController temp = other.GetComponent<PlayerController>();
        if(temp != null)
        {
            temp.RemovePossessionTarget(GetComponent<Rigidbody>());
        }
    }
}
