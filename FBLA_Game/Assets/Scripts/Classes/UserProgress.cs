/*
 * Hanlin Zhang
 * Purpose: Sets up a class to store which level the user has gotten to
 */

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