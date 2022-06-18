using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using JonathansAdventure.Sound;
using JonathansAdventure.Game.Normal;

namespace JonathansAdventure.Game
{
    /// <summary>
    ///     Manages singleplayer gameplay.
    /// </summary>
    /// <remarks>
    ///     Hanlin Zhang
    ///     Last Modified: 6/14/2022
    /// </remarks>
    public class GameManager : Singleton<GameManager>
    {

        // Used to determine which method to call every frame
        // Determined by the player state: alive, won, or dead
        private delegate void PlayerStateDelegate();
        private PlayerStateDelegate updateMethod;

        #region Settings

        [Header("Set Game Settings")]

        [SerializeField,
            Range(1, 10),
            Tooltip("Set the number of lives that the user has.")] 
        private int playerHealth;

        [Range(1, 10), 
            Tooltip("Set the amount of time (in seconds) it takes for the user to respawn.")]
        [SerializeField] private int respawnTime;

        [SerializeField, 
            Tooltip("Set the congratulatory messages that can be shown when the player finishes a level")]
        private string[] congratulatoryMsgs;

        [SerializeField,
            Header("Prefab Reference")]
        private GameObject fileManagerPrefab;

        #endregion

        internal CapsuleCollider2D PlayerCollider { get; private set; }

        // Referances to various objects in the game 
        private SoundManager soundManager;
        private GameObject player;
        private PlayerMovement playerMovement;
        //private PlayerAnimation playerAnimation;
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

        // Reset the variables to their defaults
        public void InitiateVariables()
        {
            health = playerHealth;
            time = 0f;
            doUpdateTime = false;
            updateMethod = playerAlive;
        }

        // Method that is called by onEnterLevel every time a new stage is loaded
        // Params: music (what music should be played for that level), position (spawn position)
        public void OnStageEnter(string music, Vector3 position)
        {
            // Create referances to the various game objects on the stage
            player = GameObject.Find("Jonathan");
            playerMovement = player.GetComponent<PlayerMovement>();
            //playerAnimation = player.GetComponent<PlayerAnimation>();
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
            //UpdateHealthDisplay();
            // Start incrementing the user's time
            doUpdateTime = true;

            // Stops all sound except for:
            //  1) the music for that stage and play it if it's not already playing
            //  2) the win sound
            // This is so that the music is continous between stages with the same music
            //soundManager.StopAllSound(music, "gameLevelWin");
            //soundManager.PlaySound(music);

            // Show the congratulatory message if the player has won a stage
            if (win)
            {
                // Pick a random message
                CongratulatoryText.GetComponent<TMP_Text>().SetText(
                    congratulatoryMsgs[Random.Range(0, congratulatoryMsgs.Length)]
                );

                // Display the message
                CongratulatoryText.GetComponent<Animator>().Play("CongratulatoryText");
                win = false;
            }
        }

        // Called when the player dies
        public IEnumerator onDeath()
        {
            health--;
            //UpdateHealthDisplay();

            playerMovement.enabled = false;
            //playerAnimation.playAnimation("JonathanDead");
            //soundManager.PlaySound("playerDied");
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;
            yield return new WaitForSeconds(respawnTime);

            // Checks if the player still has health
            if (health > 0)
            {
                player.transform.position = spawnPosition;
                //playerAnimation.playAnimation("JonathanIdle");
                //soundManager.PlaySound("playerRevive");
                playerMovement.enabled = true;
                playerRB.constraints = RigidbodyConstraints2D.FreezeRotation;

            } else
            {
                playerMovement.enabled = false;
                updateMethod = playerDead;

                soundManager.StopAllSound();
                //soundManager.PlaySound("gameOver");

                deathPanel.SetActive(true);
            }
        }

        // Called when the player beats the current stage
        // Set doUpdateTime to false so the time doesn't update while the next level loads
        public void winStage()
        {
            doUpdateTime = false;
            //soundManager.PlaySound("gameLevelWin");
            win = true;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        // Called when the player wins the entire game
        public void winGame()
        {
            playerMovement.enabled = false;
            playerRB.constraints = RigidbodyConstraints2D.FreezeAll;

            updateMethod = playerWon;

            soundManager.StopAllSound();
            //soundManager.PlaySound("gameLevelWin");

            winPanel.SetActive(true);

            // Calculate the subtotal score from the user's time
            int subtotalScore;
            if (time < 999)
            {
                subtotalScore = 999 - Mathf.FloorToInt(time);
            } else
            {
                subtotalScore = 0;
            }

            // Calculate the bonus points for extra health
            int healthBonus = 20 * health;

            // Calculate the final score
            finalScore = subtotalScore + healthBonus;

            // Display the user's score
            scoreText.SetText(string.Format($"Time ({Mathf.FloorToInt(time)} seconds)\nLives ({health})\n--------------------------------------\nTotal: "));
            scoreNumbers.SetText(string.Format($"{subtotalScore}\n{healthBonus}\n\n{finalScore}"));

            // Enable the field for the user to type their name
            FindObjectOfType<TMP_InputField>().ActivateInputField();
        }

        /// <summary>
        /// Returns to the main menu.
        /// </summary>
        private void ToMainMenu()
        {
            Instantiate(fileManagerPrefab);
            SceneManager.LoadScene(1);
            Destroy(gameObject);
        }



        // Method to run if the player is alive
        private void playerAlive()
        {
            if (doUpdateTime)
            {
                time += Time.deltaTime;
            }

            timeShown.SetText(Mathf.FloorToInt(time).ToString());

            // Allow the user to reload the game by pressing r
            if (Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }

            if (Input.GetKeyDown(KeyCode.Tab))
            {
                ToMainMenu();
            }
        }

        // Method to run if the player lost 
        private void playerDead()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                //soundManager.PlaySound("UISpacebar");
                //ToMainMenu(MenuPanels.HomeScreen);
            }
        }

        // Method to run if the player has won
        private void playerWon()
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                //soundManager.PlaySound("UISpacebar");
                FindObjectOfType<TMP_InputField>().DeactivateInputField();
                //FindObjectOfType<FileManager>().LeaderboardData.SaveEntry(FindObjectOfType<TMP_InputField>().text, finalScore);
                //ToMainMenu(MenuPanels.LeaderboardScreen);
            }
        }

        // Initiate variables when the script runs
        // This is for debugging for when initiate variables isn't called by game manager 
        private void Awake()
        {
            SingletonCheck(this);
            InitiateVariables();
        }

        // Call the chosen update method every frame
        private void Update()
        {
            updateMethod();
        }
    }

}

