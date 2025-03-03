using System.Collections.Generic;
using System.Linq;
using Core.Services;
using Core.Services.PlayerData;
using Main;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class LevelSelectUI : BaseUI
    {
        [SerializeField] private Button _openButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private LevelsData _levelsData;
        [SerializeField] private Transform _container;
        [SerializeField] private LevelButtonUI _levelButtonUI;
        [Space]
        [SerializeField] private List<GameObject> _objectsToHideOnOpen;

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
            LevelButtonUI.LoadLevelEvent += OnLoadedLevel;
            _openButton.onClick.AddListener(OnOpenClicked);
            _closeButton.onClick.AddListener(OnCloseClicked);
        }
        private void OnDestroy()
        {
            LevelButtonUI.LevelButtonClicked -= OnLevelButtonClicked;
            LevelButtonUI.LoadLevelEvent -= OnLoadedLevel;
            _openButton.onClick.RemoveListener(OnOpenClicked);
            _closeButton.onClick.RemoveListener(OnCloseClicked);
        }

        public override void ShowUI()
        {
            BallMoveController.IsInputAllowed = false;
            base.ShowUI();
            ToggleHidable(false);
        }

        public override void HideUI()
        {
            BallMoveController.IsInputAllowed = true;
            base.HideUI();
            ToggleHidable(true);
        }

        private void OnOpenClicked() => 
            ShowUI();

        private void OnCloseClicked() => 
            HideUI();

        private void OnLoadedLevel(uint levelNumber) => 
            HideUI();

        private void ToggleHidable(bool enable)
        {
            foreach (var obj in _objectsToHideOnOpen)
            {
                obj.SetActive(enable);
            }
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

            var nextLevelButton = _levelButtonUIList.FirstOrDefault(l => playerData.CurrentLevelNumber == l.LevelNumber);
            SelectLevelButton(nextLevelButton);
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