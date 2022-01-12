using UnityEngine;

public class gameUI : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {
        FindObjectOfType<gamePlayManager>().updateCanvas();
    }
}
