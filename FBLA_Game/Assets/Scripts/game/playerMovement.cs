using UnityEngine;

public class playerMovement : MonoBehaviour {
    public Rigidbody2D rb;
    public Animator animator;
    public LayerMask jumpableLayer;

    public float speed;
    public float jumpHeight;

    private bool facingRight = true;
    private bool jump = false;
    private float movement;
    private bool playWalkSound = false;

    public Transform groundCheckerObj;


    private bool GroundChecker() {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckerObj.position, 0.2f, jumpableLayer);
        if(colliders.Length != 0) {
            return true;
        } else {
            return false;
        }
    }

    private void calcMovement() {
        movement = Input.GetAxis("Horizontal") * speed;
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
                animator.SetBool("isPushing", true);
            }
        }

        if (collision.gameObject.layer == 8) {
            if (transform.position.y - collision.gameObject.transform.position.y > -0.1f) {
                animator.SetBool("isJumping", false);
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.tag == "Box") {
            animator.SetBool("isPushing", false);
        }
    }
    
    private void Update() {
        calcMovement();
        flipPlayer();

        if (Input.GetKeyDown("space") && GroundChecker()) {
            jump = true;
        }

        if(transform.position.y < -5.5) {
            FindObjectOfType<gamePlayManager>().onDeath();
        }
    }

    private void FixedUpdate() {
        if(facingRight) {
            transform.Translate(
                new Vector2(movement * Time.fixedDeltaTime, rb.velocity.y)
            );
        } else {
            transform.Translate(
                new Vector2(-movement * Time.fixedDeltaTime, rb.velocity.y)
            );
        }

        if(movement != 0) {
            animator.SetBool("isWalking", true);

            if(!playWalkSound) {
                FindObjectOfType<soundManager>().PlaySound("playerWalk");
                playWalkSound = true;
            }

            
        } else {
            animator.SetBool("isWalking", false);
            if (playWalkSound) {
                FindObjectOfType<soundManager>().stopSound("playerWalk");
                playWalkSound = false;
            }
        }

        if (jump) {
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            animator.SetBool("isJumping", true);
            FindObjectOfType<soundManager>().stopSound("playerWalk");
            FindObjectOfType<soundManager>().PlaySound("playerJump");
            jump = false;
        }
    }

}
