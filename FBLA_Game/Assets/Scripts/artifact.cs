using UnityEngine;

public class artifact : MonoBehaviour {
    public SpriteRenderer artifactRenderer;
    public Sprite[] sprites = new Sprite[3];

    public BoxCollider2D artifactCollider;

    private CapsuleCollider2D playerCollider;
    private GameObject playerInventory;

    private GameObject artifactPlacer;
    private BoxCollider2D artifactPlacerCollider;


    private void uncollected() {
        if(artifactCollider.IsTouching(playerCollider) && Input.GetKeyDown("w")) {
            transform.SetParent(playerInventory.transform);
        }
    }

    private void collected() {
       if(playerCollider.IsTouching(artifactPlacerCollider) && Input.GetKeyDown("w")) {
            transform.SetParent(artifactPlacer.transform);
            transform.parent.parent.Find("Door").GetComponent<closedDoor>().checkArtifacts();
        }
    }

    void Start() {
        playerCollider = GameObject.Find("Jonathan").GetComponent<CapsuleCollider2D>();
        playerInventory = GameObject.Find("Jonathan").transform.Find("Inventory").gameObject;
        artifactPlacer = transform.parent.Find("Artifact Placer").gameObject;
        artifactPlacerCollider = artifactPlacer.GetComponent<BoxCollider2D>();

        artifactRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    void Update() {
        if(transform.parent == playerInventory.transform) {
            collected();
        } else if(transform.parent != artifactPlacer.transform){
            uncollected();
        }
    }
}
