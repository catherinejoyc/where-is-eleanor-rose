using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseCursor : MonoBehaviour
{
    public Animator anim;
    public float timeBtwSpawn = 0.1f;

    #region disable mouse
    float lastMoveTime = 0;
    Vector2 lastMousePos;
    #endregion

    void Start()
    {
        Cursor.visible = false;
    }

    void Update()
    {
        Cursor.visible = false;

        if (Input.mousePosition != transform.position)
        {
            transform.position = Input.mousePosition; //update position
            lastMoveTime = Time.time; //update lastMoveTime

            //show cursor
            this.GetComponent<Image>().enabled = true;
        }

        //invisible if not moving for 1.5 sec
        if (Time.time > lastMoveTime + 1.5f)
        {
            this.GetComponent<Image>().enabled = false;
        }


        if (Input.GetMouseButtonDown(0))
        {
            anim.SetBool("Active", true);
        }
        else if (Input.GetMouseButtonUp(0))
        {
            anim.SetBool("Active", false);
        }
    }
}
