using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelsData", menuName = "")]
public class LevelsData : ScriptableObject
{
    public List<LevelData> LevelDataList;
}
