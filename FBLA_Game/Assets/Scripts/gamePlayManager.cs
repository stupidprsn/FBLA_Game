using UnityEngine;
using TMPro;

public class gamePlayManager : MonoBehaviour {
    public TMP_Text timeShown;
    private float time;

    private void Update() {
        time += Time.deltaTime;

        timeShown.text = Mathf.FloorToInt(time).ToString();
    }
}
