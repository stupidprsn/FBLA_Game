using UnityEngine;

public class playerAnimation : MonoBehaviour {
    [SerializeField] private Animator animator;
    private soundManager soundManager;
    private string currentAnimation;
    private bool isPushing = false;

    public void calcAnimation(bool isWalking, bool isJumping) {
        if(isJumping) {
            playAnimation("JonathanJumping");
        } else {
            if(isPushing) {
                if (isWalking) {
                    playAnimation("JonathanPushing");
                } else {
                    playAnimation("JonathanPushingStill");
                }
            } else {
                if(isWalking) {
                    playAnimation("JonathanWalking");
                } else {
                    playAnimation("JonathanIdle");
                }
            }
        }
    }

    public void playAnimation(string newAnimation) {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
        currentAnimation = newAnimation;

        if (newAnimation == "JonathanPushing" || newAnimation == "JonathanWalking") {
            soundManager.PlaySound("playerWalk");
        } else {
            soundManager.stopSound("playerWalk");
        }

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.collider.tag == "Box") {
            if (transform.position.y - collision.gameObject.transform.position.y < 0.9) {
                isPushing = true;
            }
        }
    }

    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.collider.tag == "Box") {
            isPushing = false;
        }
    }

    private void Start() {
        soundManager = FindObjectOfType<soundManager>();
    }
}
