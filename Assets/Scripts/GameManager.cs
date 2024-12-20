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
        public bool TryMove(Vector2 direction)
        {
            Vector2Int targetPosition = playerMatrixPosition + Vector2Int.RoundToInt(direction);
            if(objectMatrix[targetPosition.x, targetPosition.y] == -1 || floorMatrix[targetPosition.x, targetPosition.y] == 1)
            {
                return false;
            }
            return true;
        }

        // 更新矩陣
        public void UpdateMatrix(Vector2Int original)
        {
            objectMatrix[original.y, original.x] = 0;
            objectMatrix[playerMatrixPosition.y, playerMatrixPosition.x] = 1;
            Debug.Log($"PlayerM = {playerMatrixPosition}");
        }

        public void UpdatePlayer(Vector2 direction)
        {
            Vector2Int original = playerMatrixPosition;
            playerMatrixPosition += Vector2Int.RoundToInt(direction);
            UpdateMatrix(original);
        }

        // 檢查邊界
        private bool IsOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.x >= objectMatrix.GetLength(0) || position.y < 0 || position.y >= objectMatrix.GetLength(1);
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
