using UnityEngine;
using UnityEngine.Rendering;

namespace Digital_Subcurrent
{
    public class CollisionHandler : MonoBehaviour
    {
        [SerializeField]
        private Vector2 gridsize = new Vector2(1f, 1f);

        // 檢測目標物移動方向的碰撞 (1格)
        public Collider2D getBlockInfo(Vector2 currentPosition, Vector2 direction)
        {
            Collider2D hit = Physics2D.OverlapPoint(currentPosition + direction * gridsize);

            return hit; // 回傳碰撞資訊
        }

        // 檢測目標物移動方向的碰撞 (自訂距離)
        public Collider2D getBlockInfo(Vector2 currentPosition, Vector2 direction, float distance)
        {
            Collider2D hit = Physics2D.OverlapPoint(currentPosition + direction * gridsize * distance);

            return hit; // 回傳碰撞資訊
        }



    }
}