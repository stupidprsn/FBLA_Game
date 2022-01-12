using UnityEngine;
using System.Collections;

public class finalArtifact : MonoBehaviour {

    public BoxCollider2D artifactCollider;

    private CapsuleCollider2D playerCollider;

    private IEnumerator onCollect() {
        FindObjectOfType<soundManager>().PlaySound("gameLevelWin");

        Vector3 startPos = transform.localPosition;
        Vector3 finishPos = new Vector3(0, 3, 0);
        float t = 0;

        gameObject.GetComponent<SpriteRenderer>().sortingLayerName = "Foreground";

        while(t<1) {
            t += Time.deltaTime / 0.3f;
            transform.localPosition = Vector3.Lerp(startPos, finishPos, t);
            yield return null;
        }

        FindObjectOfType<gamePlayManager>().winGame();
    }

    void Start() {
        playerCollider = GameObject.Find("Jonathan").GetComponent<CapsuleCollider2D>();

        FindObjectOfType<soundManager>().stopSound("musicBossLevel");
        FindObjectOfType<soundManager>().PlaySound("musicNormalLevel");
    }

    void Update() {
        if (artifactCollider.IsTouching(playerCollider) && Input.GetKeyDown("w")) {
            StartCoroutine(onCollect());
        }
    }
}
