using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Digital_Subcurrent
{
    public class LevelLoader : MonoBehaviour
    {
        public Transform player;
        public TransitionManager transitionManager;
        private Dictionary<string, Transform> roomEntryPoints;

        void Start()
        {
            roomEntryPoints = new Dictionary<string, Transform>();
            // �M��Ҧ��ж��� EntryPoint
            foreach (Transform room in transform)
            {
                Transform entryPoint = room.Find("EntryPoint");
                if (entryPoint != null)
                {
                    roomEntryPoints.Add(room.name, entryPoint);
                }
            }
            LoadLevel("SL-1");
        }

        public void LoadLevel(string levelName)
        {
            Transform levelTransform = transform.Find(levelName);
            if (levelTransform == null)
            {
                Debug.LogError($"Level {levelName} not found!");
                return;
            }

            // �q�өж����󤤧� LoadMapInfo
            var loadMapInfo = levelTransform.GetComponent<TilemapTagArray>();
            if (loadMapInfo == null)
            {
                Debug.LogError($"LoadMapInfo not found in {levelName}!");
                return;
            }

            // 1. ���ͩж����x�}���
            loadMapInfo.GenerateMapData();
            // �o�|���� objectMatrix, floorMatrix (�i�H�s�b�����ݩʸ�)

            // 2. ����o�Ǹ��
            int[,] objectMatrix = loadMapInfo.GetObjectMatrix();
            int[,] floorMatrix = loadMapInfo.GetFloorMatrix();

            // 3. ���ƥ浹 GameManager
            GameManager.Instance.InitializeGame(objectMatrix, floorMatrix);

            // 4. �]�w���a����m
            MovePlayerToRoom(levelName);

            Debug.Log($"Finish loading {levelName}");
        }

        // �ǰe���a����w�ж�
        public void MovePlayerToRoom(string roomName)
        {
            if (roomEntryPoints.ContainsKey(roomName))
            {
                System.Action loadRoomAction = () =>
                {
                    Transform entryPoint = roomEntryPoints[roomName];
                    player.position = entryPoint.position;
                };
               // StartCoroutine(transitionManager.TransitionToRoom(loadRoomAction));
            }
            else
            {
                Debug.LogError($"Room '{roomName}' not found!");
            }
        }
    }
}
