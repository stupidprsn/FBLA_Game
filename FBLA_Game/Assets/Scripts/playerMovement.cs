using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour {
    public Rigidbody2D rb;
    public LayerMask jumpableLayer;

    public float speed;
    public float jumpHeight;

    private bool jump = false;

    private bool groundChecker() {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 0.7f, jumpableLayer);
        
        if(hit.collider != null) {
            return true;
        } else {
            return false;
        }
    }

    // Update is called once per frame
    private void Update() {
        if(Input.GetKeyDown("space") && groundChecker()) {
            jump = true;
        }
    }

    private void FixedUpdate() {
        transform.Translate(
            new Vector2(Input.GetAxis("Horizontal") * Time.fixedDeltaTime * speed, rb.velocity.y)
        );

        if(jump) {
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            jump = false;
        }
    }

}
