using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeCollectible : MonoBehaviour
{
    Renderer img;
    Collider2D coll;

    private void Start()
    {
        img = this.GetComponent<Renderer>();
        coll = this.GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript.Instance.SightPoints = PlayerScript.Instance.maxSightPoints;
            Use();
        }
    }

    void Use()
    {
        AudioManager.Instance.PlayNazarSound();
        img.enabled = false;
        coll.enabled = false;
    }
}
