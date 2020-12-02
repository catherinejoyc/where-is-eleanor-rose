using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Level1Start : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerScript.Instance.Daisies--;
    }
}
