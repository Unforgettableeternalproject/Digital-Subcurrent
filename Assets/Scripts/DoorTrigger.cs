using UnityEngine;

namespace Digital_Subcurrent
{
    public class DoorTrigger : MonoBehaviour
    {
        private string targetRoomName; // �ؼЩж����W��
        private LevelLoader levelLoader;

        void Start()
        {
            levelLoader = FindFirstObjectByType<LevelLoader>();
            if (levelLoader == null)
            {
                Debug.LogError("LevelLoader not found!");
            }
            // �����e�����ж� ID�A�Ҧp�q�L���Ҧb��������W�r
            var currentRoomId = transform.parent.parent.name;

            // ���]�ж��R�W�� SL-1, SL-2, SL-3 ...
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
