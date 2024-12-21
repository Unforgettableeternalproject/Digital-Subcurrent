using UnityEngine;
using UnityEngine.UI;
using System.Collections;

namespace Digital_Subcurrent
{
    public class TransitionManager : MonoBehaviour
    {
        public Image fadeImage; // �«̭I��
        public Slider progressBar; // �i�ױ�
        public Text loadingText; // ���J��r���ܡ]�i��^
        public float fadeDuration = 1f; // �H�J�H�X�ɶ�

        // �����ж��D��k
        public IEnumerator TransitionToRoom(System.Action loadRoomAction)
        {
            // Step 1: �H�J�«�
            yield return StartCoroutine(FadeIn());

            // Step 2: ��ܶi�ױ��ü����[��
            yield return StartCoroutine(ShowLoadingProgress(loadRoomAction));

            // Step 3: �H�X�«�
            yield return StartCoroutine(FadeOut());
        }

        private IEnumerator FadeIn()
        {
            float elapsedTime = 0f;
            while (elapsedTime < fadeDuration)
            {
                elapsedTime += Time.deltaTime;
                float alpha = elapsedTime / fadeDuration;
                fadeImage.color = new Color(0, 0, 0, alpha); // �����ܶ�
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
                fadeImage.color = new Color(0, 0, 0, alpha); // �����ܳz��
                yield return null;
            }
        }

        private IEnumerator ShowLoadingProgress(System.Action loadRoomAction)
        {
            // ��ܶi�ױ�
            progressBar.gameObject.SetActive(true);
            loadingText.gameObject.SetActive(true); // �i��

            float fakeProgress = 0f;

            // �������J�i��
            while (fakeProgress < 1f)
            {
                fakeProgress += Time.deltaTime * 0.5f; // ���]���J�t��
                progressBar.value = fakeProgress;
                yield return null;
            }

            // �u������ж�����
            loadRoomAction?.Invoke();

            // ���öi�ױ�
            progressBar.gameObject.SetActive(false);
            loadingText.gameObject.SetActive(false); // �i��
        }
    }
}