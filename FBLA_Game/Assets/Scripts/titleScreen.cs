using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class titleScreen : MonoBehaviour {
    public GameObject instructionsScreen;

    // Update is called once per frame
    private void Update() {
        if(Input.GetKeyDown("space")) {
            instructionsScreen.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
