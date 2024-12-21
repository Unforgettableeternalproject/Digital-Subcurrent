using UnityEngine;
using UnityEngine.Tilemaps;

namespace Digital_Subcurrent
{
    public class TilemapTagArray : MonoBehaviour
    {
        public Tilemap tilemap;
        private M2WConvertHelper helper = new M2WConvertHelper();

        // 二維陣列儲存每個有效格子上的 GameObject
        private GameObject[,] objectGrid;
        private GameObject[,] floorGrid;

        // 新的二維陣列儲存轉換後的數值
        private int[,] objectValueGrid;
        private int[,] floorValueGrid;

        // 用來記錄實際 Tilemap 的最小和最大格子座標
        private Vector3Int gridMin;
        private Vector3Int gridMax;

        void Start()
        {
            // 取得 Tilemap 組件
            // tilemap = GetComponent<Tilemap>();

            if (tilemap == null)
            {
                Debug.LogError("Tilemap 組件未找到！");
                return;
            }

            // 1. 計算實際上具有 Tile 的邊界範圍
            CalculateTileBounds();

            // 2. 初始化陣列並填充資料
            InitializeObjectGrid();

            // 3. 使用 M2WConvertHelper 將 GameObject 轉換為數值並存入新的二維陣列
            objectValueGrid = ConvertGridToValueGrid(objectGrid);
            floorValueGrid = ConvertGridToValueGrid(floorGrid);


            // Debug 列印GameObject內容
            PrintGrid(objectGrid);
            PrintGrid(floorGrid);

            //Debug 列印GameObject int內容
            PrintValueGrid(objectValueGrid);
            PrintValueGrid(floorValueGrid);
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

        void InitializeObjectGrid()
        {
            int width = gridMax.x - gridMin.x + 1;
            int height = gridMax.y - gridMin.y + 1;

            objectGrid = new GameObject[width, height];
            floorGrid = new GameObject[width, height];
            Debug.Log($"Initialized grid with size: {width} x {height}");

            for (int x = gridMin.x; x <= gridMax.x; x++)
            {
                for (int y = gridMin.y; y <= gridMax.y; y++)
                {
                    Vector3Int cellPosition = new Vector3Int(x, y, 0);

                    if (tilemap.HasTile(cellPosition))
                    {
                        // 將 Tilemap 格子座標轉換為世界座標
                        Vector3 worldPosition = tilemap.CellToWorld(cellPosition) + tilemap.tileAnchor;

                        int gridX = x - gridMin.x;
                        int gridY = y - gridMin.y;

                        // 第一次檢測 (objectLayerMask)
                        Collider2D objectCollider = Physics2D.OverlapPoint(worldPosition, LayerMask.GetMask("Object"));
                        if (objectCollider != null)
                        {
                            objectGrid[gridX, gridY] = objectCollider.gameObject;
                            Debug.Log($"Stored Object '{objectCollider.gameObject.name}' at Grid[{gridX}, {gridY}]");
                        }
                        else
                        {
                            objectGrid[gridX, gridY] = null;
                        }

                        // 第二次檢測 (floorLayerMask)
                        Collider2D floorCollider = Physics2D.OverlapPoint(worldPosition, LayerMask.GetMask("Floor"));
                        if (floorCollider != null)
                        {
                            floorGrid[gridX, gridY] = floorCollider.gameObject;
                            Debug.Log($"Stored Floor Object '{floorCollider.gameObject.name}' at Grid[{gridX}, {gridY}]");
                        }
                        else
                        {
                            floorGrid[gridX, gridY] = null;
                        }
                    }
                }
            }
        }

        int[,] ConvertGridToValueGrid(GameObject[,] inputGrid)
        {
            int width = inputGrid.GetLength(0);
            int height = inputGrid.GetLength(1);

            int[,] valueGrid = new int[width, height];

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    GameObject obj = inputGrid[x, y];
                    if (obj != null)
                    {
                        valueGrid[x, y] = helper.GetValue(obj);
                    }
                    else
                    {
                        valueGrid[x, y] = 0; // 預設值為 0
                    }
                }
            }

            Debug.Log("Input grid successfully converted to value grid.");
            return valueGrid;
        }

        void PrintGrid(GameObject[,] grid, string gridName = "Grid")
        {
            Debug.Log($"----- {gridName} -----");
            for (int y = grid.GetLength(1) - 1; y >= 0; y--) // 從上到下輸出
            {
                string row = "";
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    row += grid[x, y] != null ? $"[{grid[x, y].name}] " : "[null] ";
                }
                Debug.Log(row);
            }
        }

        void PrintValueGrid(int[,] grid, string gridName = "Value Grid")
        {
            Debug.Log($"----- {gridName} -----");
            for (int y = grid.GetLength(1) - 1; y >= 0; y--) // 從上到下輸出
            {
                string row = "";
                for (int x = 0; x < grid.GetLength(0); x++)
                {
                    row += $"[{grid[x, y]}] ";
                }
                Debug.Log(row);
            }
        }

    }
}
