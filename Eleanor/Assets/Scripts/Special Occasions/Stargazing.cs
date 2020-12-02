using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stargazing : MonoBehaviour
{
    public Camera mainCamera;
    public float movementSpeed;

    Vector3 startingPoint;
    public Transform goal;
    bool isAwake = false;
    bool returning = false;

    public float waitSeconds;
    public GameObject nextDialog;

    // Start is called before the first frame update
    void Awake()
    {
        startingPoint = mainCamera.transform.position;

        //disable VN
        UIManager.Instance.StopVNWithoutBlackscreen();

        isAwake = true;
    }

    // Update is called once per frame
    void Update()
    {
        // moves after awakening
        if (isAwake)
        {
            if (!returning)
            {
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, goal.position, movementSpeed * Time.deltaTime);

                if (mainCamera.transform.position == goal.position) //goal reached
                {
                    //wait for x seconds, then go back
                    Invoke("GoBack", waitSeconds);
                }
            }
            else
            {
                mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, startingPoint, movementSpeed * Time.deltaTime);

                //reactivate 
                if (mainCamera.transform.position == startingPoint)
                {
                    nextDialog.SetActive(true);
                }
            }
        }
    }

    //go back
    void GoBack()
    {
        returning = true;
    }
}
