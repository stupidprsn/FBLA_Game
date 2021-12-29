using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    private Rigidbody2D rb;
    public float speed;
    public float jumpHeight;
    private bool jump = false;
    float inputX;
    

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        if(Input.GetKeyDown("space")) {
            jump = true;
        }
    }

    void FixedUpdate() {
        rb.velocity = new Vector2(inputX * speed * Time.deltaTime, rb.velocity.y);
        if(jump) {
            rb.AddForce(0, jumpHeight, ForceMode2D.Impulse);
            jump = false;
        }
    }
}
