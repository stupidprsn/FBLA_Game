/*
 * Hanlin Zhang
 * Purpose: Manages gameplay
 */

using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

using UnityEditor;

public class gamePlayManager : MonoBehaviour {
    // Used to determine which method to call 
    // Determined by the player state: alive, won, or dead
    private delegate void playerStateDelegate();
    private playerStateDelegate updateMethod;

    // Referance to the game manager and sound manager. Set in meta data
    [SerializeField] private gameManager gameManager;
    [SerializeField] private soundManager soundManager;

    [SerializeField] private TextAsset jsonFile;

    // Referance to the time display
    private GameObject player;
    private TMP_Text timeShown;
    private GameObject deathPanel;
    private GameObject winPanel;
    private GameObject heartsImg;

    // The time taken and health of the player
    private float time;
    private int health;
    private bool doUpdateTime;
    private int finalScore;

    public static TMP_Text DEBUG;

    // Reset the variables to their defaults
    public void initiateVariables() {
        health = 3;
        time = 0;
        doUpdateTime = false;
        updateMethod = playerAlive;
    }

    public void onStageEnter() {
        player = GameObject.Find("Jonathan");

        Canvas canvas = FindObjectOfType<Canvas>();
        deathPanel = canvas.transform.Find("DeathPanel").gameObject;
        timeShown = canvas.transform.Find("ClockText").GetComponent<TMP_Text>();
        heartsImg = canvas.transform.Find("HeartsImage").gameObject;
        winPanel = canvas.transform.Find("WinPanel").gameObject;

        deathPanel.SetActive(false);
        winPanel.SetActive(false);

        updateCanvas();
        doUpdateTime = true;

        soundManager.stopAllSound();
        soundManager.PlaySound("musicNormalLevel");
    }

    public void updateCanvas() {
        heartsImg.GetComponent<RectTransform>().sizeDelta = new Vector2((health * 50), 50);
    }

    // Called when the player dies
    public void onDeath() {
        // Decrement the health
        health--;
        // Reize the size of the heart display so that it displays one less heart
        updateCanvas();

        // Checks if the player still has health
        if (health > 0) {
            // Reset player position
            player.transform.position = new Vector3(6.3386f, -3.5686f, 1);

        } else {
            // Stop all sound and play the game over sound
            soundManager.stopAllSound();
            soundManager.PlaySound("gameOver");

            deathPanel.SetActive(true);

            // Disable moving the player
            player.GetComponentInChildren<playerMovement>().enabled = false;
            // Change our update method to reflect the player's death
            updateMethod = playerDead;
        }
    }

    // Called when the player beats the current stage
    // Plays win sound and loads the next stage
    public void winStage() {
        soundManager.PlaySound("gameLevelWin");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    // Called when the player wins the entire game
    public void winGame() {

        // Stop all sound and play the win sound
        soundManager.stopAllSound();
        soundManager.PlaySound("gameLevelWin");

        // Change our update method to reflect the player's victory
        updateMethod = playerWon;

        // Disable the player's movement
        player.GetComponentInChildren<playerMovement>().enabled = false;

        // Find the win panel
        // We have to search for it by transform because it is deactivated
        winPanel.SetActive(true);

        DEBUG = FindObjectOfType<TMP_Text>();
        Transform[] t = FindObjectOfType<Canvas>().GetComponentsInChildren<Transform>();
        foreach (Transform item in t) {
            if(item.gameObject.name == "DEBUG") {
                DEBUG = item.GetComponent<TMP_Text>();
            }
        }
        DEBUG.SetText("DEBUG");

        // Calculate the final score
        int subtotalScore;
        if (time < 999) {
            subtotalScore = 999 - Mathf.FloorToInt(time);
        } else {
            subtotalScore = 0;
        }

        int healthBonus = 20 * health;
        finalScore = subtotalScore + healthBonus;

        TMP_Text[] text = FindObjectsOfType<TMP_Text>();
        TMP_Text scoreDescription = FindObjectOfType<TMP_Text>();
        foreach (var item in text) {
            if (item.name == "ScoreDescription") {
                scoreDescription = item;
            }
        }

        scoreDescription.text = string.Format("Time ({0} seconds)..................{1}\nLives..................................................{2}\n------------------------------------------\nTotal: {3}", Mathf.FloorToInt(time), subtotalScore, healthBonus, finalScore);

        FindObjectOfType<TMP_InputField>().ActivateInputField();
    }

    private void updateTime() {
        time += Time.deltaTime;
    }

    private void newEntry(string name, int score) {
        Rankings theRankings = JsonUtility.FromJson<Rankings>(jsonFile.text);
        string path = Application.persistentDataPath + Path.AltDirectorySeparatorChar + "Scripts" + Path.AltDirectorySeparatorChar + "gameManager" + Path.AltDirectorySeparatorChar + "leaderboard.json";

        Debug.Log("The length is: " + theRankings.rankings.Count);
        foreach (Rank item in theRankings.rankings) {
            Debug.Log(item.name);
        }

        bool alreadyExists = false;

        for(var i = 0; i < theRankings.rankings.Count; i++) {
            if(theRankings.rankings[i].name == name) {
                Debug.Log("Already Exists!");
                alreadyExists = true;
                theRankings.rankings[i].score = score;
                break;
            }
        }

        if(!alreadyExists) {
            theRankings.rankings.Add(new Rank(name, score));
        }

        Debug.Log("The list has " + theRankings.rankings.Count + " items now");
        foreach (Rank item in theRankings.rankings) {
            Debug.Log(item.name);
        }

        Debug.Log("The string being written is:\n" + JsonUtility.ToJson(theRankings));



        //using StreamWriter writer = new StreamWriter(path);
        using StreamWriter writer = new StreamWriter(AssetDatabase.GetAssetPath(jsonFile));
        writer.Write(JsonUtility.ToJson(theRankings));
    }

    private void playerAlive() {
        if(doUpdateTime) {
            updateTime();
        }

        timeShown.text = Mathf.FloorToInt(time).ToString();

        if (Input.GetKeyDown("r")) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private void playerDead() {
        if (Input.GetKeyDown("space")) {
            gameManager.toMainMenu(1);
        }
    }

    private void playerWon() {
        DEBUG.SetText("method is called");
        if (Input.GetKeyDown("enter") || Input.GetKey("return")) {
            DEBUG.SetText("e");
            FindObjectOfType<TMP_InputField>().DeactivateInputField();
            newEntry(FindObjectOfType<TMP_InputField>().text, finalScore);
            gameManager.toMainMenu(4);
        }
    }

    private void Start() {
        updateMethod = playerAlive;
    }

    private void Update() {
        updateMethod();
    }
}
