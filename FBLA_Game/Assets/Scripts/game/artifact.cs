/*
 * Hanlin Zhang
 * Purpose: Functionality for the artifacts
 */

using UnityEngine;
using System.Collections;

public class artifact : MonoBehaviour {

    // References to the artifacts renderer and possible sprite images
    // Used to randomize the artifact image
    [SerializeField] private SpriteRenderer artifactRenderer;
    [SerializeField] private Sprite[] sprites = new Sprite[3];

    [SerializeField] private BoxCollider2D artifactCollider;

    private CapsuleCollider2D playerCollider;

    private GameObject artifactPlacer;

    private bool collected = false;

    private IEnumerator onCollect() {
        FindObjectOfType<soundManager>().PlaySound("playerCollectArtifact");

        int numOfAmulets = artifactPlacer.transform.parent.Find("Door").GetComponent<closedDoor>().numOfAmulets;
        int currentAmulets = artifactPlacer.transform.childCount;

        transform.SetParent(artifactPlacer.transform);

        Vector3 startPos = transform.localPosition;
        Vector3 finishPos;
        float t = 0;

        float x;
        if (numOfAmulets % 2 == 1) {
            x = 0f;
        } else {
            x = 0.08f;
        }

        x = x - (0.16f * (Mathf.FloorToInt(numOfAmulets / 2) - currentAmulets));

        finishPos = new Vector3(x, 0, 0);

        artifactRenderer.sortingLayerName = "Foreground";

        while(t<1) {
            t += Time.deltaTime / 0.3f;
            transform.localPosition = Vector3.Lerp(startPos, finishPos, t);
            yield return null;
        }

        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "PlacedAmulets";
        artifactPlacer.transform.parent.Find("Door").GetComponent<closedDoor>().checkArtifacts();
    }

    void Start() {
        playerCollider = GameObject.Find("Jonathan").GetComponent<CapsuleCollider2D>();
        artifactPlacer = transform.parent.Find("Artifact Placer").gameObject;

        artifactRenderer.sprite = sprites[Random.Range(0, sprites.Length)];
    }

    void Update() {
        if (Input.GetKeyDown("w") && artifactCollider.IsTouching(playerCollider) && !collected) {
            StartCoroutine(onCollect());
            collected = true;
        }
    }
}
