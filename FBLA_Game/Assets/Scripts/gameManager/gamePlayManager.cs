/*
 * Hanlin Zhang
 * Purpose: Manages gameplay
 */

using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gamePlayManager : MonoBehaviour {
    // Used to determine which method to call 
    // Determined by the player state: alive, won, or dead
    private delegate void playerStateDelegate();
    private playerStateDelegate updateMethod;

    // Referance to the game manager and sound manager
    [SerializeField] private gameManager gameManager;
    [SerializeField] private soundManager soundManager;

    // Referances to various in game objects
    private GameObject player;
    private TMP_Text timeShown;
    private GameObject deathPanel;
    private GameObject winPanel;
    private GameObject heartsImg;
    private TMP_Text scoreText;
    private TMP_Text scoreNumbers;

    // The time the player has taken
    private float time;

    // The health of the player
    private int health;

    // Boolean to keep track of whether to increment the time
    private bool doUpdateTime;

    // The user's final score
    private int finalScore;

    // Reset the variables to their defaults
    public void initiateVariables() {
        health = 3;
        time = 0f;
        doUpdateTime = false;
        updateMethod = playerAlive;
    }

    // Method that is called by onEnterLevel every time a new stage is loaded
    // Takes in one paramenter, music, which dictates what music should be played for that stage
    public void onStageEnter(string music) {
        // Create referances to the various game objects on the stage
        player = GameObject.Find("Jonathan");

        Canvas canvas = FindObjectOfType<Canvas>();
        deathPanel = canvas.transform.Find("DeathPanel").gameObject;
        timeShown = canvas.transform.Find("ClockText").GetComponent<TMP_Text>();
        heartsImg = canvas.transform.Find("HeartsImage").gameObject;
        winPanel = canvas.transform.Find("WinPanel").gameObject;
        scoreText = winPanel.transform.Find("Image").Find("ScoreText").GetComponent<TMP_Text>();
        scoreNumbers = winPanel.transform.Find("Image").Find("ScoreNumbers").GetComponent<TMP_Text>();

        // Disable the UI screens for when the player dies and wins
        deathPanel.SetActive(false);
        winPanel.SetActive(false);

        // Update the UI to reflect the health of the user
        updateHealthDisplay();
        // Start incrementing the user's time
        doUpdateTime = true;

        // Stops all sound except for
        // 1) the music for that stage and play it if it's not already playing
        // 2) the win sound
        // This is so that the music is continous between stages with the same music
        soundManager.stopAllSound(music, "gameLevelWin");
        soundManager.PlaySound(music);
    }

    // Method for updating the UI to reflect the health of the user
    private void updateHealthDisplay() {
        // The image for displaying the health repeats the heart sprite which is 50 pixels
        // We can dictate how many hearts are shown by editing the size of this image
        heartsImg.GetComponent<RectTransform>().sizeDelta = new Vector2((health * 50), 50);
    }

    // Called when the player dies
    public void onDeath() {
        // Decrement the health
        health--;
        // Update the user's health
        updateHealthDisplay();

        // Checks if the player still has health
        if (health > 0) {
            // Reset player position
            player.transform.position = new Vector3(6.3386f, -3.5686f, 1);

        } else {
            // Disable moving the player
            player.GetComponentInChildren<playerMovement>().enabled = false;
            // Change our update method to reflect the player's death
            updateMethod = playerDead;

            // Stop all sound and play the game over sound
            soundManager.stopAllSound();
            soundManager.PlaySound("gameOver");

            // Turn on the death screen
            deathPanel.SetActive(true);
        }
    }

    // Called when the player beats the current stage
    // Plays win sound and loads the next stage
    // Set doUpdateTime to false so the time doesn't update while the next level loads
    public void winStage() {
        soundManager.PlaySound("gameLevelWin");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        doUpdateTime = false;

    }

    // Called when the player wins the entire game
    public void winGame() {
        // Disable the player's movement
        player.GetComponentInChildren<playerMovement>().enabled = false;

        // Change our update method to reflect the player's victory
        updateMethod = playerWon;

        // Stop all sound and play the win sound
        soundManager.stopAllSound();
        soundManager.PlaySound("gameLevelWin");

        // Turn on the win panel
        winPanel.SetActive(true);

        // Calculate the subtotal score from the user's time
        int subtotalScore;
        if (time < 999) {
            subtotalScore = 999 - Mathf.FloorToInt(time);
        } else {
            subtotalScore = 0;
        }

        // Calculate the bonus points for extra health
        int healthBonus = 20 * health;

        // Calculate the final score
        finalScore = subtotalScore + healthBonus;

        // Display the user's score
        scoreText.SetText(string.Format("Time ({0} seconds)\nLives\n------------------------------------------\nTotal: ", Mathf.FloorToInt(time)));
        scoreNumbers.SetText(string.Format("{0}\n{1}\n\n{2}", subtotalScore, healthBonus, finalScore));

        // Enable the field for the user to type their name
        FindObjectOfType<TMP_InputField>().ActivateInputField();
    }

    // Method to run if the player is alive
    private void playerAlive() {
        // Check if we should update time
        if(doUpdateTime) {
            // Update the time counter with the time since the last frame
            time += Time.deltaTime;
        }

        // Show the time
        timeShown.SetText(Mathf.FloorToInt(time).ToString());

        // Allow the user to reload the game by pressing r
        if (Input.GetKeyDown("r")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    // Method to run if the player lost
    private void playerDead() {
        // Wait for the user to press space, then return to the main menu
        if (Input.GetKeyDown("space")) {
            gameManager.toMainMenu(1);
        }
    }

    // Method to run if the player has won
    private void playerWon() {
        // Check if the user has pressed enter or return
        if (Input.GetKeyDown("enter") || Input.GetKey("return")) {
            // Deactivate the text input field
            FindObjectOfType<TMP_InputField>().DeactivateInputField();
            // Record the user's score
            FindObjectOfType<fileManager>().saveLeaderboard(FindObjectOfType<TMP_InputField>().text, finalScore);
            // Return to the leaderboard screen
            gameManager.toMainMenu(4);
        }
    }

    // Call the chosen update method every frame
    private void Update() {
        updateMethod();
    }
}
