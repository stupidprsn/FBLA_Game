using UnityEngine;

public class playerMovement : MonoBehaviour {
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheckerObj;
    [SerializeField] private LayerMask jumpableLayer;
    [SerializeField] private playerAnimation playerAnimation;

    [SerializeField] private float speed;
    [SerializeField] private float jumpHeight;
    private soundManager soundManager;

    private bool facingRight = true;
    private bool jump = false;
    private float movement;

    private bool isWalking;
    private bool isJumping;

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
        if(collision.collider.gameObject.layer == 8 && collision.collider.transform.position.y + 0.1f < transform.position.y) {
            isJumping = false;
        }
    }

    private void Start() {
        soundManager = FindObjectOfType<soundManager>();
    }

    private void Update() {
        Debug.DrawLine(groundCheckerObj.position, new Vector2(groundCheckerObj.position.x, groundCheckerObj.position.y - 0.2f), Color.green);

        movement = Input.GetAxis("Horizontal") * speed;

        if (Input.GetKeyDown("space") && GroundChecker()) {
            jump = true;
        }

        if(transform.position.y < -5.5) {
            StartCoroutine(FindObjectOfType<gamePlayManager>().onDeath());
        }
    }

    private void FixedUpdate() {
        flipPlayer();

        if (!facingRight) {
            movement *= -1;
        }

        transform.Translate(
            new Vector2(movement * Time.fixedDeltaTime, rb.velocity.y)
        );

        if (movement != 0) {
            isWalking = true;
        } else {
            isWalking = false;
        }

        if (jump) {
            rb.AddForce(new Vector2(0, jumpHeight), ForceMode2D.Impulse);
            soundManager.PlaySound("playerJump");
            isJumping = true;
            jump = false;
        }

        playerAnimation.calcAnimation(isWalking, isJumping);
    }

}
