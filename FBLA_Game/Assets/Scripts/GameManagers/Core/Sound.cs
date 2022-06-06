using UnityEngine;

namespace JonathansAdventure.GameManagers.Core
{
    /// <summary>
    ///     Assigns properties to audio source files.
    /// </summary>
    /// <remarks>
    ///     <para>
    ///         Hanlin Zhang
    ///         Last Modified: 6/5/2022
    ///     </para>
    ///     <para>
    ///         Used by <see cref="SoundManager"/> to play audio.
    ///     </para>
    ///     <para>
    ///         This class was inspired by "Introduction to AUDIO in Unity"
    ///         by "Asbjørn Thirslund (Brackeys)" 2017.
    ///         <seealso href="https://www.youtube.com/watch?v=6OT43pvUyfY"/>
    ///     </para>
    /// </remarks>
    [System.Serializable]
    public class Sound
    {
        /// <summary>
        ///     The name used to identify the sound.
        /// </summary>
        [field: SerializeField, Tooltip("The name used to identify the sound.")]
        internal string Name { get; private set; }

        [field: SerializeField, Tooltip("The source file of the sound.")]
        private AudioClip clip;

        [field: SerializeField, Tooltip("If the sound loops after termination.")]
        private bool loop;

        [field: SerializeField, Tooltip("The relative volume the sound plays at."), Range(0f, 1f)]
        private float volume;


        [HideInInspector]
        private AudioSource source;

        /// <summary>
        ///     The <see cref="UnityEngine.AudioSource"/> that the sound is played from.
        /// </summary>
        internal AudioSource Source
        {
            get { return source; }
            set
            {
                source = value;
                source.clip = this.clip;
                source.loop = this.loop;
                source.volume = this.volume;
            }
        }

        /// <summary>
        ///     Multiply the volume by a factor.
        /// </summary>
        /// <param name="multiplier"> The factor that the volume will be multiplied against. </param>
        internal void MultiplyVolume(float multiplier)
        {
            this.volume *= multiplier;
        }
    }

}

