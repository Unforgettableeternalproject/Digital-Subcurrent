using UnityEngine;
using UnityEngine.Tilemaps;

namespace Digital_Subcurrent
{
    public class ShowTilesCenterPosition : MonoBehaviour
    {
        public Tilemap tilemap; // 在 Inspector 中拖入你的 Tilemap

        void OnDrawGizmos()
        {
            if (tilemap == null) return;

            // 設置調試顏色
            Gizmos.color = Color.blue;

            // 使用 tilemap.cellBounds 取得 Tilemap 的邊界範圍
            BoundsInt bounds = tilemap.cellBounds;

            // 遍歷 Tilemap 範圍內的每一個格子
            foreach (Vector3Int position in bounds.allPositionsWithin)
            {
                TileBase tile = tilemap.GetTile(position);

                if (tile != null) // 如果格子上有 Tile
                {
                    Vector3 worldPosition = tilemap.GetCellCenterWorld(position);
                    Gizmos.DrawSphere(worldPosition, 0.1f); // 在每個 Tile 的中心繪製一個藍色小圓球
                }
            }
        }
    }
}
