using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLineController : MonoBehaviour
{
    public GameObject victoryMessage;
    private void OnTriggerEnter(Collider other) {
        if(other.CompareTag("Player")) {
            victoryMessage.SetActive(true);
        }
    }
}
