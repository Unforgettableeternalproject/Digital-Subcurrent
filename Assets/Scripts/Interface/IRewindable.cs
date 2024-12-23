using UnityEngine;

namespace Digital_Subcurrent
{
    /// <summary>
    /// 所有需要被回溯的物件，都必須實作這個介面。
    /// </summary>
    public interface IRewindable
    {
        /// <summary>
        /// 將當前物件的狀態 (例如位置、旋轉、其他屬性) 存成 RewindData。
        /// </summary>
        RewindData SaveData();

        /// <summary>
        /// 從 RewindData 裡還原此物件狀態 (位置、旋轉、其他屬性)。
        /// </summary>
        void LoadData(RewindData data);

        /// <summary>
        /// 每個物件需要有一個唯一識別 (例如物件名或 GUID) 以對應到回溯資料。
        /// </summary>
        string GetUniqueId();
    }

    /// <summary>
    /// 用來儲存單一物件的回溯資訊
    /// </summary>
    [System.Serializable]
    public struct RewindData
    {
        public Vector3 position;
        
        // 你還可以加更多屬性，例如 生命值, 動畫狀態, 額外標記等等
        // public float health;
        // public bool isTriggered;
    }

    /// <summary>
    /// 對應一個物件 (藉由 uniqueId) 及其保存的 RewindData
    /// </summary>
    [System.Serializable]
    public class RewindDataGroup
    {
        public string uniqueId;
        public RewindData data;
    }
}

