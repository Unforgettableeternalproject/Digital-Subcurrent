﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Digital_Subcurrent
{
    public class LevelLoader : MonoBehaviour
    {
        public static LevelLoader Instance;
        public Transform player;
        public Transform cameraT;
        public TransitionManager transitionManager;
        private Dictionary<string, Transform> roomEntryPoints;

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
                return;
            }

            roomEntryPoints = new Dictionary<string, Transform>();
            // 尋找所有房間的 EntryPoint
            foreach (Transform room in transform)
            {
                Transform entryPoint = room.Find("EntryPoint");
                if (entryPoint != null)
                {
                    roomEntryPoints.Add(room.name, entryPoint);
                }
            }
        }

        //void Start()
        //{
        //    transitionManager = TransitionManager.Instance;
        //}

        public IEnumerator LoadLevel(string levelName)
        {
            Transform levelTransform = transform.Find(levelName);
            if (levelTransform == null)
            {
                Debug.LogError($"Level {levelName} not found!");
                yield return 0;
            }

            // 從該房間物件中抓 LoadMapInfo
            var loadMapInfo = levelTransform.GetComponent<TilemapTagArray>();
            if (loadMapInfo == null)
            {
                Debug.LogError($"LoadMapInfo not found in {levelName}!");
                yield return 0;
            }

            StartCoroutine(transitionManager.CoroutineStart());

            // 0. 設定玩家的位置
            MovePlayerToRoom(levelName);

            if (cameraT != null)
            {
                cameraT.position = new Vector3(player.position.x, player.position.y, cameraT.position.z);
            }

            yield return new WaitForEndOfFrame();

            // 1. 產生房間的矩陣資料
            loadMapInfo.GenerateMapData();
            // 這會產生 objectMatrix, floorMatrix (可以存在它的屬性裡)

            // 2. 拿到這些資料
            int[,] objectMatrix = loadMapInfo.GetObjectMatrix();
            int[,] floorMatrix = loadMapInfo.GetFloorMatrix();

            // 3. 把資料交給 GameManager
            GameManager.Instance.InitializeGame(objectMatrix, floorMatrix);

            Debug.Log($"Finish loading {levelName}");
        }

        // 傳送玩家到指定房間
        public void MovePlayerToRoom(string roomName)
        {
            if (roomEntryPoints.ContainsKey(roomName))
            {
                PlayerController playerController = player.GetComponent<PlayerController>();
                Animator playerAnimator = player.GetComponent<Animator>();

                playerController.enabled = false;
                Transform entryPoint = roomEntryPoints[roomName];
                player.position = entryPoint.position;
                playerAnimator.SetBool("IsMoving", false);
                playerAnimator.SetInteger("Direction", 0);
                playerController.enabled = true;
               // StartCoroutine(transitionManager.TransitionToRoom(loadRoomAction));
            }
            else
            {
                Debug.LogError($"Room '{roomName}' not found!");
            }
        }
    }
}
