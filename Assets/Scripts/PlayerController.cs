using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent
{
    public class PlayerController : MonoBehaviour
    {
        public float moveSpeed = 5f; // 每格移動速度
        public Vector2 gridSize = new Vector2(1, 1); // 格子大小
        private bool isMoving = false;
        private Vector2 initPosition = new Vector2(0, 0);
        private Vector2 targetPosition;
        private bool canMove = true;
        private bool hasKey = false; // 玩家是否擁有鑰匙

        private Animator animator;
        private GameManager gameManager;

        private void Start()
        {
            animator = GetComponent<Animator>();
            gameManager = GameManager.Instance;
            if (gameManager == null)
            {
                Debug.LogError("GameManager is missing in the scene!");
            }

            initPosition = transform.position; // 初始化為當前位置
        }

        private void Update()
        {
            if (isMoving)
            {
                MoveTowardsTarget();
                return;
            }

            if (canMove)
            {
                Vector2 inputDir = HandleMovement();

                if (inputDir != Vector2.zero)
                {
                    TryMove(inputDir);
                }
            }
        }

        // 處理玩家移動輸入
        private Vector2 HandleMovement()
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

        private void TryMove(Vector2 direction)
        {
            Vector2 playerPosition = transform.position;
            if (gameManager.HasBox(new Vector2(direction.x, -direction.y)))
            {
                BoxController box = gameManager.GetBox(playerPosition + direction * gridSize);
                if (box != null)
                {
                    Debug.Log("Has box");
                    if (box.TryMove(direction))
                    {
                        Debug.Log("Push box success");
                        canMove = false;
                        gameManager.UpdateBox(new Vector2(direction.x, -direction.y));
                        StartCoroutine(WaitForNextMove());
                    }
                }
                return;
            }

            if (gameManager.PlayerTryMove(new Vector2(direction.x, -direction.y)))
            {
                targetPosition = playerPosition + direction * gridSize;
                isMoving = true;
                canMove = false;
                if (gameManager.HasKey(new Vector2(direction.x, -direction.y)))
                {
                    KeyController key = gameManager.GetKey(playerPosition + direction * gridSize);
                    if (key != null)
                    {
                        hasKey = true;
                        key.OnCollect();
                    }
                }
                gameManager.UpdatePlayer(new Vector2(direction.x, -direction.y));
            }

            // 打印當前的物件矩陣狀態
            PrintObjectMatrix();
        }

        // 平滑移動到目標格子
        private void MoveTowardsTarget()
        {
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // 判斷是否到達目標格子
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // 對齊到目標位置
                isMoving = false; // 移動完成
                StartCoroutine(WaitForNextMove());
            }
        }

        // 等待玩家放開按鍵後才能再次移動
        private IEnumerator WaitForNextMove()
        {
            yield return new WaitUntil(() => Input.GetAxisRaw("Horizontal") == 0 && Input.GetAxisRaw("Vertical") == 0);
            canMove = true;
        }

        // 打印當前的物件矩陣狀態
        private void PrintObjectMatrix()
        {
            int[,] objectMatrix = GameManager.Instance.GetObjectMatrix();
            string matrixString = "";

            for (int x = 0; x < objectMatrix.GetLength(0); x++)
            {
                for (int y = 0; y < objectMatrix.GetLength(1); y++)
                {
                    matrixString += objectMatrix[x, y] + " ";
                }
                matrixString += "\n";
            }
            Debug.Log("Current Object Matrix:\n" + matrixString);
        }

        public bool HasKey()
        {
            return hasKey;
        }
        //private Vector2 GetInputDirection()
        //{
        //    Vector2 dir = Vector2.zero;
        //    if (Input.GetKey(KeyCode.A))
        //    {
        //        dir = Vector2.left;
        //        animator.SetInteger("Direction", 3); // 左
        //    }
        //    else if (Input.GetKey(KeyCode.D))
        //    {
        //        dir = Vector2.right;
        //        animator.SetInteger("Direction", 2); // 右
        //    }
        //    else if (Input.GetKey(KeyCode.W))
        //    {
        //        dir = Vector2.up;
        //        animator.SetInteger("Direction", 1); // 上
        //    }
        //    else if (Input.GetKey(KeyCode.S))
        //    {
        //        dir = Vector2.down;
        //        animator.SetInteger("Direction", 0); // 下
        //    }

        //    animator.SetBool("IsMoving", dir != Vector2.zero);
        //    return dir;
        //}

        //private void TryMove(Vector2 direction)
        //{
        //    Vector2Int playerGridPosition = gameManager.WorldToGrid(transform.position);
        //    Vector2Int targetGridPosition = playerGridPosition + Vector2Int.RoundToInt(direction);

        //    // 使用GameManager來檢查移動是否合法
        //    if (gameManager.CanPlayerMoveTo(targetGridPosition))
        //    {
        //        gameManager.UpdatePlayerPosition(playerGridPosition, targetGridPosition);
        //        targetPosition = gameManager.GridToWorld(targetGridPosition);
        //        isMoving = true;
        //    }
        //    else if (gameManager.IsBoxAtPosition(targetGridPosition))
        //    {
        //        // 嘗試推箱子
        //        Vector2Int boxNewPosition = targetGridPosition + Vector2Int.RoundToInt(direction);
        //        if (gameManager.TryMoveBox(targetGridPosition, boxNewPosition))
        //        {
        //            gameManager.UpdatePlayerPosition(playerGridPosition, targetGridPosition);
        //            targetPosition = gameManager.GridToWorld(targetGridPosition);
        //            isMoving = true;
        //        }
        //    }
        //}

        //private void MoveTowardsTarget()
        //{
        //    // 平滑移動到目標格子
        //    transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

        //    // 判斷是否到達目標格子
        //    if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
        //    {
        //        transform.position = targetPosition; // 對齊到目標位置
        //        isMoving = false; // 移動完成
        //    }
        //}
    }
}
