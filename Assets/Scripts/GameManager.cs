﻿using System;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Digital_Subcurrent
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; // Singleton
        public string stageID = "SL";
        public int defaultLoadLevel = 1;

        private int[,] objectMatrix;
        private int[,] floorMatrix;
        private Stack<GameState> stateStack = new Stack<GameState>();
        private GameState initialState;
        private Vector2 offset = new Vector2(0, 0);
        private Vector2Int playerMatrixPosition;
        private Vector2Int tempBoxMPosition;
        private Vector2 gridSize = new Vector2(1, 1); // 每格的世界座標大小
        private LevelLoader levelLoader;

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
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持跨場景持續
        }

        void Start()
        {
            levelLoader = LevelLoader.Instance;
            StartCoroutine(levelLoader.LoadLevel(stageID + "-" + defaultLoadLevel));
            // 初始化遊戲
            //InitializeGame();
        }

        // 初始化矩陣
        public void InitializeGame(int[,] oM, int[,] fM)
        {
            objectMatrix = oM;
            floorMatrix = fM;
            playerMatrixPosition = FindPlayerPosition(objectMatrix);
            Debug.Log($"PlayerM = {playerMatrixPosition}");

            PrintMatrix(objectMatrix);
        }

        // 找到玩家在矩陣中的位置
        private Vector2Int FindPlayerPosition(int[,] matrix)
        {
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    if (matrix[x, y] == 1)
                    {
                        return new Vector2Int(y, x);
                    }
                }
            }
            throw new Exception("Player not found in the matrix");
        }

        // 檢查玩家是否可以移動到目標位置
        public bool PlayerTryMove(Vector2 direction)
        {
            Debug.Log($"Moving direction: {direction}");
            Vector2Int targetPosition = playerMatrixPosition + Vector2Int.RoundToInt(direction);
            if (IsOutOfBounds(targetPosition) || objectMatrix[targetPosition.y, targetPosition.x] < 0 || floorMatrix[targetPosition.y, targetPosition.x] == 1)
            {
                return false;
            }
            Debug.Log($"objectMatrix[{targetPosition.x}, {targetPosition.y}] = {objectMatrix[targetPosition.y, targetPosition.x]}");
            return true;
        }

        public bool BoxTryMove(Vector2 direction)
        {
            if (tempBoxMPosition.x == -1 || tempBoxMPosition.y == -1) return false;

            Vector2Int targetPosition = tempBoxMPosition + Vector2Int.RoundToInt(direction);
            if (IsOutOfBounds(targetPosition) || objectMatrix[targetPosition.y, targetPosition.x] < 0)
            {
                return false;
            }
            return true;
        }


        public bool HasBox(Vector2 direction)
        {
            Vector2Int targetPosition = playerMatrixPosition + Vector2Int.RoundToInt(direction);
            if (objectMatrix[targetPosition.y, targetPosition.x] == 2)
            {
                tempBoxMPosition = targetPosition;
                return true;
            }
            else
            {
                tempBoxMPosition = new Vector2Int(-1, -1);
                return false;
            }
        }

        public bool HasKey(Vector2 direction)
        {
            Vector2Int targetPosition = playerMatrixPosition + Vector2Int.RoundToInt(direction);
            if (objectMatrix[targetPosition.y, targetPosition.x] == 3)
            {
                return true;
            }
            return false;
        }

        public BoxController GetBox(Vector2 worldPosition)
        {
            if (tempBoxMPosition.x == -1 || tempBoxMPosition.y == -1) return null;
            if (objectMatrix[tempBoxMPosition.y, tempBoxMPosition.x] == 2)
            {
                // 在該位置檢測是否有碰撞的物件 (假設箱子有 "Box" Layer 或 Tag)
                Collider2D hit = Physics2D.OverlapPoint(worldPosition);
                if (hit != null && hit.CompareTag("Box")) // 使用 Tag 過濾箱子
                {
                    // 返回該物件的 BoxController
                    return hit.GetComponent<BoxController>();
                }
                return null; // 若無法檢測到箱子，返回 null
            }
            return null; // 如果沒有找到箱子，返回 null
        }

        public KeyController GetKey(Vector2 worldPosition)
        {
            Collider2D hit = Physics2D.OverlapPoint(worldPosition);
            if (hit != null && hit.CompareTag("Key"))
            {
                return hit.GetComponent<KeyController>();
            }
            return null;
        }

        // 更新矩陣
        public void UpdateMatrix(Vector2Int original, Vector2Int updated, int value)
        {
            if (value == 2) // 填洞
            {
                if (floorMatrix[updated.y, updated.x] == 1)
                {
                    value = 0;
                    UpdateFloor(updated, 0);
                    Debug.Log("Fill hole");
                }
            }

            if (floorMatrix[original.y, original.x] == 2) // 玻璃
            {
                UpdateFloor(original, 1);
            }

            objectMatrix[original.y, original.x] = 0;
            objectMatrix[updated.y, updated.x] = value;

            // SaveState();
            // Debug.Log("Update matrix");

        }

        public void UpdatePlayer(Vector2 direction)
        {
            Vector2Int original = playerMatrixPosition;
            playerMatrixPosition += Vector2Int.RoundToInt(direction);
            UpdateMatrix(original, playerMatrixPosition, 1);
        }

        public void UpdateBox(Vector2 direction)
        {
            Vector2Int original = tempBoxMPosition;
            tempBoxMPosition += Vector2Int.RoundToInt(direction);
            UpdateMatrix(original, tempBoxMPosition, 2);
        }

        public void DoorOpened()
        {
            //找到objectMatrix中-2的位置並將其替換為0
            for (int x = 0; x < objectMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < objectMatrix.GetLength(1); y++)
                {
                    if (objectMatrix[x, y] == -2)
                    {
                        objectMatrix[x, y] = 0;
                    }
                }
            }
        }

        private void PrintMatrix(int[,] matrix)
        {
            string matrixString = "";
            for (int x = 0; x < matrix.GetLength(0); x++)
            {
                for (int y = 0; y < matrix.GetLength(1); y++)
                {
                    matrixString += matrix[x, y] + " ";
                }
                matrixString += "\n";
            }
            Debug.Log(matrixString);
        }

        // 檢查邊界
        private bool IsOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.y >= objectMatrix.GetLength(0) || position.y < 0 || position.x >= objectMatrix.GetLength(1);
        }

        // 更新地板
        private void UpdateFloor(Vector2Int position, int value)
        {
            floorMatrix[position.y, position.x] = value;
        }

        public int[,] GetObjectMatrix()
        {
            return (int[,])objectMatrix.Clone();
        }

        private List<IRewindable> allRewindables = new List<IRewindable>();

        public void RegisterRewindable(IRewindable obj)
        {
            if (!allRewindables.Contains(obj))
                allRewindables.Add(obj);
        }

        public void UnregisterRewindable(IRewindable obj)
        {
            if (allRewindables.Contains(obj))
                allRewindables.Remove(obj);
        }

        public void SaveState()
        {
            // 1. 複製地圖陣列
            int[,] floorCopy = (int[,])floorMatrix.Clone();
            int[,] objectCopy = (int[,])objectMatrix.Clone();

            // 2. 建立一個新的 GameState
            GameState currentState = new GameState
            {
                FloorMatrix = floorCopy,
                ObjectMatrix = objectCopy,
                PlayerPosition = playerMatrixPosition,
            };

            //掃描整個scene，要不是我們東西少不然可能有效能問題
            // 3. 收集所有 IRewindable 物件的狀態
            foreach (var r in rewindables)
            {
                RewindDataBase data = r.SaveData();
                string uid = r.GetUniqueId();
                currentState.RewindDataList.Add(new RewindDataGroup
                {
                    uniqueId = uid,
                    data = data
                });
            }

            // 4. 推進堆疊
            stateStack.Push(currentState);
            Debug.Log("GameManager: SaveState done. Stack size = " + stateStack.Count);
            Debug.Log($"PlayerM = {playerMatrixPosition}" + "Save");
        }

        public void LoadState()
        {
            // 沒有狀態可回溯就直接跳出
            if (stateStack.Count <= 0)
            {
                Debug.LogWarning("No state available to load!");
                return;
            }

            // 1. 彈出狀態
            GameState previousState = stateStack.Pop();


            // 2. 還原地圖矩陣
            floorMatrix = (int[,])previousState.FloorMatrix.Clone();
            objectMatrix = (int[,])previousState.ObjectMatrix.Clone();
            playerMatrixPosition = previousState.PlayerPosition;


            // Debug.Log("GameManager: LoadState - Restored matrix and player pos.");

            // 3. 還原所有 IRewindable 物件的狀態
            var rewindables = FindObjectsOfType<MonoBehaviour>().OfType<IRewindable>();
            foreach (var r in rewindables)
            {
                string uid = r.GetUniqueId();
                // 找到對應的 RewindDataGroup
                var match = previousState.RewindDataList.Find(g => g.uniqueId == uid);
                if (match != null)
                {
                    r.LoadData(match.data);
                }
                else
                {
                    // 可能剛生成或其他原因，不在先前的狀態裡
                    // 可視需求處理
                }
            }

            // Debug.Log("GameManager: LoadState done. Stack size = " + stateStack.Count);



        }

        public void ResetRoom()
        {
            stateStack.Clear();

            StartCoroutine(levelLoader.LoadLevel("SL-1"));
        }
    }

}
