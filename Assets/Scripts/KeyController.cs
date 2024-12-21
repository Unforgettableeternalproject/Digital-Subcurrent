using System.Collections;
using UnityEngine;

namespace Digital_Subcurrent
{
    public class KeyController : MonoBehaviour
    {
        public float floatAmplitude = 0.02f; // 上下漂浮的幅度
        public float floatSpeed = 1.2f;       // 漂浮的速度
        private Vector3 startPos;

        private void Start()
        {
            startPos = transform.position;
        }

        private void Update()
        {
            // 使用 Sin 函數讓鑰匙上下浮動
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }

        public void OnCollect()
        {
            // 播放收集音效
            PlaySound();
            // 銷毀自己
            Destroy(gameObject);
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
