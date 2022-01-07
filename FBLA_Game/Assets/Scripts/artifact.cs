using UnityEngine;

public class artifact : MonoBehaviour {
    public SpriteRenderer sr;
    public Sprite[] sprites;

    public BoxCollider2D selfBox;
    public CapsuleCollider2D playerBox;

    public GameObject player;

    void Start() {
        sr.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    void Update() {
        if (selfBox.IsTouching(playerBox) && Input.GetKeyDown("w")) {
            transform.SetParent(player.transform);
        }
    }
}
