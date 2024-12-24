using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public CanvasGroup canvasGroup; // 指向黑色畫面的 CanvasGroup
    public float fadeDuration = 2f; // 淡出時間
    public float targetAlpha = 0.2f; // 最終透明度

    private bool isFading = false;

    void Start()
    {
        // 確保起始透明度為 1
        canvasGroup.alpha = 1;
        StartFadeOut();
    }

    public void StartFadeOut()
    {
        if (!isFading) // 防止重複調用
        {
            isFading = true;
            StartCoroutine(FadeOutRoutine());
        }
    }

    private System.Collections.IEnumerator FadeOutRoutine()
    {
        float elapsed = 0f;
        float startAlpha = canvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(startAlpha, targetAlpha, elapsed / fadeDuration);
            yield return null;
        }

        canvasGroup.alpha = targetAlpha; // 確保最終值
        canvasGroup.interactable = false; // 停止屏蔽交互
        canvasGroup.blocksRaycasts = false;

        isFading = false;
    }
}
