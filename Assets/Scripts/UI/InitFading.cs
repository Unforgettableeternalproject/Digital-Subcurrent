using UnityEngine;

public class FadeOut : MonoBehaviour
{
    public CanvasGroup canvasGroup; // ���V�¦�e���� CanvasGroup
    public float fadeDuration = 2f; // �H�X�ɶ�
    public float targetAlpha = 0.2f; // �̲׳z����

    private bool isFading = false;

    void Start()
    {
        // �T�O�_�l�z���׬� 1
        canvasGroup.alpha = 1;
        StartFadeOut();
    }

    public void StartFadeOut()
    {
        if (!isFading) // ����ƽե�
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

        canvasGroup.alpha = targetAlpha; // �T�O�̲׭�
        canvasGroup.interactable = false; // ����̽��椬
        canvasGroup.blocksRaycasts = false;

        isFading = false;
    }
}
