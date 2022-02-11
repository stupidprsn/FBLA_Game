/*
 * Hanlin Zhang
 * Purpose: Navigation system for the home screen
 */

using UnityEngine;
using UnityEngine.SceneManagement;

public class homeScreen : MonoBehaviour {
    // Referances to the scripts in charge of managing gameplay
    private gamePlayManager gamePlayManager;

    // Arrays of the 6 main menu buttons 
    [SerializeField] private UnityEngine.UI.Button[] topButtons = new UnityEngine.UI.Button[3];
    [SerializeField] private UnityEngine.UI.Button[] bottomButtons = new UnityEngine.UI.Button[3];

    // A multidimensional array used to keep track of the buttons
    private UnityEngine.UI.Button[,] buttons = new UnityEngine.UI.Button[2, 3];

    // Variables for keeping track of which button is currently selected
    // We start on the play button which has a vertical index of 0 and a horizontal index of 1
    private int verticalIndex = 0;
    private int horizontalIndex = 1;

    // Method for loading into a singleplayer game, it is used by the play button.
    public void toGame() {
        gamePlayManager.enabled = true;
        gamePlayManager.initiateVariables();
        SceneManager.LoadScene("LevelOne");
    }

    // Loads another scene in the main menu
    // Has one parameter which dictates which panel is loaded
    public void changePanel(GameObject toPanel) {
        toPanel.SetActive(true);
        gameObject.SetActive(false);
    }

    // Used by the quit button to leave the application
    public void quitButton() {
        Application.Quit();
    }

    // For updating the selected button. Takes in the index (vertical or horizontal) and whether to incremenmt or decrement.
    private void buttonSelect(ref int index, bool increment) {
        // Play sound
        FindObjectOfType<soundManager>().PlaySound("UISelectButton");
        
        // Increment or decrement the index
        if(increment) {
            index++;
        } else {
            index--;
        }

        // Update visuals
        buttons[verticalIndex, horizontalIndex].Select();
    }

    private void Start() {
        // Create referances to the game play manager and two player manager
        gamePlayManager = FindObjectOfType<gamePlayManager>();
        
        // Copy over the arrays to the multidimensional array. 
        buttons = new UnityEngine.UI.Button[2, 3] {
            {topButtons[0], topButtons[1], topButtons[2] },
            {bottomButtons[0], bottomButtons[1], bottomButtons[2] }
        };
        // Since we don't need the original lists anymore, we can make them null so C# knows to clear them from ram
        topButtons = null;
        bottomButtons = null;

        // Update visual to "select" the play button
        buttons[verticalIndex, horizontalIndex].Select();
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

        // Checks if the user presses space to "click" the button
        if (Input.GetKeyDown("space")) {
            // Play sound
            FindObjectOfType<soundManager>().PlaySound("UISpacebar");
            // Reset button visuals
            buttons[verticalIndex, horizontalIndex].GetComponent<UnityEngine.UI.Image>().color = new Color(255, 255, 255, 0);
            // Perform the action associated with the button
            buttons[verticalIndex, horizontalIndex].onClick.Invoke();
        }
    }
}