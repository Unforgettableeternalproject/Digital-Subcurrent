using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent
{
    using UnityEngine;

    public class BoxController : MonoBehaviour
    {
        public Vector2 gridSize = new Vector2(1, 1); // ��l�j�p
        public float moveSpeed = 5f; // ���ʳt��

        private bool isMoving = false;
        private Vector2 targetPosition;

        private void Start()
        {
            targetPosition = transform.position; // ��l�ƥؼЦ�m
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
            if (isMoving) return false; // �p�G���b���ʡA�L�k�A����

            // �p��ؼЦ�m
            Vector2 potentialPosition = (Vector2)transform.position + direction * gridSize;

            
            if (CanMoveTo(potentialPosition))
            {
                targetPosition = potentialPosition;
                isMoving = true;
                return true; // ���ʦ��\
            }

            return false; // ���ʥ���
        }

        private bool CanMoveTo(Vector2 position)
        {
            // �ϥ� Raycast �ˬd�ؼЦ�m�O�_�i��
            RaycastHit2D hit = Physics2D.Raycast(position, Vector2.zero);
            return hit.collider == null; // �p�G�ؼЮ�l�S���I���A�h�i����
        }

        private void MoveTowardsTarget()
        {
            // ���Ʋ���
            transform.position = Vector2.MoveTowards(transform.position, targetPosition, moveSpeed * Time.deltaTime);

            // �P�_�O�_��F�ؼЦ�m
            if (Vector2.Distance(transform.position, targetPosition) < 0.01f)
            {
                transform.position = targetPosition; // ��T�����l
                isMoving = false;
            }
        }
    }

}
