using UnityEngine;
using System.Collections;

public class bossArtifact : MonoBehaviour {
    public SpriteRenderer artifactRenderer;
    public Sprite[] sprites = new Sprite[3];

    public BoxCollider2D artifactCollider;
    private CapsuleCollider2D playerCollider;
    private GameObject boss;

    private IEnumerator onCollect() {
        FindObjectOfType<soundManager>().PlaySound("playerCollectArtifact");

        transform.SetParent(boss.transform);
        Vector3 startPos = transform.localPosition;
        Vector3 finishPos = new Vector3 (0, 0, 1);
        float t = 0;

        artifactRenderer.sortingLayerName = "Foreground";

        while(t<1) {
            t += Time.deltaTime / 0.3f;
            transform.localPosition = Vector3.Lerp(startPos, finishPos, t);
            yield return null;
        }

        boss.GetComponent<bossSnake>().onDamage();
        Destroy(gameObject);
    }

    void Start() {
        boss = FindObjectOfType<bossSnake>().gameObject;
        playerCollider = GameObject.Find("Jonathan").GetComponent<CapsuleCollider2D>();

        artifactRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    void Update() {
        if (artifactCollider.IsTouching(playerCollider) && Input.GetKeyDown("w")) {
            StartCoroutine(onCollect());
        }
    }
}
