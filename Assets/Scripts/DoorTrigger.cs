using UnityEngine;

namespace Digital_Subcurrent
{
    public class DoorTrigger : MonoBehaviour
    {
        private string targetRoomName; // �ؼЩж����W��
        private LevelLoader levelLoader;

        void Start()
        {
            levelLoader = LevelLoader.Instance;
            if (levelLoader == null)
            {
                Debug.LogError("LevelLoader not found!");
            }
            // �����e�����ж� ID�A�Ҧp�q�L���Ҧb��������W�r
            var currentRoomId = transform.parent.parent.parent.name;
            Debug.Log($"Current room ID: {currentRoomId}");
            // ���]�ж��R�W�� SL-1, SL-2, SL-3 ...
            string currentRoomName = currentRoomId.Split('-')[0];
            int currentRoomNumber = int.Parse(currentRoomId.Split('-')[1]);
            targetRoomName = $"{currentRoomName}-" + (currentRoomNumber + 1);
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                Debug.Log($"Player entered door to {targetRoomName}");
                StartCoroutine(levelLoader.LoadLevel(targetRoomName));
                //levelLoader.LoadLevel(targetRoomName);
            }
        }
    }
}
