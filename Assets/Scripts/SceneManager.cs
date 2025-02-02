﻿using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Digital_Subcurrent
{
    public class SceneManagerCUS : MonoBehaviour
    {
        public static SceneManagerCUS Instance;

        private TransitionManager transitionManager; // 指向 TransitionManager
        public float transitionDuration = 1f;       // 轉場動畫的持續時間

        public AudioClip BGM;
        public AudioSource audioSource;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        void Start()
        {
            if (audioSource == null)
            {
                Debug.LogError("AudioSource not found!");
            }

            audioSource.enabled = true;
            audioSource.clip = BGM;
            audioSource.Play();
            transitionManager = TransitionManager.Instance;
        }

        // 切換場景的方法
        public void ChangeScene(string sceneName)
        {
            StartCoroutine(ChangeSceneCoroutine(sceneName));
        }

        private IEnumerator ChangeSceneCoroutine(string sceneName)
        {
            // 1. 播放淡出動畫
            if (transitionManager != null)
            {
                yield return StartCoroutine(transitionManager.CoroutineStart(0.5f, 2));
            }

            // 2. 加載場景
            yield return SceneManager.LoadSceneAsync(sceneName);
        }
    }
}
