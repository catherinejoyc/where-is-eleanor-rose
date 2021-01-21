using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerbudLevel3 : MonoBehaviour
{
    public GameObject nextDialog;
    public TutorialTrigger flowerbudsTut;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !collision.isTrigger)
        {
            if (PlayerScript.Instance.pickedUpObj != null) //if teddy bear is picked up
            {
                //activate nextDialog
                if (nextDialog != null)
                    nextDialog.SetActive(true);

                //delete
                flowerbudsTut.currentlyActive = false;
                flowerbudsTut.isFinished = true;
            }
        }
    }
}
