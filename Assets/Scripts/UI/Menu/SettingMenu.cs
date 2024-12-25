using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Digital_Subcurrent
{
    public class SettingMenu : MonoBehaviour
    {
        public AudioMixer audioMixer;

        void Update() { 
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                CloseSettingMenu();
            }
        }

        public void OpenSettingMenu()
        {
            gameObject.SetActive(true);
        }

        public void CloseSettingMenu()
        {
            gameObject.SetActive(false);
        }

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

        public void SetFullScreen(bool isFullScreen)
        {
            Screen.fullScreen = isFullScreen;
        }
    }

}