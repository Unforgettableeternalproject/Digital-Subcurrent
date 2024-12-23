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
                PlaySound(); // ���񭵮�
                isHovered = true;
            }
        }

        private void OnTriggerExit2D(Collider2D other)
        {
            if (isHovered)
            {
                Transform parentContainer = transform.parent;
                // �������w�񺡪��a�O
                Instantiate(holePrefab, transform.position, Quaternion.identity, parentContainer);
                Destroy(gameObject);       // ���������a�O

                isHovered = false;
            }
        }

        private void PlaySound()
        {
            // �Ыؤ@���{�ɭ��Ī���
            GameObject audioPlayer = new GameObject("TempAudioPlayer");
            AudioSource tempAudio = audioPlayer.AddComponent<AudioSource>();
            tempAudio.clip = GetComponent<AudioSource>().clip;
            tempAudio.volume = 0.5f;
            tempAudio.Play();

            // �۰ʾP�����Ī���
            Destroy(audioPlayer, tempAudio.clip.length);
        }
    }
}
