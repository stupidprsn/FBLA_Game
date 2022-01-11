using UnityEngine;

public class spaceToContinue : MonoBehaviour {
    public GameObject fromPanel;
    public GameObject toPanel;

    private void Update() {
        if (Input.GetKeyDown("space")) {
            toPanel.SetActive(true);
            fromPanel.SetActive(false);
            //FindObjectOfType<soundManager>().PlaySound("UISpacebar");
            FindObjectOfType<soundManager>().PlaySound("playerWalk");
        }
    }
}
