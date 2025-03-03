using UnityEngine;
using System.Collections.Generic;

public class LevelDataLoader : MonoBehaviour
{
    public List<LevelData> LevelDataList;

    private static readonly int[] levelOrder = new int[]
    {
        1, 14, 7, 21, 39, 6, 47, 38, 25, 28, 5, 106, 12, 45, 49, 9, 34, 32, 103, 26, 46, 36, 11, 16, 33, 107, 43, 2, 4, 13, 22, 24, 101, 40, 3, 37, 27, 105, 51, 8, 35, 10, 48, 102, 17, 41, 23, 20, 18, 42, 104, 50, 15, 44
    };

    void Start()
    {
        FillLevelDataList();
    }

    void FillLevelDataList()
    {
        LevelDataList = new List<LevelData>();

        var n = 1;
        foreach (int levelNumber in levelOrder)
        {
            Texture2D levelTexture = Resources.Load<Texture2D>($"Levels/{levelNumber}");

            if (levelTexture != null)
            {
                LevelData levelData = new LevelData
                {
                    LevelNumber = (uint)n,
                    LevelTexture = levelTexture,
                    PaintColor = new Color(156f/255f, 4f/255f, 20f/255f),
                    WallColor = new Color(247f/255f, 235f/255f, 214f/255f)
                };

                LevelDataList.Add(levelData);
                n++;
            }
            else
            {
                Debug.LogWarning($"Texture for Level {levelNumber} not found at path: {$"Levels/{levelNumber}.png"}");
            }
        }

        Debug.Log("Level Data List populated with textures.");
    }
}
