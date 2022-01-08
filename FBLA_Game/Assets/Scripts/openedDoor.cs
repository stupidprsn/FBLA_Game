using UnityEngine;

public class openedDoor : MonoBehaviour {
    public CapsuleCollider2D playerCollider;
    public BoxCollider2D doorCollider;

    void Update() {
        if(playerCollider.IsTouching(doorCollider) && Input.GetKeyDown("w")) {
            Debug.Log("U win!!!! <congraudatory msgs>");
        }
    }
}
