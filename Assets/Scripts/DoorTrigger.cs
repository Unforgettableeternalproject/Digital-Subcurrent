using UnityEngine;

namespace Digital_Subcurrent
{
    public class DoorTrigger : MonoBehaviour
    {
        private string targetRoomName; // 目標房間的名稱
        private LevelLoader levelLoader;

        void Start()
        {
            levelLoader = FindFirstObjectByType<LevelLoader>();
            if (levelLoader == null)
            {
                Debug.LogError("LevelLoader not found!");
            }
            // 獲取當前門的房間 ID，例如通過門所在的父物件名字
            var currentRoomId = transform.parent.parent.name;

            // 假設房間命名為 SL-1, SL-2, SL-3 ...
            int currentRoomNumber = int.Parse(currentRoomId.Split('-')[1]);
            targetRoomName = "SL-" + (currentRoomNumber + 1);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"Player entered door to {targetRoomName}");
                levelLoader.MovePlayerToRoom(targetRoomName);
            }
        }
    }
}
