using UnityEngine;

public class closedDoor : MonoBehaviour {
    public int numOfAmulets;

    private GameObject artifactPlacer;
    public SpriteRenderer doorPieceRenderer;
    public openedDoor openedDoorScript;

    private void Start() {
        artifactPlacer = transform.parent.Find("Artifact Placer").gameObject;
    }

    public void checkArtifacts() {
        if (artifactPlacer.transform.childCount == numOfAmulets) {
            doorPieceRenderer.enabled = false;
            FindObjectOfType<soundManager>().PlaySound("gameDoorOpen");
            openedDoorScript.enabled = true;
        }
    }
}