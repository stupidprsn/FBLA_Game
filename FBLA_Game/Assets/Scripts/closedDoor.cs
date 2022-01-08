using UnityEngine;

public class closedDoor : MonoBehaviour {
    public int numOfAmulets;

    public GameObject amuletHolder;
    public SpriteRenderer doorPieceRenderer;
    public openedDoor openedDoorScript;

    public void checkAmulets() {
        if (amuletHolder.transform.childCount == numOfAmulets) {
            doorPieceRenderer.enabled = false;
            openedDoorScript.enabled = true;
        }
    }
}