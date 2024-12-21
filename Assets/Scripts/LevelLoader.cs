using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Digital_Subcurrent
{
    public class LevelLoader : MonoBehaviour
    {
        public Transform player;
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
        }

        // �ǰe���a����w�ж�
        public void MovePlayerToRoom(string roomName)
        {
            if (roomEntryPoints.ContainsKey(roomName))
            {
                Transform entryPoint = roomEntryPoints[roomName];
                player.position = entryPoint.position;
            }
            else
            {
                Debug.LogError($"Room '{roomName}' not found!");
            }
        }
    }
}
