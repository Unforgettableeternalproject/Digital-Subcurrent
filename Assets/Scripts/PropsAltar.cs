﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent
{
    public class PropsAltar : MonoBehaviour
    {
        public List<SpriteRenderer> runes;
        public float lerpSpeed;
        public GameObject closedDoor; // 關閉門的 prefab
        public GameObject openDoor;   // 開啟門的 prefab
        public Transform doorPosition; // 門的位置 (同一位置切換)

        private Color curColor;
        private Color targetColor;

        private void Awake()
        {
            targetColor = runes[0].color;
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            Debug.Log("Test");
            PlayerController player = other.GetComponent<PlayerController>();
            if (player != null && player.HasKey())
            {
                Debug.Log("Altar activated!");
                targetColor.a = 1.0f; // 符文發光

                // 切換門狀態
                OpenTheDoor();
            }
        }

        private void Update()
        {
            curColor = Color.Lerp(curColor, targetColor, lerpSpeed * Time.deltaTime);
            foreach (var r in runes)
            {
                r.color = curColor;
            }
        }

        private void OpenTheDoor()
        {
            // 移除舊門並生成新門
            if (closedDoor != null && openDoor != null)
            {
                Destroy(closedDoor);
                Instantiate(openDoor, doorPosition.position, Quaternion.identity);
                PlaySound();
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
