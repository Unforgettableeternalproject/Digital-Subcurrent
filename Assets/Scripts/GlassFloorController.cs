using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent
{
    public class GlassFloorController : MonoBehaviour
    {
        public GameObject holePrefab;
        private GameManager gameManager;

        private bool isHovered = false;

        void Start()
        {
            gameManager = GameManager.Instance;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isHovered)
            {
                Debug.Log("Player stepped on the glass floor!");
                PlaySound(); // 播放音效
                isHovered = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (isHovered)
            {
                Transform parentContainer = transform.parent;
                // 切換為已填滿的地板
                Instantiate(holePrefab, transform.position, Quaternion.identity, parentContainer);
                Destroy(gameObject);       // 移除玻璃地板

                isHovered = false;
            }
        }

        private void PlaySound()
        {
            // 創建一個臨時音效物件
            GameObject audioPlayer = new GameObject("TempAudioPlayer");
            AudioSource tempAudio = audioPlayer.AddComponent<AudioSource>();
            tempAudio.clip = GetComponent<AudioSource>().clip;
            tempAudio.volume = 0.5f;
            tempAudio.Play();

            // 自動銷毀音效物件
            Destroy(audioPlayer, tempAudio.clip.length);
        }
    }
}
