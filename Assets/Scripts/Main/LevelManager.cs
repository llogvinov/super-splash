using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour
{
   public static Action Initialized;

   [SerializeField] private Transform plane;
   [SerializeField] private Transform map;
   [SerializeField] private LevelsData levelsData;

   [Header("Tiles Prefabs")]
   [SerializeField] private GameObject prefabWallTile;
   [SerializeField] private GameObject prefabRoadTile;

   public Color PaintColor { get; private set; }
   public List<RoadTile> RoadTilesList { get; private set; }
   public RoadTile DefaultBallRoadTile { get; private set; }
   public LevelData CurrentLevelData { get; private set; }

   private Color colorWall = Color.white;
   private Color colorRoad = Color.black;

   private float unitPerPixel;
   private Texture2D levelTexture;

   public void Initialize(uint levelNumber)
   {
      var levelData = levelsData.LevelDataList.FirstOrDefault(d => d.LevelNumber == levelNumber);
      CurrentLevelData = levelData;
      levelTexture = CurrentLevelData.LevelTexture;
      PaintColor = CurrentLevelData.PaintColor;
      Generate();
      Initialized?.Invoke();
   }

   private void Generate()
   {
      ClearMap();
      RoadTilesList = new List<RoadTile>();
      unitPerPixel = prefabWallTile.transform.lossyScale.x;
      var halfUnitPerPixel = unitPerPixel / 2f;
      var width = levelTexture.width;
      var height = levelTexture.height;
      plane.localScale = new Vector3(width * unitPerPixel / 10f, 1, height * unitPerPixel / 10f);

      var offset = (new Vector3(width / 2f, 0f, height / 2f) * unitPerPixel)
         - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

      for (var x = 0; x < width; x++)
      {
         for (var y = 0; y < height; y++)
         {
            var pixelColor = levelTexture.GetPixel(x, y);
            var spawnPos = (new Vector3(x, 0f, y) * unitPerPixel) - offset;
            if (pixelColor == colorWall)
               Spawn(prefabWallTile, spawnPos);
            else if (pixelColor == colorRoad)
               Spawn(prefabRoadTile, spawnPos);
         }
      }

      DefaultBallRoadTile = RoadTilesList[0];
   }

   private void ClearMap()
   {
      foreach (Transform child in map.transform)
      {
         Destroy(child.gameObject);
      }
   }

   private void Spawn(GameObject prefabTile, Vector3 position)
   {
      position.y = prefabTile.transform.position.y;

      var obj = Instantiate(prefabTile, position, Quaternion.identity, map);
      if (prefabTile == prefabRoadTile)
         RoadTilesList.Add(obj.GetComponent<RoadTile>());
   }
}
