using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateClearSight : MonoBehaviour
{
    public GameObject nextDialog;

    private void Awake()
    {
        PlayerScript.Instance.clearSightAvailable = true;

        //UI
        UIManager.Instance.ShowClearSightUI();

        //activate next dialog
        nextDialog.SetActive(true);
    }
}
