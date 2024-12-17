using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using UnityEngine;
using UnityEngine.AI;

namespace Digital_Subcurrent
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f; // 每格移動速度
        public Vector2 gridSize = new Vector2(1, 1); // 格子大小


        private Animator animator;
        private bool isMoving = false;
        private Vector2 targetPosition;
        private CollisionHandler collisionHandler;

        private void Start()
        {
            animator = GetComponent<Animator>();
            collisionHandler = GetComponent<CollisionHandler>();
            targetPosition = transform.position; // 初始化為當前位置


            if (collisionHandler == null)
            {
                Debug.LogError("CollisionHandler is missing on this GameObject!");
            }
            else
            {
                Debug.Log("CollisionHandler found successfully.");
            }
        }

        private void Update()
        {
            if (isMoving)
            {
                MoveTowardsTarget();
                return;
            }

            Vector2 inputDir = GetInputDirection();

            if (inputDir != Vector2.zero)
            {
                TryMoveOrPush(inputDir);
            }
        }

        private Vector2 GetInputDirection()
        {
            Vector2 dir = Vector2.zero;
            if (Input.GetKey(KeyCode.A))
            {
                dir = Vector2.left;
                animator.SetInteger("Direction", 3); // 左
            }
            else if (Input.GetKey(KeyCode.D))
            {
                dir = Vector2.right;
                animator.SetInteger("Direction", 2); // 右
            }
            else if (Input.GetKey(KeyCode.W))
            {
                dir = Vector2.up;
                animator.SetInteger("Direction", 1); // 上
            }
            else if (Input.GetKey(KeyCode.S))
            {
                dir = Vector2.down;
                animator.SetInteger("Direction", 0); // 下
            }

            animator.SetBool("IsMoving", dir != Vector2.zero);
            return dir;
        }

        private void TryMoveOrPush(Vector2 direction)

        {
            //獲取碰撞物資訊
            Collider2D collidedObject = collisionHandler.getBlockInfo(transform.position, direction);
            
            Debug.Log (" return hit info " + collidedObject);

            if (collidedObject == null || collidedObject.CompareTag("Standable"))  
            {
                //移動
                targetPosition += direction * gridSize;
                isMoving = true;
            }

            else if (collidedObject.CompareTag("Wall") || collidedObject.CompareTag("Obstacle"))
            {
                Debug.Log("Blocked by wall or obstacle");
                return; // 如果目標格子被牆或障礙物阻擋
            }

            else if (collidedObject.CompareTag("Box"))
            {
                // 嘗試推動箱子
                Debug.Log("Try push box");

                BoxController box = collidedObject.GetComponent<BoxController>();
                if (box.TryMove(direction))
                {
                    isMoving = true;
                }

            }

        }

        private void MoveTowardsTarget()
        {
            // 平滑移動到目標格子
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 判斷是否到達目標格子
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // 對齊到目標位置
                isMoving = false; // 移動完成
            }
        }

    }
}
