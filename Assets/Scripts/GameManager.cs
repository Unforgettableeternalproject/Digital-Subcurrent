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
        private Vector2 gridSize = new Vector2(1, 1); // �C�檺�@�ɮy�Фj�p

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

        // ��l�Ưx�}
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

        // ��쪱�a�b�x�}������m
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

        // �ˬd���a�O�_�i�H���ʨ�ؼЦ�m
        public bool TryMove(Vector2 direction)
        {
            Vector2Int targetPosition = playerMatrixPosition + Vector2Int.RoundToInt(direction);
            if(objectMatrix[targetPosition.x, targetPosition.y] == -1 || floorMatrix[targetPosition.x, targetPosition.y] == 1)
            {
                return false;
            }
            return true;
        }

        // ��s�x�}
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

        // �ˬd���
        private bool IsOutOfBounds(Vector2Int position)
        {
            return position.x < 0 || position.x >= objectMatrix.GetLength(0) || position.y < 0 || position.y >= objectMatrix.GetLength(1);
        }

        // ��s�a�O
        private void UpdateFloor(Vector2Int position)
        {

        }

        // ���ѰT�������a
        public string GetMessage(Vector2Int targetPosition)
        {
            if (IsOutOfBounds(targetPosition)) return "�L�k���ʡG�W�X��ɡI";

            int objectValue = objectMatrix[targetPosition.x, targetPosition.y];
            switch (objectValue)
            {
                case -1:
                    return "�L�k���ʡG������I";
                case 2:
                    return "�c�l���סA���ձ��ʥ��I";
                default:
                    return "�i�H���ʡC";
            }
        }

        public int[,] GetObjectMatrix()
        {
            return (int[,])objectMatrix.Clone();
        }

    }
}
