/*
 * Hanlin Zhang
 * Purpose: Class for accessing and using audio
 * 
 * This script was inspired by "Introduction to AUDIO in Unity" by "Brackeys" 2017.
 * Credit for Sound class goes to "Brackeys"
 * https://www.youtube.com/watch?v=6OT43pvUyfY 
 */

using UnityEngine;

[System.Serializable]
public class Sound {
    public string name;
    public AudioClip clip;
    public bool loop;

    [Range(0f, 1f)]
    public float volume;


    [HideInInspector]
    public AudioSource source;
}
