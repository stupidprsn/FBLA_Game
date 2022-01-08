using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class gamePlayManager : MonoBehaviour {
    private TMP_Text timeShown;

    private bool doUpdateTime = true;
    private float time;

    public void onWin() {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
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
