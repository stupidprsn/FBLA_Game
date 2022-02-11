/*
 * Hanlin Zhang
 * Purpose: Functionality for the artifacts in the boss level
 *          Very similar to the normal artifacts script (see normal artifacts script for in-depth explanations)
 */

using UnityEngine;
using System.Collections;

public class bossArtifact : MonoBehaviour {
    public SpriteRenderer artifactRenderer;
    public Sprite[] sprites = new Sprite[3];

    public BoxCollider2D artifactCollider;
    private CapsuleCollider2D playerCollider;

    // The artifact goes to hit the boss instead of into the artifact holder
    private GameObject boss;

    private IEnumerator onCollect() {
        FindObjectOfType<soundManager>().PlaySound("playerCollectArtifact");

        // Put the artifact under the boss
        transform.SetParent(boss.transform);

        // Set up variables for the animation
        Vector3 startPos = transform.localPosition;
        // Since the artifact is a child of the boss, the boss is the origin so we can make it go to 0, 0
        Vector3 finishPos = new Vector3 (0, 0, 1);
        float t = 0;

        artifactRenderer.sortingLayerName = "Foreground";

        while(t<1) {
            t += Time.deltaTime / 0.3f;
            transform.localPosition = Vector3.Lerp(startPos, finishPos, t);
            yield return null;
        }

        // Damage the boss snake and destroy this artifact
        boss.GetComponent<bossSnake>().onDamage();
        Destroy(gameObject);
    }

    void Start() {
        // Set the reference to the boss
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
