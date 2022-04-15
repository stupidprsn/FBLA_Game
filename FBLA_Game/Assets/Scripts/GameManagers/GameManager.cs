/*
 * Hanlin Zhang
 * Purpose: Misc methods used to manage the game
 */

using UnityEngine;

public class GameManager : MonoBehaviour {
    // Singleton Check for making sure only one GameManager exists
    public static GameManager singletonCheck;

    // Referance to other management scripts
    [Header("Object/Prefab References")]
    [SerializeField] private GameObject gamePlayManagerPrefab;
    [SerializeField] private GameObject fileManagerPrefab;
    [SerializeField] private SoundManager soundManager;

    // Read by onEnterMainMenu to know what panel to load
    [HideInInspector] public int mainMenuPanel = 0;

    // Applys the user's saved settings
    public void SetSettings(UserSettings userSettings) {
        Screen.SetResolution(userSettings.display.x, userSettings.display.y, userSettings.display.fullScreen);
        soundManager.SetVolume(userSettings.volume);
    }

    private void Awake() {
        // Singleton Check
        if (singletonCheck == null) {
            singletonCheck = this;
        } else {
            Destroy(gameObject);
        }

        // Keep the game manager in all scenes
        DontDestroyOnLoad(transform.parent.gameObject);

        // Set the screen resolution and lock the mouse
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update() {
        // Allow the application to quit with the esc key
        if (Input.GetKeyDown("escape")) {
            Application.Quit();
        }
    }
}
