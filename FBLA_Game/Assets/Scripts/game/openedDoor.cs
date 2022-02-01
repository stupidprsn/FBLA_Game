using UnityEngine;

public class openedDoor : MonoBehaviour {
    private CapsuleCollider2D playerCollider;
    public BoxCollider2D doorCollider;

    private void Start() {
        playerCollider = GameObject.Find("Jonathan").GetComponent<CapsuleCollider2D>();
    }

    private void Update() {
        if(playerCollider.IsTouching(doorCollider) && (Input.GetKeyDown("w") || Input.GetKeyDown("up"))) {
            GameObject.Find("GameManager").GetComponent<gamePlayManager>().winStage();
        }
    }
}
