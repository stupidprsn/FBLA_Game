using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gamePlayManager : MonoBehaviour {
    private TMP_Text timeShown;

    private bool doUpdateTime = true;
    private float time;

    private int health = 3;

    public void onWin() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void onDeath() {
        if(health > 1) {
            GameObject.Find("Jonathan").transform.position = new Vector3(6.3386f, -3.5686f, 1);
            health--;
        } else {
            Debug.Log("u lose");
        }
    }

    private void updateTime() {
        time += Time.deltaTime;
    }

    private void Update() {
        if(doUpdateTime) {
            updateTime();
        }

        if(timeShown == null) {
            timeShown = GameObject.Find("Canvas").transform.Find("ClockText").gameObject.GetComponent<TMP_Text>();
        }
        timeShown.text = Mathf.FloorToInt(time).ToString();
    }
}
