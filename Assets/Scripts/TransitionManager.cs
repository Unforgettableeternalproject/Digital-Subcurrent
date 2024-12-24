using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Digital_Subcurrent
{
    public class TransitionManager : MonoBehaviour
    {
        public static TransitionManager Instance;

        [Header("UI Components")]
        public ProgressBarCircle progressBar; // �i�ױ�
        public CanvasGroup blackScreen; // �µe���� CanvasGroup
        public Text transitionText; // �ʺA��r

        [Header("Settings")]
        public float fadeDuration = 1.0f; // �µe���H�J/�H�X�ɶ�
        public float transitionDuration = 2.0f; // �[���L��ɶ�
        public string[] descriptions; // �L��ɪ���r�y�z

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
                progressBar.BarValue = 0; // ��l�ƶi�ױ�
            if (blackScreen != null)
                blackScreen.alpha = 0; // �T�O�µe���O�z����
        }

        public void StartTransition()
        {
            
            if (!isTransitioning)
            {
                StartCoroutine(TransitionRoutine());
            }
        }

        private IEnumerator TransitionRoutine()
        {
            progressBar.ShowProgressBar();
            isTransitioning = true;

            // �H����ܹL��y�z
            if (descriptions.Length > 0)
            {
                transitionText.text = descriptions[Random.Range(0, descriptions.Length)];
            }

            // �H�J�µe��
            yield return StartCoroutine(FadeBlackScreen(1));

            // �}�l�[�������ç�s�i�ױ�
            StartCoroutine(UpdateTransitionText()); // �ʺA��r�ʵe
            yield return StartCoroutine(UpdateProgressBar());

            // �[�������A�H�X�µe��
            yield return StartCoroutine(FadeBlackScreen(0));

            isTransitioning = false;
            progressBar.HideProgressBar();
        }

        private IEnumerator FadeBlackScreen(float targetAlpha)
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
                dotCount = (dotCount + 1) % 4; // �`����ܡu...�v
                yield return new WaitForSeconds(0.5f); // �C 0.5 ���s�@��
            }
        }
    }
}