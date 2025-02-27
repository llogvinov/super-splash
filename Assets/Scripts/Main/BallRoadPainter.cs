using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;
using Core;

public class BallRoadPainter : MonoBehaviour
{
   [SerializeField] private LevelManager levelManager;
   [SerializeField] private BallMovement ballMovement;
   [SerializeField] private MeshRenderer ballMeshRenderer;

   private int paintedRoadTiles = 0;

   private void Start()
   {
      LevelManager.Initialized += OnInitialized;
      ballMovement.onMoveStart += OnBallMoveStartHandler;
   }

   private void OnDestroy()
   {
      LevelManager.Initialized -= OnInitialized;
      ballMovement.onMoveStart -= OnBallMoveStartHandler;
   }

   private void OnInitialized()
   {
      Debug.Log("initialized");
      paintedRoadTiles = 0;
      ballMeshRenderer.material.color = levelManager.PaintColor;
      Paint(levelManager.DefaultBallRoadTile, 0.5f, 0f);
   }

   private void OnBallMoveStartHandler(List<RoadTile> roadTiles, float totalDuration)
   {
      float stepDuration = totalDuration / roadTiles.Count;
      for (int i = 0; i < roadTiles.Count; i++)
      {
         RoadTile roadTile = roadTiles[i];
         if (!roadTile.isPainted)
         {
            float duration = totalDuration / 2f;
            float delay = i * (stepDuration / 2f);
            Paint(roadTile, duration, delay);

            if (paintedRoadTiles == levelManager.RoadTilesList.Count)
            {
               Debug.Log("Level Completed");
               Game.LevelCompleted?.Invoke();
            }
         }
      }
   }

   private void Paint(RoadTile roadTile, float duration, float delay)
   {
      roadTile.transform
         .DOMoveY(roadTile.transform.position.y + 0.15f, 0.15f)
         .SetLoops(2, LoopType.Yoyo)
         .SetDelay(delay);

      roadTile.meshRenderer.material
         .DOColor(levelManager.PaintColor, duration)
         .SetDelay(delay);

      roadTile.isPainted = true;
      paintedRoadTiles++;
   }
}
