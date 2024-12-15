using UnityEngine;

namespace Digital_Subcurrent
{

    public class ShowObjectCenter : MonoBehaviour
    {
        void OnDrawGizmos()
        {
            // 設置調試顏色
            Gizmos.color = Color.red;

            // 在當前物件的位置上繪製一個小圓球
            Vector3 center = transform.position;
            
            Gizmos.DrawSphere(center, 0.1f);
        }
    }
}