using UnityEngine;

namespace Digital_Subcurrent
{
    /// <summary>
    /// 所有回溯資料的基底類別
    /// 可放通用屬性 (如位置、旋轉等)
    /// </summary>
    [System.Serializable]
    public class RewindDataBase
    {
        public Vector3 position;

        // 若需要，可加 public Quaternion rotation;
        // 或 public bool isActive 之類的隨便拉
    }

    
    public class PlayerRewindData : RewindDataBase
    {
        public bool hasKey;

    }

    public class BoxRewindData : RewindDataBase
    {
        public bool isActive;
    }

    public class GlassFloorRewindData : RewindDataBase 
    {
        public bool isHovered;
    }

    public class HoleRewindData : RewindDataBase
    {
        public bool isFilled;
        public bool isActive;
    }


}

