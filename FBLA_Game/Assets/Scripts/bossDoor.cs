using UnityEngine;

public class bossDoor : MonoBehaviour {
    public int numOfAmulets;

    private GameObject artifactPlacer;
    public SpriteRenderer doorPieceRenderer;
    public openedDoor openedDoorScript;

    private void Start() {
        artifactPlacer = transform.parent.Find("Artifact Placer").gameObject;
    }

    public void checkArtifacts() {
        Destroy(artifactPlacer.transform.GetChild(0));
        FindObjectOfType<bossSnake>().hp = FindObjectOfType<bossSnake>().hp - 1;
    }
}