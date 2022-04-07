[System.Serializable]
public class UserProgress {
    public int level;
    public UserProgress(int inputLevel) {
        this.level = inputLevel;
    }

    public UserProgress() {
        this.level = 1;
    }
}