using UnityEngine;

public class artifact : MonoBehaviour {
    public SpriteRenderer sr;
    public Sprite[] sprites;

    public BoxCollider2D selfBox;
    public CapsuleCollider2D playerBox;

    public GameObject player;
    public GameObject playerInventory;
    public GameObject amuletPlacer;

    private void uncollected()
    {
        if (selfBox.IsTouching(playerBox) && Input.GetKeyDown("w"))
        {
            transform.SetParent(playerInventory.transform);
        }
    }

    private void collected()
    {
       if(playerBox.IsTouching(amuletPlacer.GetComponent<BoxCollider2D>()) && Input.GetKeyDown("w"))
        {
            transform.SetParent(amuletPlacer.transform);
        }
    }

    void Start() {
        sr.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    void Update() {
        if(transform.parent == playerInventory.transform) {
            collected();
        } else {
            uncollected();
        }
    }
}
