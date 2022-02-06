using UnityEngine;

public class onEnterLevel : MonoBehaviour {
    [SerializeField] private string music;
    private void Start() {
        FindObjectOfType<gamePlayManager>().onStageEnter(music);
    }
}
