using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;

public class LevelManager : MonoBehaviour
{
    public static Action Initialized;

    [SerializeField] private Transform _plane;
    [SerializeField] private Transform _map;

    [Header("Tiles Prefabs")]
    [SerializeField] private GameObject _prefabWallTile;
    [SerializeField] private GameObject _prefabRoadTile;

    public Color PaintColor { get; private set; }
    public List<RoadTile> RoadTilesList { get; private set; }
    public RoadTile DefaultBallRoadTile { get; private set; }
    public LevelData CurrentLevelData { get; private set; }

    private Color _colorWall = Color.white;
    private Color _colorRoad = Color.black;

    private float _unitPerPixel;
    private Texture2D _levelTexture;

    public void Initialize(LevelData levelData)
    {
        CurrentLevelData = levelData;
        _levelTexture = CurrentLevelData.LevelTexture;
        PaintColor = CurrentLevelData.PaintColor;
        Generate();
        Initialized?.Invoke();
    }

    private void Generate()
    {
        ClearMap();
        RoadTilesList = new List<RoadTile>();
        _unitPerPixel = _prefabWallTile.transform.lossyScale.x;
        var halfUnitPerPixel = _unitPerPixel / 2f;
        var width = _levelTexture.width;
        var height = _levelTexture.height;
        _plane.localScale = new Vector3(width * _unitPerPixel / 10f, 1, height * _unitPerPixel / 10f);

        var offset = (new Vector3(width / 2f, 0f, height / 2f) * _unitPerPixel)
           - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var pixelColor = _levelTexture.GetPixel(x, y);
                var spawnPos = (new Vector3(x, 0f, y) * _unitPerPixel) - offset;
                if (pixelColor == _colorWall)
                    Spawn(_prefabWallTile, spawnPos);
                else if (pixelColor == _colorRoad)
                    Spawn(_prefabRoadTile, spawnPos);
            }
        }

        DefaultBallRoadTile = RoadTilesList[0];
    }

    private void ClearMap()
    {
        foreach (Transform child in _map.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void Spawn(GameObject prefabTile, Vector3 position)
    {
        position.y = prefabTile.transform.position.y;

        var obj = Instantiate(prefabTile, position, Quaternion.identity, _map);
        if (prefabTile == _prefabRoadTile)
            RoadTilesList.Add(obj.GetComponent<RoadTile>());
    }
}
