using System.Collections.Generic;

[System.Serializable]
public class Rank {

    public string name;
    public int score;

    public Rank(string inputName, int inputScore) {
        name = inputName;
        score = inputScore;
    }
}

[System.Serializable]
public class Rankings {
    public List<Rank> rankings;
}