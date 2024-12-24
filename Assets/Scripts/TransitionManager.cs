using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Digital_Subcurrent
{
    public class TransitionManager : MonoBehaviour
    {
        public static TransitionManager Instance;

        [Header("UI Components")]
        public ProgressBarCircle progressBar; // 進度條
        public CanvasGroup blackScreen; // 黑畫面的 CanvasGroup
        public Text transitionText; // 動態文字

        [Header("Settings")]
        public float fadeDuration = 1.0f; // 黑畫面淡入/淡出時間
        public float transitionDuration = 2.0f; // 加載過渡時間
        public string[] descriptions; // 過渡時的文字描述

        private bool isTransitioning = false;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }

        private void Start()
        {
            if (progressBar != null)
                progressBar.BarValue = 0; // 初始化進度條
            if (blackScreen != null)
                blackScreen.alpha = 0; // 確保黑畫面是透明的
        }

        public IEnumerator CoroutineStart(float delay = 0.5f)
        {   
            if (!isTransitioning)
            {
                yield return StartCoroutine(TransitionRoutine());
            }
            yield return new WaitForSeconds(delay);
        }

        private IEnumerator TransitionRoutine()
        {
            progressBar.ShowProgressBar();
            isTransitioning = true;

            // 隨機選擇過渡描述
            if (descriptions.Length > 0)
            {
                transitionText.text = descriptions[Random.Range(0, descriptions.Length)];
            }

            blackScreen.gameObject.SetActive(true);

            // 淡入黑畫面
            yield return StartCoroutine(FadeInBlackScreen(1));

            // 開始加載場景並更新進度條
            StartCoroutine(UpdateTransitionText()); // 動態文字動畫
            yield return StartCoroutine(UpdateProgressBar());

            // 加載完成，淡出黑畫面
            yield return StartCoroutine(FadeOutBlackScreen(0));

            blackScreen.gameObject.SetActive(false);

            isTransitioning = false;
            progressBar.HideProgressBar();
        }

        private IEnumerator FadeInBlackScreen(float targetAlpha)
        {
            float startAlpha = blackScreen.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                blackScreen.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / (fadeDuration / 2));
                yield return null;
            }

            blackScreen.alpha = targetAlpha;
        }

        private IEnumerator FadeOutBlackScreen(float targetAlpha)
        {
            float startAlpha = blackScreen.alpha;
            float elapsedTime = 0f;

            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                blackScreen.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / fadeDuration);
                yield return null;
            }

            blackScreen.alpha = targetAlpha;
        }

        private IEnumerator UpdateProgressBar()
        {
            float elapsedTime = 0f;

            while (elapsedTime < transitionDuration)
            {
                elapsedTime += Time.deltaTime;
                float progress = Mathf.Clamp01(elapsedTime / transitionDuration) * 100f;

                if (progressBar != null)
                {
                    progressBar.BarValue = progress;
                }

                yield return null;
            }
        }

        private IEnumerator UpdateTransitionText()
        {
            string baseText = transitionText.text;
            int dotCount = 0;

            while (isTransitioning)
            {
                transitionText.text = baseText + new string('.', dotCount);
                dotCount = (dotCount + 1) % 4; // 循環顯示「...」
                yield return new WaitForSeconds(0.5f); // 每 0.5 秒更新一次
            }
        }
    }
}