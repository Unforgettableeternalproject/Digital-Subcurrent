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
        RewindDataBase SaveData();

        /// <summary>
        /// 從 RewindData 裡還原此物件狀態 (位置、旋轉、其他屬性)。
        /// </summary>
        void LoadData(RewindDataBase data);

        /// <summary>
        /// 每個物件需要有一個唯一識別 (例如物件名或 GUID) 以對應到回溯資料。
        /// </summary>
        string GetUniqueId();
    }

    
    /// <summary>
    /// 對應一個物件 (藉由 uniqueId) 及其保存的 RewindData
    /// </summary>
    [System.Serializable]
    public class RewindDataGroup
    {
        public string uniqueId;
        public RewindDataBase data;
    }
}

