using System.Collections.Generic;
using UnityEngine;

namespace Digital_Subcurrent
{
    public class TagValueMapping
    {
        // 字典用來對應 Tag 到整數值
        private Dictionary<string, int> tagToValueMap;

        // 建構子：初始化對照表
        public TagValueMapping()
        {
            tagToValueMap = new Dictionary<string, int>
        {
            { "Hole", 1 },
            { "Box", 2 },
            { "Key", 3 }
        };
        }

        // 取得 Tag 對應的數值
        public int GetValue(string tag)
        {
            if (tagToValueMap.TryGetValue(tag, out int value))
            {
                return value;
            }
            else
            {
                Debug.LogWarning($"Tag '{tag}' 未找到，回傳預設值 0");
                return 0; // 預設回傳 0，如果找不到對應的 Tag
            }
        }


    }
}

