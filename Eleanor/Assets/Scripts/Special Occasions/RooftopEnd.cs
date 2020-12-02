using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RooftopEnd : MonoBehaviour
{
    public Camera mainCam;
    public Transform desiredCamPos;
    public float movementSpeed;
    public GameObject dialog;

    // Update is called once per frame
    void Update()
    {
        mainCam.transform.position = Vector3.MoveTowards(mainCam.transform.position, desiredCamPos.position, movementSpeed * Time.deltaTime);

        if (mainCam.transform.position == desiredCamPos.position)
        {
            dialog.SetActive(true);
            this.enabled = false;
        }
    }
}
