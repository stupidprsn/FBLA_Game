using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {
    public float speed;
    public float jumpHeight;

    private Rigidbody2D rb;
    private Vector2 movement;
    private bool jump = false;

    // Start is called before the first frame update
    private void Start() {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    private void Update() {
        movement = new Vector2(Input.GetAxis("Horizontal") * Time.deltaTime * speed, rb.velocity.y);
        if(Input.GetKeyDown("space")) {
            jump = true;
        }
    }

    private void FixedUpdate() {
        rb.velocity = movement;
        if(jump) {
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            jump = false;
        }
    }
}
