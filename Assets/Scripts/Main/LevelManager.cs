using UnityEngine;
using System.Collections.Generic;
using System;
using Main;
using DG.Tweening;

public class LevelManager : MonoBehaviour
{
    public static Action OnBeforeInitialized, Initialized;

    [SerializeField] private LevelsBackgroundData _backgroundData;
    [SerializeField] private Transform _plane;
    [SerializeField] private Transform _map;

    [Header("Tiles Prefabs")]
    [SerializeField] private GameObject _prefabWallTile;
    [SerializeField] private RoadTile _prefabRoadTile;

    [Header("Spawn Settings")]
    [SerializeField] private float _spawnDelayMultiplier;
    [SerializeField] private float _yOffset;

    public Color PaintColor { get; private set; }
    public List<RoadTile> RoadTilesList { get; private set; }
    public RoadTile DefaultBallRoadTile { get; private set; }
    public LevelData CurrentLevelData { get; private set; }

    private Color _colorWall = Color.white;
    private Color _colorRoad = Color.black;
    private Color _colorStart = Color.gray;

    private const float SMALL_LEVEL_CAMERA_SIZE = 3.5f;
    private const float MEDIUM_LEVEL_CAMERA_SIZE = 4.5f;
    private const float BIG_LEVEL_CAMERA_SIZE = 5.5f;

    private float _unitPerPixel;
    private float _maxInterval;
    private Texture2D _levelTexture;
    private Camera _camera;

    private void Awake()
    {
        _camera = Camera.main;
    }

    public void Initialize(LevelData levelData)
    {
        CurrentLevelData = levelData;
        _levelTexture = CurrentLevelData.LevelTexture;
        PaintColor = CurrentLevelData.PaintColor;
        OnBeforeInitialized?.Invoke();
        _plane.localScale = Vector3.zero;
        Generate();
    }

    private void Generate()
    {
        ClearMap();
        UpdateCamera();
        RoadTilesList = new List<RoadTile>();
        _unitPerPixel = _prefabWallTile.transform.lossyScale.x;
        var halfUnitPerPixel = _unitPerPixel / 2f;
        var width = _levelTexture.width;
        var height = _levelTexture.height;
        var offset = (new Vector3(width / 2f, 0f, height / 2f) * _unitPerPixel)
           - new Vector3(halfUnitPerPixel, 0f, halfUnitPerPixel);

        _maxInterval = _spawnDelayMultiplier * (width - 1 + height - 1);

        for (var x = 0; x < width; x++)
        {
            for (var y = 0; y < height; y++)
            {
                var pixelColor = _levelTexture.GetPixel(x, y);
                var spawnPos = (new Vector3(x, 0f, y) * _unitPerPixel) - offset;

                var sequence = DOTween.Sequence();
                var interval = _spawnDelayMultiplier * (x + y);
                if (pixelColor == _colorWall)
                {
                    sequence.AppendInterval(interval)
                        .AppendCallback(() => SpawnWall(_prefabWallTile, spawnPos, interval));
                }
                else if (pixelColor == _colorRoad)
                {
                    sequence.AppendInterval(interval)
                        .AppendCallback(() => SpawnRoadTile(spawnPos, pixelColor));
                }
                else
                {
                    var color = RoundColor(pixelColor);
                    if (color == _colorStart)
                    {
                        sequence.AppendInterval(interval)
                            .AppendCallback(() => SpawnRoadTile(spawnPos, color));
                    }
                }
            }
        }
    }

    private void UpdatePlane(int width, int height) =>
        _plane.localScale = new Vector3(width * _unitPerPixel / 10f, 1, height * _unitPerPixel / 10f);

    private void OnAllSpawned()
    {
        Debug.Log("completed");
        var width = _levelTexture.width;
        var height = _levelTexture.height;
        UpdatePlane(width, height);
        if (DefaultBallRoadTile == null)
        {
            DefaultBallRoadTile = RoadTilesList[0];
        }
        Initialized?.Invoke();
    }

    private void ClearMap()
    {
        foreach (Transform child in _map.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SpawnWall(GameObject prefabTile, Vector3 position, float interval)
    {
        position.y = prefabTile.transform.position.y + _yOffset;

        var wallTile = Instantiate(prefabTile, position, Quaternion.identity, _map);
        wallTile.GetComponent<Renderer>().material.color = CurrentLevelData.WallColor;

        wallTile.transform.DOMoveY(wallTile.transform.position.y - _yOffset, 0.1f).OnComplete(() =>
        {
            if (interval == _maxInterval)
            {
                OnAllSpawned();
            }
        });
    }

    private void SpawnRoadTile(Vector3 position, Color color)
    {
        position.y = _prefabRoadTile.transform.position.y + _yOffset;

        var roadTile = Instantiate(_prefabRoadTile, position, Quaternion.identity, _map);
        RoadTilesList.Add(roadTile);
        if (color == _colorStart)
        {
            DefaultBallRoadTile = roadTile;
        }

        roadTile.transform.DOMoveY(roadTile.transform.position.y - _yOffset, 0.1f);
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

    private void UpdateCamera()
    {
        UpdateCameraBackground(_backgroundData.BackgroundColors[(int)CurrentLevelData.LevelNumber / 11]);
        if (_levelTexture.height > 26f)
        {
            UpdateCameraSize(BIG_LEVEL_CAMERA_SIZE);
        }
        else if (_levelTexture.height > 17f)
        {
            UpdateCameraSize(MEDIUM_LEVEL_CAMERA_SIZE);
        }
        else
        {
            UpdateCameraSize(SMALL_LEVEL_CAMERA_SIZE);
        }
    }

    private void UpdateCameraSize(float cameraSize) =>
        _camera.orthographicSize = cameraSize;

    private void UpdateCameraBackground(Color color) =>
        _camera.backgroundColor = color;
}
