using System;
using System.Collections.Generic;
using Core.Services;
using Core.Services.PlayerData;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectUI : MonoBehaviour
    {
        public Action NextLevelButtonClicked;
        
        [SerializeField] private Canvas _canvas;
        [SerializeField] private LevelsData _levelsData;
        [SerializeField] private Transform _container;
        [SerializeField] private LevelButtonUI _levelButtonUI;
        [SerializeField] private Button _nextLevelButton;
        
        private PlayerData _playerData;
        private List<LevelButtonUI> _levelButtonUIList;

        private void Awake()
        {
            _playerData = AllServices.Container.Single<IPlayerDataService>().Load();
            _levelButtonUIList = new List<LevelButtonUI>();
        }

        private void Start()
        {
            CleanUp();
            Initialize();
            LevelButtonUI.LoadLevelEvent += HideUI;
            _nextLevelButton.onClick.AddListener(OnNextLevelButtonClicked);
        }

        private void OnDestroy()
        {
            LevelButtonUI.LoadLevelEvent -= HideUI;
            _nextLevelButton.onClick.RemoveListener(OnNextLevelButtonClicked);
        }

        public void ShowUI()
        {
            _canvas.gameObject.SetActive(true);
        }

        public void HideUI()
        {
            _canvas.gameObject.SetActive(false);
        }

        private void OnNextLevelButtonClicked() => 
            NextLevelButtonClicked?.Invoke();

        private void HideUI(uint levelNumber)
        {
            HideUI();
        }

        private void CleanUp()
        {
            foreach (Transform child in _container.transform)
            {
                Destroy(child.gameObject);
            }
            _levelButtonUIList.Clear();
        }

        private void Initialize()
        {
            foreach (var levelData in _levelsData.LevelDataList)
            {
                var levelButton = Instantiate(_levelButtonUI, _container);
                var isLevelOpened = _playerData.OpenedLevels.Contains(levelData.LevelNumber);
                levelButton.Initialize(levelData, isLevelOpened);
                _levelButtonUIList.Add(levelButton);
            }
        }

        public void UpdateUI(PlayerData playerData)
        {
            foreach(var levelButton in _levelButtonUIList)
            {
                var isLevelOpened = playerData.OpenedLevels.Contains(levelButton.LevelNumber);
                levelButton.UpdateLock(isLevelOpened);
            }
        }
    }
}