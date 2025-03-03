using UnityEngine;
using System.Collections.Generic;
using System;

public class LevelManager : MonoBehaviour
{
    public static Action Initialized;

    [SerializeField] private Transform _plane;
    [SerializeField] private Transform _map;

    [Header("Tiles Prefabs")]
    [SerializeField] private GameObject _prefabWallTile;
    [SerializeField] private RoadTile _prefabRoadTile;

    public Color PaintColor { get; private set; }
    public List<RoadTile> RoadTilesList { get; private set; }
    public RoadTile DefaultBallRoadTile { get; private set; }
    public LevelData CurrentLevelData { get; private set; }

    private Color _colorWall = Color.white;
    private Color _colorRoad = Color.black;
    private Color _colorStart = Color.gray;

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
                {
                    SpawnWall(_prefabWallTile, spawnPos);
                }
                else if (pixelColor == _colorRoad)
                {
                    SpawnRoadTile(spawnPos, pixelColor);
                }
                else
                {
                    var color = RoundColor(pixelColor);
                    if (color == _colorStart)
                    {
                        SpawnRoadTile(spawnPos, color);
                    }
                }
            }
        }

        if (DefaultBallRoadTile == null)
        {
            DefaultBallRoadTile = RoadTilesList[0];
        }
    }

    private void ClearMap()
    {
        foreach (Transform child in _map.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SpawnWall(GameObject prefabTile, Vector3 position)
    {
        position.y = prefabTile.transform.position.y;

        var wallTile = Instantiate(prefabTile, position, Quaternion.identity, _map);
        wallTile.GetComponent<Renderer>().material.color = CurrentLevelData.WallColor;
    }

    private void SpawnRoadTile(Vector3 position, Color color)
    {
        position.y = _prefabRoadTile.transform.position.y;

        var roadTile = Instantiate(_prefabRoadTile, position, Quaternion.identity, _map);
        RoadTilesList.Add(roadTile);
        if (color == _colorStart)
        {
            DefaultBallRoadTile = roadTile;
        }
    }

    public static Color RoundColor(Color color, int decimalPlaces = 2)
    {
        float factor = Mathf.Pow(10, decimalPlaces);
        float r = Mathf.Round(color.r * factor) / factor;
        float g = Mathf.Round(color.g * factor) / factor;
        float b = Mathf.Round(color.b * factor) / factor;
        float a = Mathf.Round(color.a * factor) / factor;

        return new Color(r, g, b, a);
    }
}
