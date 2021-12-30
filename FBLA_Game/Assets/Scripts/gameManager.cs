using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gameManager : MonoBehaviour
{
    // Start is called before the first frame update
    

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
}
