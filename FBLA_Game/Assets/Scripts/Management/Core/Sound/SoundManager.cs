using System;
using UnityEngine;

namespace JonathansAdventure.Sound
{
    /// <summary>
    ///     Manages audio and music
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/10/2022
    ///     </para>
    ///     <para>
    ///         This class was inspired by "Introduction to AUDIO in Unity"
    ///         by "Asbjørn Thirslund (Brackeys)" 2017.
    ///         
    ///         The members: <see cref="sounds"/>, <see cref="PlaySound(string, bool)"/>, 
    ///         <see cref="Awake"/>, and <see cref="FindSound(SoundNames)"/>
    ///         are creddited to Brackeys.
    ///         
    ///         <seealso href="https://www.youtube.com/watch?v=6OT43pvUyfY"/>
    ///     </para>
    /// </remarks>
    public class SoundManager : MonoBehaviour
    {
        [SerializeField, Tooltip("Set all of the audios")]
        private Sound[] sounds;

        /// <summary>
        ///     Implements singleton pattern.
        /// </summary>
        public static SoundManager Instance { get; private set; }

        /// <summary>
        ///     Plays a sound.
        /// </summary>
        /// <param name="name"> Used to determine which song to play. </param>
        public void PlaySound(SoundNames name)
        {
            FindSound(name).Source.Play();
        }

        /// <summary>
        ///     Stops a sound from playing.
        /// </summary>
        /// <param name="name"> The name of the sound </param> 
        public void StopSound(SoundNames name)
        {
            Sound soundToStop = FindSound(name);
            // Checks to make sure the sound is playing
            if (soundToStop.Source.isPlaying)
            {
                soundToStop.Source.Stop();
            } else
            {
                Debug.LogError("The sound " + soundToStop.Name + " is not playing");
            }
        }

        /// <summary>
        ///     Stops all currently playing sounds.
        /// </summary>
        public void StopAllSound()
        {
            foreach (Sound soundToStop in sounds)
            {
                if (soundToStop.Source.isPlaying)
                {
                    soundToStop.Source.Stop();
                }
            }
        }

        /// <summary>
        ///     Overload of <see cref="StopAllSound"/> in which the songs dictated by
        ///     <paramref name="exception"/> are allowed to continue playing.
        /// </summary>
        /// <param name="exception"> The songs that are allowed to keep playing. </param>
        public void StopAllSound(params SoundNames[] exception)
        {
            foreach (Sound soundToStop in sounds)
            {
                if (soundToStop.Source.isPlaying)
                {
                    // Check our sound against our list of exceptions
                    bool stop = true;
                    foreach (SoundNames name in exception)
                    {
                        if (soundToStop.Name == name)
                        {
                            stop = false;
                            break;
                        }
                    }
                    if (stop)
                    {
                        soundToStop.Source.Stop();
                    }
                }
            }
        }

        /// <summary>
        ///     Sets the master volume of the game.
        /// </summary>
        /// <remarks>
        ///     Multiplies all sound volumes by a factor.
        /// </remarks>
        /// <param name="volume"> The factor that all sounds are multiplied by. </param>
        public void SetVolume(float volume)
        {
            foreach (Sound sound in sounds)
            {
                sound.MultiplyVolume(volume);
            }
        }

        /// <summary>
        ///     Used privately to find the reference to a specific <see cref="Sound"/>.
        /// </summary>
        /// <param name="name"> The name being searched for </param>
        /// <returns> 
        ///     The <see cref="Sound"/> associated with <paramref name="name"/>
        /// </returns>
        private Sound FindSound(SoundNames name)
        {
            return Array.Find(sounds, sound => sound.Name == name);
        }

        /// <summary>
        ///     For each sound in the sounds list, 
        ///     create an AudioSource, which is used to play audio in the Unity Engine.
        /// </summary>
        private void Awake()
        {
            // Singleton pattern Check
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
            } else
            {
                Instance = this;
            }

            foreach (Sound sound in sounds)
            {
                sound.Source = gameObject.AddComponent<AudioSource>();
            }
        }
    }

}

