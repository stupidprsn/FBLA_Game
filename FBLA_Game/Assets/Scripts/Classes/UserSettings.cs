[System.Serializable]
public class UserSettings {
    public Display display;
    public float volume;
    public bool playCutScene;

    public UserSettings(int x = 1920, int y = 1080, bool fullScreen = true, float inputVolume = 1f, bool inputPlayCutScene = true) {
        display = new Display(x, y, fullScreen);
        volume = inputVolume;
        playCutScene = inputPlayCutScene;
    }

    public UserSettings() {
        display = new Display(1920, 1080, true);
        volume = 1f;
        playCutScene = true;
    }
}

[System.Serializable]
public class Display {
    public int x;
    public int y;
    public bool fullScreen;

    public Display(int inputX, int inputY, bool inputFullScreen) {
        x = inputX;
        y = inputY;
        fullScreen = inputFullScreen;
    }
}