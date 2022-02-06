using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class soundManager : MonoBehaviour {
    public Sound[] sounds;
    //private List<Sound> playingSounds = new List<Sound>();

    public void PlaySound(string name) {
        Sound soundToPlay = Array.Find(sounds, sound => sound.name == name);
        soundToPlay.source.Play();

        //if(soundToPlay.loop == true) {
        //    playingSounds.Add(soundToPlay);
        //}
    }

    public void stopSound(string name) {
        Sound soundToStop = Array.Find(sounds, sound => sound.name == name);
        soundToStop.source.Stop();
        //playingSounds.Remove(soundToStop);
    }

    public void stopAllSound() {
        foreach(Sound soundToStop in sounds) {
            if(soundToStop.source.isPlaying) {
                soundToStop.source.Stop();
            }
        }

        //foreach(Sound soundToStop in playingSounds) {
        //    soundToStop.source.Stop();
        //}

        //playingSounds.Clear();
    }

    private void Awake() {
        foreach(Sound sound in sounds) {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.clip = sound.clip;
            sound.source.volume = sound.volume;
            sound.source.loop = sound.loop;
        }
    }


}
