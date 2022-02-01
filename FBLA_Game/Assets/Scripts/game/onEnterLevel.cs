using UnityEngine;

public class onEnterLevel : MonoBehaviour {
    private void Start() {
        FindObjectOfType<gamePlayManager>().setTimeShown();
        Destroy(gameObject);
    }
}
