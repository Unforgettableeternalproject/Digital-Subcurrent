using System;
using UnityEngine;

namespace Digital_Subcurrent
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance; // Singleton

        private int[,] objectMatrix;
        private int[,] floorMatrix;
        private Vector2 offset = new Vector2(0, 0);
        private Vector2Int playerMatrixPosition;
        private Vector2Int tempBoxMPosition;
        private Vector2 gridSize = new Vector2(1, 1); // 每格的世界座標大小

        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Destroy(gameObject);
            }
        }

        // 初始化矩陣
        public void InitializeGame(Vector2 position)
        {
            objectMatrix = new int[,] { 
                { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
                { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
                { -1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -1 },
                { -1, 0, 0, 0, 0, 0, 0, 0, 0, 3, -1 },
                { -1, 0, 0, 0, 0, 0, 2, 0, 0, 0, -1 },
                { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
                { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
                { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
                { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 } 
            };
            floorMatrix = new int[,] { 
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
                { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } 
            };
            playerMatrixPosition = FindPlayerPosition(objectMatrix);
            Debug.Log($"PlayerM = {playerMatrixPosition}");
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
            if(IsOutOfBounds(targetPosition) || objectMatrix[targetPosition.y, targetPosition.x] == -1 || floorMatrix[targetPosition.y, targetPosition.x] == 1)
            {
                return false;
            }
            Debug.Log($"objectMatric[{targetPosition.x}, {targetPosition.y}] = {objectMatrix[targetPosition.y, targetPosition.x]}");
            return true;
        }

        public bool BoxTryMove(Vector2 direction)
        {
            if (tempBoxMPosition.x == -1 || tempBoxMPosition.y == -1) return false;

            Vector2Int targetPosition = tempBoxMPosition + Vector2Int.RoundToInt(direction);
            if (IsOutOfBounds(targetPosition) || objectMatrix[targetPosition.y, targetPosition.x] == -1)
            {
                return false;
            }
            return true;
        }

        public bool HasBox(Vector2 direction)
        {
            Vector2Int targetPosition = playerMatrixPosition + Vector2Int.RoundToInt(direction);
            if (objectMatrix[targetPosition.y, targetPosition.x] == 2) {
                tempBoxMPosition = targetPosition;
                return true;
            }else
            {   
                tempBoxMPosition = new Vector2Int(-1, -1);
                return false;
            }
        }

        public BoxController GetBox(Vector2 worldPosition)
        {
            if (tempBoxMPosition.x == -1 || tempBoxMPosition.y == -1) return null;
            Debug.Log("1");
            if (objectMatrix[tempBoxMPosition.y, tempBoxMPosition.x] == 2)
            {
                Debug.Log("2");
                // 在該位置檢測是否有碰撞的物件 (假設箱子有 "Box" Layer 或 Tag)
                Collider2D hit = Physics2D.OverlapPoint(worldPosition);
                if (hit != null && hit.CompareTag("Box")) // 使用 Tag 過濾箱子
                {
                    Debug.Log("3");
                    // 返回該物件的 BoxController
                    return hit.GetComponent<BoxController>();
                }
                return null; // 若無法檢測到箱子，返回 null
            }
            return null; // 如果沒有找到箱子，返回 null
        }

        // 更新矩陣
        public void UpdateMatrix(Vector2Int original, Vector2Int updated, int value)
        {
            objectMatrix[original.y, original.x] = 0;
            objectMatrix[updated.y, updated.x] = value;
            Debug.Log($"PlayerM = {playerMatrixPosition}");
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

        // 檢查邊界
        private bool IsOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.y >= objectMatrix.GetLength(0) || position.y < 0 || position.x >= objectMatrix.GetLength(1);
        }

        // 更新地板
        private void UpdateFloor(Vector2Int position)
        {

        }

        // 提供訊息給玩家
        public string GetMessage(Vector2Int targetPosition)
        {
            if (IsOutOfBounds(targetPosition)) return "無法移動：超出邊界！";

            int objectValue = objectMatrix[targetPosition.x, targetPosition.y];
            switch (objectValue)
            {
                case -1:
                    return "無法移動：有牆壁！";
                case 2:
                    return "箱子阻擋，嘗試推動它！";
                default:
                    return "可以移動。";
            }
        }

        public int[,] GetObjectMatrix()
        {
            return (int[,])objectMatrix.Clone();
        }

    }
}
