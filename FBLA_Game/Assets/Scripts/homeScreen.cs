using UnityEngine;
using UnityEngine.SceneManagement;

public class homeScreen : MonoBehaviour {
    public UnityEngine.UI.Button[] topButtons = new UnityEngine.UI.Button[3];
    public UnityEngine.UI.Button[] bottomButtons = new UnityEngine.UI.Button[3];

    private UnityEngine.UI.Button[,] buttons = new UnityEngine.UI.Button[2, 3];
    private int verticalIndex = 0;
    private int horizontalIndex = 1;

    public void changePanel(GameObject toPanel) {
        toPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    public void quitButton() {
        Application.Quit();
    }

    private void Start() {
        buttons = new UnityEngine.UI.Button[2, 3] {
            {topButtons[0], topButtons[1], topButtons[2] },
            {bottomButtons[0], bottomButtons[1], bottomButtons[2] }
        };
    }

    private void Update() {
        buttons[verticalIndex, horizontalIndex].Select();
        buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.4f);

        if (Input.GetKeyDown("w") && verticalIndex == 1) {
            buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
            verticalIndex--;
        }

        if(Input.GetKeyDown("a") && horizontalIndex != 0) {
            buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
            horizontalIndex--;
        }

        if(Input.GetKeyDown("s") && verticalIndex == 0) {
            buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
            verticalIndex++;
        }

        if(Input.GetKeyDown("d") && horizontalIndex != 2) {
            buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
            horizontalIndex++;
        }

        if(Input.GetKeyDown("space")) {
            buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255, 0);
            buttons[verticalIndex, horizontalIndex].onClick.Invoke();
        }
    }
}