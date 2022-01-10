using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gamePlayManager : MonoBehaviour {
    private TMP_Text timeShown;

    private float time = 0;
    private int health = 3;
    private bool isDead = false;

    public void initiateVariables() {
        health = 3;
        isDead = false;
        time = 0;
    }

    public void onWin() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void onDeath() {
        if(health > 1) {
            GameObject.Find("Jonathan").transform.position = new Vector3(6.3386f, -3.5686f, 1);
            health--;
            GameObject.Find("Canvas").transform.Find("HeartsImage").GetComponent<RectTransform>().sizeDelta = new Vector2((health * 50), 50);
        } else {
            Transform[] transforms = GameObject.Find("Canvas").GetComponentsInChildren<Transform>(true);
            foreach (var t in transforms) {
                if(t.gameObject.name == "DeathPanel") {
                    t.gameObject.SetActive(true);
                }
            }

            isDead = true;
            Destroy(GameObject.Find("Jonathan"));
        }
    }

    private void updateTime() {
        time += Time.deltaTime;
    }

    private void Update() {
        if(isDead) {
            if(Input.GetKeyDown("space")) {
                SceneManager.LoadScene("MainMenu");
                gameObject.GetComponent<gamePlayManager>().enabled = false;
            }
        } else {
            updateTime();

            if (timeShown == null) {
                timeShown = GameObject.Find("Canvas").transform.Find("ClockText").gameObject.GetComponent<TMP_Text>();
            }
            timeShown.text = Mathf.FloorToInt(time).ToString();
        }
    }
}
