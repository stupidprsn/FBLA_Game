using UnityEngine;

public class playerMovement : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheckerObj;
    [SerializeField] private LayerMask jumpableLayer;

    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;

    private soundManager soundManager;

    private bool facingRight = true;
    private bool jump = false;
    private float movement;
    private bool playWalkSound = false;

    private bool walking;
    private bool jumping;
    private bool pushing;

    private bool GroundChecker() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckerObj.position, 0.2f, jumpableLayer);
        if(colliders.Length != 0) {
            return true;
        } else {
            return false;
        }
    }

    private void flipPlayer() {
        if((movement < 0 && facingRight) || (movement > 0 && !facingRight)) {
            transform.Rotate(new Vector3(0, 180, 0));
            facingRight = !facingRight;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {

        if(collision.collider.tag == "Box") {
            if (transform.position.y - collision.gameObject.transform.position.y < 0.9) {
                pushing = true;
            }
        }

        if (collision.gameObject.layer == 8) {
            if (transform.position.y - collision.gameObject.transform.position.y > -0.15f) {
                jumping = false;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.tag == "Box") {
            pushing = false;
        }
    }
    
    private void Start() {
        soundManager = FindObjectOfType<soundManager>();
    }

    private void Update() {
        movement = Input.GetAxis("Horizontal") * speed;

        if (Input.GetKeyDown("space") && GroundChecker()) {
            jump = true;
        }

        if(transform.position.y < -5.5) {
            FindObjectOfType<gamePlayManager>().onDeath();
        }
    }

    private void FixedUpdate() {
        walking = false;

        flipPlayer();

        if (!facingRight) {
            movement *= -1;
        }

        transform.Translate(
            new Vector2(movement * Time.fixedDeltaTime, rb.velocity.y)
        );

        if (movement != 0) {
            walking = true;
            if(!playWalkSound) {
                soundManager.PlaySound("playerWalk");
                playWalkSound = true;
            }
        } else {
            if (playWalkSound) {
                soundManager.stopSound("playerWalk");
                playWalkSound = false;
            }
        }

        if (jump) {
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            jumping = true;
            soundManager.stopSound("playerWalk");
            soundManager.PlaySound("playerJump");
            jump = false;
        }
    }

}
