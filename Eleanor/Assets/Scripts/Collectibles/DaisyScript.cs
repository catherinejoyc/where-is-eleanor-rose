using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DaisyScript : MonoBehaviour
{

    public Renderer img;
    public Collider2D coll;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            PlayerScript.Instance.Daisies++;
            Use();
        }
    }

    void Use()
    {
        AudioManager.Instance.PlayDaisySound();
        img.enabled = false;
        coll.enabled = false;
    }
}
