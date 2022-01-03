using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gamePlayManager : MonoBehaviour {
    public string numberOfLetters;
    //tomb fun
    //numbers are superior to letter puzzles change my mind
    private List<int> rock = new List<int>();


    void Start() {
        for (int i = 0; i < 3; i++)
        {
            rock.Add(Random.Range(0, 9));
           

        }
        foreach (var item in rock)
        {
            Debug.Log(item);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
