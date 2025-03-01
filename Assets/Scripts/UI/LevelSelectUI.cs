using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
        private LevelButtonUI _currentSelectedButton;

        private void Awake()
        {
            _playerData = AllServices.Container.Single<IPlayerDataService>().Load();
            _levelButtonUIList = new List<LevelButtonUI>();
        }

        private void Start()
        {
            CleanUp();
            Initialize();
            LevelButtonUI.LevelButtonClicked += OnLevelButtonClicked;
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

        private async void OnNextLevelButtonClicked()
        {
            var nextLevelNumber = _currentSelectedButton.LevelNumber + 1;
            var nextLevelButton = _levelButtonUIList.FirstOrDefault(l => nextLevelNumber == l.LevelNumber);
            SelectLevelButton(nextLevelButton);
            await Task.Delay(500);
            NextLevelButtonClicked?.Invoke();
        }

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
                if (levelButton.LevelNumber == _playerData.CurrentLevelNumber)
                {
                    _currentSelectedButton = levelButton;
                    _currentSelectedButton.ToggleOutline(true);
                }
                else
                {
                    levelButton.ToggleOutline(false);
                }
                _levelButtonUIList.Add(levelButton);
            }
        }

        public void UpdateUI(PlayerData playerData)
        {
            foreach (var levelButton in _levelButtonUIList)
            {
                var isLevelOpened = playerData.OpenedLevels.Contains(levelButton.LevelNumber);
                levelButton.UpdateLock(isLevelOpened);
            }
        }

        private void OnLevelButtonClicked(LevelButtonUI levelButton)
        {
            SelectLevelButton(levelButton);
        }

        private void SelectLevelButton(LevelButtonUI levelButton)
        {
            _currentSelectedButton.ToggleOutline(false);
            _currentSelectedButton = levelButton;
            _currentSelectedButton.ToggleOutline(true);
        }
    }
}