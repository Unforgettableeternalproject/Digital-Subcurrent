using UnityEngine;
using System.Collections.Generic;

namespace Digital_Subcurrent
{
    public class GameState
    {
        public int[,] FloorMatrix { get; set; }
        public int[,] ObjectMatrix { get; set; }
        public Vector2Int PlayerPosition { get; set; }

        // 每個物件的回溯資料清單
        // 怕你看不懂，每個物件對應 (uniqueId, RewindDataBase)
        public List<RewindDataGroup> RewindDataList { get; set; }

        public GameState()
        {
            RewindDataList = new List<RewindDataGroup>();
        }
    }
}
