using UnityEngine;
using UnityEngine.Tilemaps;

namespace Digital_Subcurrent
{


    public class TilemapValueArray : MonoBehaviour
    {
        private Tilemap tilemap;

        // 二維陣列儲存每個有效格子上的 Tag 對應的整數值
        private int[,] valueGrid;

        // 用來記錄實際 Tilemap 的最小和最大格子座標
        private Vector3Int gridMin;
        private Vector3Int gridMax;

        // Tag 對應的數值映射類別
        private TagValueMapping tagMapping;

        void Start()
        {
            // 取得 Tilemap 組件
            tilemap = GetComponent<Tilemap>();

            if (tilemap == null)
            {
                Debug.LogError("Tilemap 組件未找到！");
                return;
            }

            // 初始化 Tag 映射表
            tagMapping = new TagValueMapping();

            // 1. 計算實際上具有 Tile 的邊界範圍
            CalculateTileBounds();

            // 2. 初始化陣列並填充資料
            InitializeValueGrid();

            // 3. 列印陣列內容
            PrintValueGrid();
        }

        void CalculateTileBounds()
        {
            // 遍歷所有 Tilemap 格子範圍，找出具有 Tile 的最小和最大座標
            BoundsInt bounds = tilemap.cellBounds;

            gridMin = new Vector3Int(int.MaxValue, int.MaxValue, 0);
            gridMax = new Vector3Int(int.MinValue, int.MinValue, 0);

            foreach (Vector3Int position in bounds.allPositionsWithin)
            {
                if (tilemap.HasTile(position))
                {
                    gridMin = Vector3Int.Min(gridMin, position);
                    gridMax = Vector3Int.Max(gridMax, position);
                }
            }

            Debug.Log($"Tile Bounds Calculated - Min: {gridMin}, Max: {gridMax}");
        }

        void InitializeValueGrid()
        {
            // 計算二維陣列的大小
            int width = gridMax.x - gridMin.x + 1;
            int height = gridMax.y - gridMin.y + 1;

            valueGrid = new int[width, height];
            Debug.Log($"Initialized grid with size: {width} x {height}");

            // 遍歷具有 Tile 的位置並填充對應的數值
            for (int x = gridMin.x; x <= gridMax.x; x++)
            {
                for (int y = gridMin.y; y <= gridMax.y; y++)
                {
                    Vector3Int position = new Vector3Int(x, y, 0);

                    if (tilemap.HasTile(position))
                    {
                        // 轉換格子座標到世界座標
                        Vector3 worldPosition = tilemap.CellToWorld(position);
                        worldPosition += tilemap.tileAnchor;

                        // 使用 Physics2D 檢查該位置上的 GameObject
                        Collider2D collider = Physics2D.OverlapPoint(worldPosition);

                        // 計算二維陣列的索引
                        int gridX = x - gridMin.x;
                        int gridY = y - gridMin.y;

                        if (collider != null)
                        {
                            // 將 Tag 轉換為數值並存入陣列
                            string tag = collider.gameObject.tag;
                            valueGrid[gridX, gridY] = tagMapping.GetValue(tag);

                            Debug.Log($"Stored Value '{valueGrid[gridX, gridY]}' for Tag '{tag}' at Grid[{gridX}, {gridY}]");
                        }
                        else
                        {
                            // 若沒有物件，預設值設為 0
                            valueGrid[gridX, gridY] = 0;
                        }
                    }
                }
            }
        }

        void PrintValueGrid()
        {
            for (int y = valueGrid.GetLength(1) - 1; y >= 0; y--) // 從上到下輸出
            {
                string row = "";
                for (int x = 0; x < valueGrid.GetLength(0); x++)
                {
                    row += $"[{valueGrid[x, y]}] ";
                }
                Debug.Log(row);
            }
        }
    }
}