/*
 * Hanlin Zhang
 * Purpose: Manages singleplayer gameplay
 */

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GamePlayManager : MonoBehaviour {
    // Used to determine which method to call every frame
    // Determined by the player state: alive, won, or dead
    private delegate void PlayerStateDelegate();
    private PlayerStateDelegate updateMethod;

    [Header("Prefab Reference")]
    [SerializeField] private GameObject fileManagerPrefab;

    // Referances to various objects in the game 
    private SoundManager soundManager;
    private GameObject player;
    private playerMovement playerMovement;
    private playerAnimation playerAnimation;
    private Rigidbody2D playerRB;
    private TMP_Text timeShown;
    private GameObject deathPanel;
    private GameObject winPanel;
    private GameObject heartsImg;
    private TMP_Text scoreText;
    private TMP_Text scoreNumbers;
    private GameObject CongratulatoryText;

    // Variables
    private float time;
    private int health;
    private bool doUpdateTime;
    private int finalScore;
    private Vector3 spawnPosition;

    // Used to decide if the congratulatory message should be shown
    private bool win = false;

    private readonly string[] congratulatoryMsgs = new string[] {
        "GOOD JOB, LEVEL CLEARED!",
        "CONGRATULATIONS, LEVEL CLEARED!",
        "NICELY DONE, LEVEL CLEARED!",
        "PHENOMENAL, LEVEL CLEARED!",
        "WELL DONE, LEVEL CLEARED!"
    };

    // Reset the variables to their defaults
    public void InitiateVariables() {
        health = 5;
        time = 0f;
        doUpdateTime = false;
        updateMethod = playerAlive;
    }

    // Method that is called by onEnterLevel every time a new stage is loaded
    // Params: music (what music should be played for that level), position (spawn position)
    public void OnStageEnter(string music, Vector3 position) {
        // Create referances to the various game objects on the stage
        player = GameObject.Find("Jonathan");
        playerMovement = player.GetComponent<playerMovement>();
        playerAnimation = player.GetComponent<playerAnimation>();
        playerRB = player.GetComponent<Rigidbody2D>();
        Canvas canvas = FindObjectOfType<Canvas>();
        deathPanel = canvas.transform.Find("DeathPanel").gameObject;
        timeShown = canvas.transform.Find("ClockText").GetComponent<TMP_Text>();
        heartsImg = canvas.transform.Find("HeartsImage").gameObject;
        winPanel = canvas.transform.Find("WinPanel").gameObject;
        scoreText = winPanel.transform.Find("Image").Find("ScoreText").GetComponent<TMP_Text>();
        scoreNumbers = winPanel.transform.Find("Image").Find("ScoreNumbers").GetComponent<TMP_Text>();
        CongratulatoryText = canvas.transform.Find("CongratulatoryText").gameObject;

        spawnPosition = position;

        // Disable the UI screens for when the player dies and wins
        deathPanel.SetActive(false);
        winPanel.SetActive(false);

        // Update the UI to reflect the health of the user
        updateHealthDisplay();
        // Start incrementing the user's time
        doUpdateTime = true;

        // Stops all sound except for:
        //  1) the music for that stage and play it if it's not already playing
        //  2) the win sound
        // This is so that the music is continous between stages with the same music
        soundManager.stopAllSound(music, "gameLevelWin");
        soundManager.PlaySound(music);

        // Show the congratulatory message if the player has won a stage
        if(win) {
            // Pick a random message
            CongratulatoryText.GetComponent<TMP_Text>().SetText(
                congratulatoryMsgs[Random.Range(0, congratulatoryMsgs.Length)]    
            );

            // Display the message
            CongratulatoryText.GetComponent<Animator>().Play("CongratulatoryText");
            win = false;
        }
    }

    // Return to main menu.
    // Parameters: panel (dictates which panel to return to)
    private void ToMainMenu(int panel) {
        GameObject fileManager = Instantiate(fileManagerPrefab, transform.parent);
        DontDestroyOnLoad(fileManager);
        FindObjectOfType<GameManager>().mainMenuPanel = panel;
        SceneManager.LoadScene(1);
        Destroy(gameObject);
    }

    // Method for updating the UI to reflect the health of the user
    private void updateHealthDisplay() {
        // The image for displaying the health repeats the heart sprite which is 50 pixels
        // We can dictate how many hearts are shown by editing the size of this image
        heartsImg.GetComponent<RectTransform>().sizeDelta = new Vector2((health * 50), 50);
    }

    // Called when the player dies
    public IEnumerator onDeath() {
        // Decrement the health
        health--;
        // Update the user's health
        updateHealthDisplay();

        playerMovement.enabled = false;
        playerAnimation.playAnimation("JonathanDead");
        soundManager.PlaySound("playerDied");
        playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
        yield return new WaitForSeconds(2);

        // Checks if the player still has health
        if (health > 0) {
            // Reset player position
            player.transform.position = spawnPosition;
            playerAnimation.playAnimation("JonathanIdle");
            soundManager.PlaySound("playerRevive");
            playerMovement.enabled = true;
            playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

        } else {
            // Disable moving the player
            playerMovement.enabled = false;
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
        win = true;
    }

    // Called when the player wins the entire game
    public void winGame() {
        // Disable the player's movement
        playerMovement.enabled = false;

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
        // Wait for the user to press space, then return to the main menu and play the sound effect
        if (Input.GetKeyDown("space")) {
            soundManager.PlaySound("UISpacebar");
            ToMainMenu(1);
        }
    }

    // Method to run if the player has won
    private void playerWon() {
        // Check if the user has pressed enter or return
        if (Input.GetKeyDown("enter") || Input.GetKey("return")) {
            // Play sound effect
            soundManager.PlaySound("UISpacebar");
            // Deactivate the text input field
            FindObjectOfType<TMP_InputField>().DeactivateInputField();
            // Record the user's score
            FindObjectOfType<FileManager>().SaveLeaderboard(FindObjectOfType<TMP_InputField>().text, finalScore);
            // Return to the leaderboard screen
            ToMainMenu(4);
        }
    }

    // Initiate variables when the script runs
    // This is for debugging for when initiate variables isn't called by game manager 
    private void Start() {
        InitiateVariables();
    }

    // Call the chosen update method every frame
    private void Update() {
        updateMethod();
    }
}
