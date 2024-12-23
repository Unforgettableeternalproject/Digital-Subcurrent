using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Digital_Subcurrent
{
    public class TransitionManager : MonoBehaviour
    {
        public Image fadeImage; // 黑屏背景
        public Slider progressBar; // 進度條
        public Text loadingText; // 載入文字提示（可選）
        public float fadeDuration = 1f; // 淡入淡出時間

        // 切換房間主方法
        public IEnumerator TransitionToRoom(System.Action loadRoomAction)
        {
            // Step 1: 淡入黑屏
            yield return StartCoroutine(FadeIn());

            // Step 2: 顯示進度條並模擬加載
            yield return StartCoroutine(ShowLoadingProgress(loadRoomAction));

            // Step 3: 淡出黑屏
            yield return StartCoroutine(FadeOut());
        }

        private IEnumerator FadeIn()
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = elapsedTime / fadeDuration;
                fadeImage.color = new Color(0, 0, 0, alpha); // 漸漸變黑
                yield return null;
            }
        }

        private IEnumerator FadeOut()
        {
            float elapsedTime = fadeDuration;
            while (elapsedTime > 0)
            {
                elapsedTime -= Time.deltaTime;
                float alpha = elapsedTime / fadeDuration;
                fadeImage.color = new Color(0, 0, 0, alpha); // 漸漸變透明
                yield return null;
            }
        }

        private IEnumerator ShowLoadingProgress(System.Action loadRoomAction)
        {
            // 顯示進度條
            progressBar.gameObject.SetActive(true);
            loadingText.gameObject.SetActive(true); // 可選

            float fakeProgress = 0f;

            // 模擬載入進度
            while (fakeProgress < 1f)
            {
                fakeProgress += Time.deltaTime * 0.5f; // 假設載入速度
                progressBar.value = fakeProgress;
                yield return null;
            }

            // 真正執行房間切換
            loadRoomAction?.Invoke();

            // 隱藏進度條
            progressBar.gameObject.SetActive(false);
            loadingText.gameObject.SetActive(false); // 可選
        }
    }
}