using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Digital_Subcurrent
{
    public class HoleController : MonoBehaviour, IRewindable
    {
        public GameObject filledFloorPrefab; // 已填滿的地板Prefab
        private GameManager gameManager;       // 遊戲管理器

        private bool isFilled = false;
        public bool isActive = true;
        private string uniqueId;

        private void Awake()
        {
            uniqueId = $"{gameObject.name}_{GetInstanceID()}";
            GameManager.Instance.RegisterRewindable(this);
        }

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


                other.gameObject.SetActive(false);
                this.gameObject.SetActive(false);
                // Destroy(other.gameObject); // 移除箱子
                // Destroy(gameObject);       // 移除空洞

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

        public RewindDataBase SaveData()
        {
            var data = new HoleRewindData();
            data.position = transform.position;
            data.isFilled = isFilled;
            data.isActive = gameObject.activeSelf;
            return data;
        }

        public void LoadData(RewindDataBase data)
        {
            if (data is HoleRewindData fdata)
            {
                transform.position = fdata.position;
                isFilled = fdata.isFilled;
                gameObject.SetActive(fdata.isActive);
            }
            else
            {
                //嘿嘿 見鬼了
                Debug.LogWarning($"{nameof(HoleController)}: LoadData() 收到的資料不是 HoleController!");
            }
        }

        public string GetUniqueId()
        {
            return uniqueId;
        }
    }
}
