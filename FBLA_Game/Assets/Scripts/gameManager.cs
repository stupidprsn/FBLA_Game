using UnityEngine;

public class gameManager : MonoBehaviour {
    void Update()
    {
        if(Input.GetKeyDown("escape"))
        {
            Application.Quit();
        }
    }
}
