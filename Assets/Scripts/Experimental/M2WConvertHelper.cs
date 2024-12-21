using UnityEngine;
using System.Collections.Generic;
using System;

namespace Digital_Subcurrent
{
    public class M2WConvertHelper
    { 
        private Dictionary<string, int> objectToValueMap;

        private void InitializeObjectToValueMap()
        {
            objectToValueMap = new Dictionary<string, int>
            {
                { "Player", 1 },
                { "Box", 2 },
                { "Key", 3 },
                { "Obstacle", -1 },
                { "Exit", -2 },
                { "Hole", 1 },
                { "Glass", 2 }
            };
        }

         // 取得 GameObject 對應的數值
        public int GetValue(GameObject obj)
        {
            try { 
                InitializeObjectToValueMap();
                if (obj == null)
                {
                    Debug.LogWarning("GameObject 為空，回傳預設值 0");
                    return 0;
                }

                if (objectToValueMap.TryGetValue(obj.tag, out int value))
                {
                    return value;
                }
                else
                {
                    Debug.LogWarning($"GameObject '{obj?.tag ?? "null"}' 未找到對應的數值，回傳預設值 0");
                    return 0; // 預設回傳 0，如果找不到對應的 GameObject
                }
            }catch (Exception e){
                Debug.LogError(e);
                return 0;
            }
        }
        
    }
}