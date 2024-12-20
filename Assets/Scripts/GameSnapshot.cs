using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class GameSnapshot
{
    public Vector2Int playerPosition;
    public string playerDirection;
    public List<Vector2Int> boxPositions;
    public List<bool> boxTriggers;
    public List<bool> keyStatus;
    public List<bool> terminalStatus;
    public Dictionary<Vector2Int, string> tileStates;
}

