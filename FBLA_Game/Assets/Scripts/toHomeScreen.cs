using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class toHomeScreen : MonoBehaviour {
    public GameObject homeScreen;
    // Update is called once per frame
    void Update() {
        if (Input.GetKeyDown("space")) {
            homeScreen.SetActive(true);
            gameObject.SetActive(false);
        }
    }
}
