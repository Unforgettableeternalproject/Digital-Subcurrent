using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

namespace Digital_Subcurrent
{
    public class PauseMenu : MonoBehaviour
    {
        public static bool IsPaused = false;

        public GameObject pauseMenuUI;
        public SettingMenu settingMenu;
        public CanvasGroup canvasGroup;

        private SceneManagerCUS sceneManagerCUS;

        void Start()
        {
            sceneManagerCUS = SceneManagerCUS.Instance;
        }

        void Update() 
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (IsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public void Resume()
        {
            canvasGroup.blocksRaycasts = false;
            canvasGroup.alpha = 0;
            pauseMenuUI.SetActive(false);
            Time.timeScale = 1f;
            IsPaused = false;
        }

        public void Pause()
        {
            canvasGroup.blocksRaycasts = true;
            canvasGroup.alpha = 1;
            pauseMenuUI.SetActive(true);
            Time.timeScale = 0f;
            IsPaused = true;
        }

        public void OpenSettingMenu()
        {
            Resume();
            settingMenu.OpenSettingMenu();
        }

        public void ReturnToMain() 
        {
            Resume();
            sceneManagerCUS.ChangeScene("Menu");
        }

        public void ReturnToMap()
        {
            Resume();
            sceneManagerCUS.ChangeScene("Level");
        }
    }

}