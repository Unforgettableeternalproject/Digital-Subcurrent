using System.Collections;
using UnityEngine;

namespace Digital_Subcurrent
{
    public class KeyController : MonoBehaviour
    {
        public float floatAmplitude = 0.02f; // 上下漂浮的幅度
        public float floatSpeed = 1.2f;       // 漂浮的速度
        private Vector3 startPos;

        private void Start()
        {
            startPos = transform.position;
        }

        private void Update()
        {
            // 使用 Sin 函數讓鑰匙上下浮動
            float newY = startPos.y + Mathf.Sin(Time.time * floatSpeed) * floatAmplitude;
            transform.position = new Vector3(startPos.x, newY, startPos.z);
        }
    }
}
