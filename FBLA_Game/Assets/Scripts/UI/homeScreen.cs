/*
 * Hanlin Zhang
 * Purpose: Navigation system for the home screen
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class homeScreen : MonoBehaviour {
    // Arrays of the 6 main menu buttons. They are set in metadata.
    public UnityEngine.UI.Button[] topButtons = new UnityEngine.UI.Button[3];
    public UnityEngine.UI.Button[] bottomButtons = new UnityEngine.UI.Button[3];

    // The two arrays are copied over to a multidimensional array.
    private UnityEngine.UI.Button[,] buttons = new UnityEngine.UI.Button[2, 3];

    // Keep track of which button we are on. We start on the play button which has an index of 0, 1
    private int verticalIndex = 0;
    private int horizontalIndex = 1;

    // Used by some of the buttons to go to another panel
    public void changePanel(GameObject toPanel) {
        toPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    // Used by the quit button
    public void quitButton() {
        Application.Quit();
    }

    // For updating the selected button. Takes in the index (vertical or horizontal) and whether to incremenmt or decrement.
    private void buttonSelect(ref int index, bool increment) {
        // Play sound
        FindObjectOfType<soundManager>().PlaySound("UISelectButton");
        // Reset the button color of the previously selected button
        buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(1, 1, 1, 0);
        
        // Increment or decrement the index
        if(increment) {
            index++;
        } else {
            index--;
        }

        // Update visuals
        buttons[verticalIndex, horizontalIndex].Select();
        buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.4f);
    }

    // Copy over the arrays to the multidimensional array. 
    private void Start() {
        buttons = new UnityEngine.UI.Button[2, 3] {
            {topButtons[0], topButtons[1], topButtons[2] },
            {bottomButtons[0], bottomButtons[1], bottomButtons[2] }
        };

        // Update visual to "select" the play button
        buttons[verticalIndex, horizontalIndex].Select();
        buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(0.6f, 0.6f, 0.6f, 0.4f);
    }

    private void Update() {
        // Check if we get wasd or the corresponding arrow key.
        // Then check if we stay inside the bounds.
        // Call the buttonSelect method with the corresponding index and action
        if ((Input.GetKeyDown("w") || Input.GetKeyDown("up")) && verticalIndex == 1) {
            buttonSelect(ref verticalIndex, false);
        }

        if((Input.GetKeyDown("a") || Input.GetKeyDown("left")) && horizontalIndex != 0) {
            buttonSelect(ref horizontalIndex, false);

        }

        if ((Input.GetKeyDown("s") || Input.GetKeyDown("down")) && verticalIndex == 0) {
            buttonSelect(ref verticalIndex, true);

        }

        if ((Input.GetKeyDown("d") || Input.GetKeyDown("right")) && horizontalIndex != 2) {
            buttonSelect(ref horizontalIndex, true);
        }

        // If the user presses space to "click" the button
        if (Input.GetKeyDown("space")) {
            // Play sound
            FindObjectOfType<soundManager>().PlaySound("UISelectButton");
            // Reset button visuals
            buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255, 0);
            // Perform the action associated with the button
            buttons[verticalIndex, horizontalIndex].onClick.Invoke();
        }
    }
}