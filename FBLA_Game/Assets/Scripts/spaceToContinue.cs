using UnityEngine;

public class spaceToContinue : MonoBehaviour {
    public GameObject fromPanel;
    public GameObject toPanel;

    // Update is called once per frame
    private void Update() {
        if (Input.GetKeyDown("space")) {
            toPanel.SetActive(true);
            fromPanel.SetActive(false);
        }
    }
}
