using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace Digital_Subcurrent
{
    public class GameManager : MonoBehaviour
    {
        private Stack<GameSnapshot> stepStack = new Stack<GameSnapshot>();

        public static GameManager Instance { get; private set; }

        private void Awake()
        {
            // 確保 Singleton 模式
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            DontDestroyOnLoad(gameObject); // 保持跨場景持續
        }

        // 快照保存邏輯
        public void SaveSnapshot(GameSnapshot snapshot)
        {
            stepStack.Push(snapshot);
        }

        // 快照還原邏輯
        public GameSnapshot Undo()
        {
            if (stepStack.Count > 1)
            {
                stepStack.Pop();
                return stepStack.Peek();
            }
            return null;
        }

        public GameSnapshot ResetLevel()
        {
            return stepStack.Count > 0 ? stepStack.First() : null;
        }
    }
}
