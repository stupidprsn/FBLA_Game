using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gamePlayManager : MonoBehaviour {
    public gameManager gameManager;
    private TMP_Text timeShown;

    private float time = 0;
    private int health = 3;
    private bool win = false;
    private bool isDead = false;
    private int finalScore;

    public void initiateVariables() {
        health = 3;
        isDead = false;
        time = 0;
    }

    public void onWin() {
        FindObjectOfType<soundManager>().PlaySound("gameLevelWin");
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void onDeath() {
        if(health > 1) {
            GameObject.Find("Jonathan").transform.position = new Vector3(6.3386f, -3.5686f, 1);
            health--;
            GameObject.Find("Canvas").transform.Find("HeartsImage").GetComponent<RectTransform>().sizeDelta = new Vector2((health * 50), 50);
        } else {
            FindObjectOfType<soundManager>().stopSound("musicNormalLevel");
            FindObjectOfType<soundManager>().stopSound("musicBossLevel");
            FindObjectOfType<soundManager>().PlaySound("gameOver");


            Transform[] transforms = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
            foreach (var t in transforms) {
                if(t.gameObject.name == "DeathPanel") {
                    t.gameObject.SetActive(true);
                }
            }

            isDead = true;
            FindObjectOfType<playerMovement>().enabled = false;
            health = 3;
        }
    }

    public void winGame() {
        win = true;
        FindObjectOfType<playerMovement>().enabled = false;

        Transform[] transforms = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
        foreach (var t in transforms) {
            if (t.gameObject.name == "winPanel") {
                t.gameObject.SetActive(true);
            }
        }

        int score;
        if(time < 999) {
            score = 999 - Mathf.FloorToInt(time);
        } else {
            score = 0;
        }

        int healthBonus = 20 + health;
        finalScore = score + healthBonus;

        TMP_Text[] text = FindObjectsOfType<TMP_Text>();
        TMP_Text scoreDescription = FindObjectOfType<TMP_Text>();
        foreach (var item in text) {
            if(item.name == "ScoreDescription") {
                scoreDescription = item;
            }
        }

        scoreDescription.text = string.Format("Time ({0} seconds)..................{1}\nLives..................................................{2}\n------------------------------------------\nTotal: {3}", Mathf.FloorToInt(time), score, healthBonus, finalScore);

        FindObjectOfType<TMP_InputField>().ActivateInputField();
    }

    private void updateTime() {
        time += Time.deltaTime;
    }

    public void updateCanvas() {
            GameObject.Find("Canvas").transform.Find("HeartsImage").GetComponent<RectTransform>().sizeDelta = new Vector2((health * 50), 50);
    }

    private void Update() {
        if(win) {
            if(Input.GetKeyDown("enter") || Input.GetKey("return")) {
                FindObjectOfType<TMP_InputField>().DeactivateInputField();
                gameManager.newEntry(FindObjectOfType<TMP_InputField>().text, finalScore);
                gameManager.toMainMenu();
                gameObject.GetComponent<gamePlayManager>().enabled = false;
            }
        } else {
            if (isDead) {
                if (Input.GetKeyDown("space")) {
                    gameManager.toMainMenu();
                    gameObject.GetComponent<gamePlayManager>().enabled = false;
                }
            } else {
                updateTime();

                if (timeShown == null) {
                    timeShown = GameObject.Find("Canvas").transform.Find("ClockText").gameObject.GetComponent<TMP_Text>();
                }
                timeShown.text = Mathf.FloorToInt(time).ToString();

                if (Input.GetKeyDown("r")) {
                    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
                    GameObject.Find("Canvas").transform.Find("HeartsImage").GetComponent<RectTransform>().sizeDelta = new Vector2((health * 50), 50);
                }
            }
        }
    }
}
