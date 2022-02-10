using UnityEngine;

public class playerAnimation : MonoBehaviour {
    [SerializeField] private Animator animator;
    private string currentAnimation;

    public void playAnimation(string newAnimation) {
        if (currentAnimation == newAnimation) return;

        animator.Play(newAnimation);
    }
}
