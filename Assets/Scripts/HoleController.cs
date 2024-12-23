using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Digital_Subcurrent
{
    public class HoleController : MonoBehaviour
    {
        public GameObject filledFloorPrefab; // 已填滿的地板Prefab
        private GameManager gameManager;       // 遊戲管理器

        private bool isFilled = false;

        private void Start()
        {
            gameManager = GameManager.Instance;
        }

        // 當箱子進入時觸發
        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Test");
            if (!isFilled && other.CompareTag("Box"))
            {
                Debug.Log("Box filled the hole!");

                PlaySound(); // 播放音效
                Transform parentContainer = transform.parent;
                // 切換為已填滿的地板
                Instantiate(filledFloorPrefab, transform.position, Quaternion.identity, parentContainer);
                Destroy(other.gameObject); // 移除箱子
                Destroy(gameObject);       // 移除空洞

                isFilled = true;
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
