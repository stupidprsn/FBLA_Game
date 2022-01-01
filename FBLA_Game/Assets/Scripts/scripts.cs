using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scripts : MonoBehaviour {
    public Collider2D player;
    public Collider2D script;
    public SpriteRenderer scriptText;

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown("w") && player.IsTouching(script)) {
            scriptText.enabled = true;
        }
    }
}
