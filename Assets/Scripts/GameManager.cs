using System.Collections;
using UnityEngine;

namespace Digital_Subcurrent
{
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance;

        public int[,] floorMatrix;
        public int[,] objectMatrix;

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }

            InitializeMatrices();
        }

        private void InitializeMatrices() // ��l�Ʀa�� (�H�ثe���d�ҩж�����)
        {
            floorMatrix = new int[,]
            {
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

            objectMatrix = new int[,]
            {
            { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
            { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
            { -1, 0, 0, 0, 0, 1, 0, 0, 0, 0, -1 },
            { -1, 0, 0, 0, 0, 0, 0, 0, 0, 3, -1 },
            { -1, 0, 0, 0, 0, 0, 2, 0, 0, 0, -1 },
            { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
            { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
            { -1, 0, 0, 0, 0, 0, 0, 0, 0, 0, -1 },
            { -1, -1, -1, -1, -1, -1, -1, -1, -1, -1, -1 },
            };
        }
    }
}