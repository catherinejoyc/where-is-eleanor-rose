using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System;

public class LockedAreaScript : MonoBehaviour
{ 
    public string message;
    public string speaker;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.StartPopUp(message, speaker);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            UIManager.Instance.StopPopUp();
        }
    }

    // --- unlock
    public Collider2D bodyColl;
    public Collider2D triggerColl;

    //deactivate the colliders
    public void Unlock()
    {
        bodyColl.enabled = false;
        triggerColl.enabled = false;

        //deactivate the obj
        gameObject.SetActive(false);
    }
}
