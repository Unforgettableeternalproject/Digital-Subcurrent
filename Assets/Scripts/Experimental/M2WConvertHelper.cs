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

         // ���o GameObject �������ƭ�
        public int GetValue(GameObject obj)
        {
            try { 
                InitializeObjectToValueMap();
                if (obj == null)
                {
                    Debug.LogWarning("GameObject ���šA�^�ǹw�]�� 0");
                    return 0;
                }

                if (objectToValueMap.TryGetValue(obj.tag, out int value))
                {
                    return value;
                }
                else
                {
                    Debug.LogWarning($"GameObject '{obj?.tag ?? "null"}' �����������ƭȡA�^�ǹw�]�� 0");
                    return 0; // �w�]�^�� 0�A�p�G�䤣������� GameObject
                }
            }catch (Exception e){
                Debug.LogError(e);
                return 0;
            }
        }
        
    }
}