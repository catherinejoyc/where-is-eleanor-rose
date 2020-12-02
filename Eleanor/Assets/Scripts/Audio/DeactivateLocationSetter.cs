using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateLocationSetter : MonoBehaviour
{
    public GameObject audioTriggers;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.CompareTag("Player"))
        {
            audioTriggers.SetActive(false);
        }
    }
}

