using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//when something get into the alta, make the runes glow
namespace Digital_Subcurrent
{
    public class HoleController : MonoBehaviour
    {
        public GameObject filledFloorPrefab; // 已填滿的地板Prefab

        private bool isFilled = false;

        // 當箱子進入時觸發
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isFilled && other.CompareTag("Box"))
            {
                Debug.Log("Box filled the hole!");
                // 切換為已填滿的地板
                Instantiate(filledFloorPrefab, transform.position, Quaternion.identity);
                Destroy(other.gameObject); // 移除箱子
                Destroy(gameObject); // 移除空洞
                isFilled = true;
            }
        }
    }
}
