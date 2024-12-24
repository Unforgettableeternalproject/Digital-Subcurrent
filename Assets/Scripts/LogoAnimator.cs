using UnityEngine;
using UnityEngine.UI;

namespace Digital_Subcurrent
{
    public class LogoAnimator : MonoBehaviour
    {
        public Sprite[] frames; // 動畫幀的圖片
        public float frameRate = 0.1f; // 每幀顯示時間

        private Image image;
        private int currentFrame;
        private float timer;

        private AudioSource audioSource;

        void Start()
        {
            audioSource = GetComponent<AudioSource>();
            image = GetComponent<Image>();
            if (frames.Length > 0)
            {
                image.sprite = frames[0];
            }
        }

        void Update()
        {
            if (frames.Length == 0) return;
            if (currentFrame == frames.Length - 1) return;

            timer += Time.deltaTime;
            if (timer >= frameRate)
            {
                timer -= frameRate;
                currentFrame += 1;
                image.sprite = frames[currentFrame];
                if (currentFrame % 2 == 0 && currentFrame > 2) audioSource.Play();
            }
        }
    }
}