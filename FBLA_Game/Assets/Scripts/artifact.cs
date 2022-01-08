using UnityEngine;

public class artifact : MonoBehaviour {
    public SpriteRenderer artifactRenderer;
    public Sprite[] sprites = new Sprite[3];

    public BoxCollider2D artifactCollider;

    private CapsuleCollider2D playerCollider;
    private GameObject playerInventory;

    private GameObject amuletPlacer;
    private BoxCollider2D amuletPlacerCollider;


    private void uncollected() {
        if(artifactCollider.IsTouching(playerCollider) && Input.GetKeyDown("w")) {
            transform.SetParent(playerInventory.transform);
        }
    }

    private void collected() {
       if(playerCollider.IsTouching(amuletPlacerCollider) && Input.GetKeyDown("w")) {
            transform.SetParent(amuletPlacer.transform);
            transform.parent.parent.Find("Door").GetComponent<closedDoor>().checkAmulets();
        }
    }

    void Start() {
        playerCollider = GameObject.Find("Jonathan").GetComponent<CapsuleCollider2D>();
        playerInventory = GameObject.Find("Jonathan").transform.Find("Inventory").gameObject;
        amuletPlacer = transform.parent.Find("Amulet Placer").gameObject;
        amuletPlacerCollider = amuletPlacer.GetComponent<BoxCollider2D>();

        artifactRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    void Update() {
        if(transform.parent == playerInventory.transform) {
            collected();
        } else {
            uncollected();
        }
    }
}
