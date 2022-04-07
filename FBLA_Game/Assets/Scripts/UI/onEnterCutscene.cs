using UnityEngine;

public class onEnterCutscene : MonoBehaviour {
    [SerializeField] private gameManager gameManagerPreset;

    private void Awake() {
        if(FindObjectOfType<gameManager>() == null) {
            gameManager currentGameManager = Instantiate(gameManagerPreset);
        }

        FindObjectOfType<fileManager>().loadDefaults();
        UserSettings userSettings = FindObjectOfType<fileManager>().loadUserSettings();
    }
}
