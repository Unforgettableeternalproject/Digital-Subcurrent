using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent
{
    public class CharacterController : MonoBehaviour
    {
        public float moveSpeed = 5f; // 每格移動速度
        public Vector2 gridSize = new Vector2(1, 1); // 格子大小
        //public Vector2 test = new Vector2(0.5f,0.5f);

        private Animator animator;
        private bool isMoving = false;
        private Vector2 targetPosition;
        private bool hasKey = false; // 玩家是否擁有鑰匙

        private void Start()
        {
            animator = GetComponent<Animator>();
            targetPosition = transform.position; // 初始化為當前位置
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

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Key"))
            {
                Debug.Log("Box filled the hole!");
                hasKey = true;

                // 創建一個臨時音效物件
                GameObject audioPlayer = new GameObject("TempAudioPlayer");
                AudioSource tempAudio = audioPlayer.AddComponent<AudioSource>();
                tempAudio.clip = other.gameObject.GetComponent<AudioSource>().clip;
                tempAudio.volume = 0.6f;
                tempAudio.Play();

                // 自動銷毀音效物件
                Destroy(audioPlayer, tempAudio.clip.length);

                // 移除鑰匙物件
                Destroy(other.gameObject, 0.1f);
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
            // 計算目標位置
            Vector2 playerPosition = transform.position;
            Vector2 targetTestGrid = playerPosition + direction * new Vector2(0.5f,0.5f);
            Vector2 targetGrid = playerPosition + direction * gridSize;

            if (IsBlocked(targetGrid) || IsHole(targetGrid))
            {
                Debug.Log("Blocked by wall or obstacle");
                return; // 如果目標格子被牆或障礙物阻擋
            }

            RaycastHit2D hit = Physics2D.Raycast(targetGrid, Vector2.zero);

            if (hit.collider != null && hit.collider.CompareTag("Box"))
            {
                Debug.Log("Try push box");
                // 嘗試推動箱子
                Vector2 boxTargetGrid = targetGrid + direction * gridSize;
                if (!IsBlocked(boxTargetGrid) || IsHole(boxTargetGrid)) // 檢查箱子目標位置是否被阻擋
                {
                    Debug.Log("Box can be pushed");
                    BoxController box = hit.collider.GetComponent<BoxController>();
                    if (box != null && box.TryMove(direction))
                    {
                        Debug.Log("Push box success");
                        AudioSource pushAudio = hit.collider.GetComponent<AudioSource>();
                        if (pushAudio != null)
                        {
                            pushAudio.Play();
                        }
                        isMoving = true;
                    }
                }
                else
                {
                    Debug.Log("Box push blocked");
                }
            }
            else // 如果目標格子是空的
            {
                targetPosition = targetGrid;
                isMoving = true;
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

        private bool IsBlocked(Vector2 position)
        {
            // 使用圓形檢測，以更準確地檢查位置是否有障礙物
            Collider2D hit = Physics2D.OverlapCircle(position, 0.1f);
            
            if (hit != null && (hit.CompareTag("Wall") || (hit.CompareTag("Obstacle"))))
            {
                Debug.Log(" hit " + hit.gameObject.tag + " position " + position);
                return true; // 如果碰到牆或其他障礙物
            }
            return false;
        }

        private bool IsHole(Vector2 position)
        {
            Collider2D hit = Physics2D.OverlapCircle(position, 0.1f);
            if (hit != null && hit.CompareTag("Hole"))
            {
                return true;
            }
            return false;
        }

        public bool HasKey()
        {
            return hasKey;
        }
    }
}
