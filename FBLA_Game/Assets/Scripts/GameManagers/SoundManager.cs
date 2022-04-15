/*
 * Hanlin Zhang
 * Purpose: Manages audio and music
 * 
 * This script was inspired by "Introduction to AUDIO in Unity" by "Brackeys" 2017.
 * https://www.youtube.com/watch?v=6OT43pvUyfY 
 */

using System;
using UnityEngine;

public class SoundManager : MonoBehaviour {
    // Array of sounds
    [SerializeField] private Sound[] sounds;

    // Method for playing a sound
    // Takes in one parameter, name, which is used to indicate which song to play
    public void PlaySound(string name, bool playMultiple = false) {
        // Find the sound by its name
        Sound soundToPlay = Array.Find(sounds, sound => sound.name == name);
        // Checks to make sure the sound isn't already playing
        if(!soundToPlay.source.isPlaying) {
            soundToPlay.source.Play();
        } else {
            Debug.Log("The sound " + soundToPlay.name + " is already playing");
        }
    }

    // Method for stopping a sound
    // Takes in one parameter, name, which is used to indicate which song to stop
    public void stopSound(string name) {
        // Find the sound by its name
        Sound soundToStop = Array.Find(sounds, sound => sound.name == name);
        // Checks to make sure the sound is playing
        if (soundToStop.source.isPlaying) {
            soundToStop.source.Stop();
        } else {
            Debug.Log("The sound " + soundToStop.name + " is not playing");
        }
    }

    // Method for stopping all playing sound
    // Iterates through each sound and checks if it is playing, if it is, stop it
    public void stopAllSound() {
        foreach(Sound soundToStop in sounds) {
            if(soundToStop.source.isPlaying) {
                soundToStop.source.Stop();
            }
        }
    }

    // Variant of the stopAllSound method
    // Takes in a list of paramenters that dictates which sounds should be excluded
    public void stopAllSound(params string[] exception) {
        foreach (Sound soundToStop in sounds) {
            if (soundToStop.source.isPlaying) {
                // Check our sound against our list of exceptions
                bool stop = true;
                foreach (string name in exception) {
                    if(soundToStop.name == name) {
                        stop = false;
                    }
                }
                if(stop) {
                    soundToStop.source.Stop();
                }
            }
        }
    }

    public void setVolume(float volume) {
        foreach (Sound sound in sounds) {
            sound.volume *= volume;
            sound.source.volume = volume;
        }
    }

    // For each sound in the sounds list, create an AudioSource, which is used to play audio in the Unity Engine
    private void Awake() {
        foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }
    }
}
