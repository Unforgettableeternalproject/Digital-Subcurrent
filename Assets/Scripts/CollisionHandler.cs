using System.Collections.Generic;
using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.Rendering;

namespace Digital_Subcurrent
{
    public class CollisionHandler : MonoBehaviour
    {
        [SerializeField]
        private Vector2 gridsize = new Vector2(1f, 1f);

        // 檢測特定位置碰撞資訊
        public Collider2D getBlockInfo(Vector2 position)
        {
            Collider2D hit = Physics2D.OverlapPoint(position);

            return hit; // 回傳碰撞資訊
        }

        // 檢測目標物移動方向碰撞資訊 (1格)
        public Collider2D getBlockInfo(Vector2 currentPosition, Vector2 direction)
        {
            Collider2D hit = Physics2D.OverlapPoint(currentPosition + direction * gridsize);

            return hit; // 回傳碰撞資訊
        }

        // 檢測目標物移動方向碰撞資訊 (自訂距離)
        public Collider2D getBlockInfo(Vector2 currentPosition, Vector2 direction, float distance)
        {
            Collider2D hit = Physics2D.OverlapPoint(currentPosition + direction * gridsize * distance);

            return hit; // 回傳碰撞資訊
        }

        // 檢測該碰撞物是否為正確標的物(預設已經有碰撞，非null)
        public bool IsTagMatched(Collider2D collidedObject, List<string> tags)
        {
            // 遍歷標籤列表，判斷碰撞物的 tag 是否匹配
            foreach (string tag in tags)
            {
                if (collidedObject.CompareTag(tag))
                {
                    return true; // 如果有匹配的標籤，返回 true
                }
            }

            // 若沒有匹配的標籤，返回 false
            return false;

        }



    }
}