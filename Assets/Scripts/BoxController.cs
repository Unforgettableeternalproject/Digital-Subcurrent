using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent
{
    using UnityEngine;

    public class BoxController : MonoBehaviour
    {
        public Vector2 gridSize = new Vector2(1, 1); // 格子大小
        public float moveSpeed = 5f; // 移動速度
        private bool isMoving = false;
        private Vector2 targetPosition;
        private CollisionHandler collisionHandler;

        public GameManager gameManager;

        private void Start()
        {
            targetPosition = transform.position; // 初始化目標位置
            collisionHandler = GetComponent<CollisionHandler>();
        }

        private void Update()
        {
            if (isMoving)
            {
                MoveTowardsTarget();
            }
        }

        public bool TryMove(Vector2 direction)
        {
            if (isMoving) return false; // 如果正在移動，無法再推動

            Vector2 boxPosition = transform.position;
            // 檢查目標位置是否有效（可根據具體邏輯擴展檢查條件）
            if (gameManager.BoxTryMove(new Vector2(direction.x, -direction.y)))
            {
                PlaySound();
                targetPosition = boxPosition + direction * gridSize;
                isMoving = true;
                return true; // 推動成功
            }

            return false; // 推動失敗
        }
        
        private void MoveTowardsTarget()
        {
            // 平滑移動
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 判斷是否到達目標位置
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // 精確對齊格子
                isMoving = false;
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
