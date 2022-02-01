/*
 * Hanlin Zhang
 * Purpose: Manages gameplay
 */

using System.IO;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

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
    private TMP_Text timeShown;

    // The time taken and health of the player
    private float time;
    private int health;
    private bool doUpdateTime;
    private int finalScore;


    // Reset the variables to their defaults
    public void initiateVariables() {
        health = 3;
        time = 0;
        doUpdateTime = false;
    }

    public void setTimeShown() {
       timeShown = FindObjectOfType<Canvas>().transform.Find("ClockText").GetComponent<TMP_Text>();
       doUpdateTime = true;
    }

    public void updateCanvas() {
        GameObject.Find("Canvas").transform.Find("HeartsImage").GetComponent<RectTransform>().sizeDelta = new Vector2((health * 50), 50);
    }

    // Called when the player dies
    public void onDeath() {
        // Decrement the health
        health--;
        // Reize the size of the heart display so that it displays one less heart
        updateCanvas();

        // Checks if the player still has health
        if (health > 1) {
            // Reset player position
            GameObject.Find("Jonathan").transform.position = new Vector3(6.3386f, -3.5686f, 1);

        } else {
            // Stop all sound and play the game over sound
            soundManager.stopAllSound();
            soundManager.PlaySound("gameOver");

            // Find the death panel and activate it
            // We have to search for it by transform because it is deactivated
            Transform[] transforms = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
            foreach (var t in transforms) {
                if (t.gameObject.name == "DeathPanel") {
                    t.gameObject.SetActive(true);
                }
            }

            // Disable moving the player
            FindObjectOfType<playerMovement>().enabled = false;
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
        FindObjectOfType<playerMovement>().enabled = false;

        // Find the win panel
        // We have to search for it by transform because it is deactivated
        Transform[] transforms = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
        foreach (var t in transforms) {
            if (t.gameObject.name == "winPanel") {
                t.gameObject.SetActive(true);
            }
        }

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
        List<Rank> theRankings = JsonUtility.FromJson<Rankings>(jsonFile.text).rankings;
        bool alreadyExists = false;

        for(var i = 0; i < theRankings.Count; i++) {
            if(theRankings[i].name == name) {
                alreadyExists = true;
                theRankings[i].score = score;
                break;
            }
        }

        if(!alreadyExists) {
            theRankings.Add(new Rank(name, score));
        }

        File.WriteAllText(AssetDatabase.GetAssetPath(jsonFile), JsonUtility.ToJson(theRankings));
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
        if (Input.GetKeyDown("enter") || Input.GetKey("return")) {
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
