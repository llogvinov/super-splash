using System.Collections.Generic;
using Core;
using DG.Tweening;
using UnityEngine;

namespace Main
{
    public class BallPaintController : MonoBehaviour
    {
        [SerializeField] private LevelManager levelManager;
        [SerializeField] private BallMoveController ballMovement;
        [SerializeField] private MeshRenderer ballMeshRenderer;

        private int paintedRoadTiles = 0;
        private Sequence _sequence;

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
            paintedRoadTiles = 0;
            ballMeshRenderer.material.color = levelManager.PaintColor;
            PaintInitial(levelManager.DefaultBallRoadTile);
        }

        private void OnBallMoveStartHandler(List<RoadTile> roadTiles, float totalDuration)
        {
            float stepDuration = totalDuration / roadTiles.Count;
            for (int i = 0; i < roadTiles.Count; i++)
            {
                RoadTile roadTile = roadTiles[i];
                if (!roadTile.isPainted)
                {
                    float delay = i * stepDuration;
                    Paint(roadTile, delay);
                }
            }
        }

        private void PaintInitial(RoadTile roadTile)
        {
            roadTile.meshRenderer.material.DOColor(levelManager.PaintColor, 0f);
            roadTile.isPainted = true;
            paintedRoadTiles++;
        }

        private void Paint(RoadTile roadTile, float delay)
        {
            roadTile.isPainted = true;
            _sequence = DOTween.Sequence();
            _sequence
                .AppendInterval(delay)
                .Append(roadTile.meshRenderer.material.DOColor(levelManager.PaintColor, 0.02f))
                .Append(roadTile.transform.DOMoveY(roadTile.transform.position.y + 0.15f, 0.15f).SetLoops(2, LoopType.Yoyo))
                .OnComplete(() =>
                {
                    paintedRoadTiles++;
                    CheckAllTilesPainted();
                });
        }

        private void CheckAllTilesPainted()
        {
            if (paintedRoadTiles == levelManager.RoadTilesList.Count)
            {
                Game.LevelCompleted?.Invoke();
            }
        }
    }
}