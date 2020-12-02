using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogTrigger : MonoBehaviour
{
    public DialogScript dialog;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //activate dialog
        if (collision.CompareTag("Player"))
        {
            //activate particular dialog gameobject
            dialog.gameObject.SetActive(true);

            //start dialog
            dialog.InitialDialogActivation();
        }
    }
}
