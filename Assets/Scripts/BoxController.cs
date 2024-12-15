using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent
{
    using UnityEngine;

    public class BoxController : MonoBehaviour
    {
        public Vector2 gridSize = new Vector2(1, 1); // 格子大小
        public float moveSpeed = 5f; // 移動速度

        private bool isMoving = false;
        private Vector2 targetPosition;

        private void Start()
        {
            targetPosition = transform.position; // 初始化目標位置
        }

        private void Update()
        {
            if (isMoving)
            {
                MoveTowardsTarget();
            }
        }

        public bool TryMove(Vector2 direction)
        {
            if (isMoving) return false; // 如果正在移動，無法再推動

            // 計算目標位置
            Vector2 potentialPosition = (Vector2)transform.position + direction * gridSize;

            // 檢查目標位置是否有效（可根據具體邏輯擴展檢查條件）
            if (CanMoveTo(potentialPosition))
            {
                targetPosition = potentialPosition;
                isMoving = true;
                return true; // 推動成功
            }

            return false; // 推動失敗
        }

        private bool CanMoveTo(Vector2 position)
        {
            // 使用 Raycast 檢查目標位置是否可用
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
            return hit.collider == null; // 如果目標格子沒有碰撞，則可移動
        }

        private void MoveTowardsTarget()
        {
            // 平滑移動
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 判斷是否到達目標位置
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // 精確對齊格子
                isMoving = false;
            }
        }
    }

}
