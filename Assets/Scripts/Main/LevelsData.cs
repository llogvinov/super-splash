using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "")]
public class LevelsData : ScriptableObject
{
    public List<LevelData> LevelDataList;
    public uint MaxLevel => (uint)LevelDataList.Count;
}
