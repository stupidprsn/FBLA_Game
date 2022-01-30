/*
 * Hanlin Zhang
 * Purpose: Used in the UI, for any panel where the user can press space to continue
 */

using UnityEngine;

public class gameUI : MonoBehaviour {
    
    void Start() {
        FindObjectOfType<gamePlayManager>().updateCanvas();
    }
}
