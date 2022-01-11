using System;
using UnityEngine;
using UnityEngine.Audio;

public class soundManager : MonoBehaviour {
    public Sound[] sounds;

    public void PlaySound(string name) {
        Sound soundToPlay = Array.Find(sounds, sound => sound.name == name);
        soundToPlay.source.Play();
    }

    public void stopSound(string name) {
        Sound soundToPlay = Array.Find(sounds, sound => sound.name == name);
        soundToPlay.source.Stop();
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
