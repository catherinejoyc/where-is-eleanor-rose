using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLocationSetter : MonoBehaviour
{
    public float locationValue;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            AudioManager.Instance.ChangePlayerLocationValue(locationValue);
        }
    }
}
