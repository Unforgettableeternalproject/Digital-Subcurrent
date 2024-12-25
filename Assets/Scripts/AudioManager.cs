using UnityEngine;
using UnityEngine.Audio;

namespace Digital_Subcurrent
{
    public class AudioManager : MonoBehaviour
    {
        public AudioMixer audioMixer;

        public void SetMasterVolume(float volume)
        {
            audioMixer.SetFloat("MasterVolume", volume);
        }

        public void SetMusicVolume(float volume)
        {
            audioMixer.SetFloat("MusicVolume", volume);
        }

        public void SetSFXVolume(float volume)
        {
            audioMixer.SetFloat("SFXVolume", volume);
        }
    }
}

