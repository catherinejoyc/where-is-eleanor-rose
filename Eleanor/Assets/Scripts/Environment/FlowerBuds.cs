using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBuds : MonoBehaviour
{
    public ParticleSystem ps;
    public Sprite buds;
    public Sprite flowers;
    public Collider2D bodyColl;

    void Bloom()
    {
        ps.Play();

        //add Clear Points
        PlayerScript.Instance.SightPoints = PlayerScript.Instance.maxSightPoints;

        //change appearance
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sprite = flowers;
        }

        //deactivate bodyColl
        bodyColl.enabled = false;

        //remove from UI
        UIManager.Instance.DeactivateObjImage();

        //remove from player
        PlayerScript.Instance.pickedUpObj = null;

        //deactivate self
        GetComponent<Collider2D>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            //player has picked up a minion
            if (collision.GetComponent<PlayerScript>().pickedUpObj != null)
            {
                //set off dying in that minion
                collision.GetComponent<PlayerScript>().pickedUpObj.transform.position = this.transform.position;
                bool blooming = collision.GetComponent<PlayerScript>().pickedUpObj.Die();

                if (blooming)
                    Bloom();              
            }
        }
    }
}
