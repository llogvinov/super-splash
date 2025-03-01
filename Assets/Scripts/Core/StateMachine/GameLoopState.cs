using Core.Services;
using Core.Services.PlayerData;
using UI;
using UnityEngine;

namespace Core.StateMachine
{
    public class GameLoopState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly AllServices _services;
        private readonly LevelsData _levelsData;
        private IPlayerDataService _playerDataService;
        private PlayerData _playerData;
        private LevelSelectUI _levelSelectUI;

        private LevelSelectUI LevelSelectUI => _levelSelectUI ??= GameObject.FindObjectOfType<LevelSelectUI>();

        public GameLoopState(GameStateMachine stateMachine,
            AllServices services)
        {
            _stateMachine = stateMachine;
            _services = services;
            _levelsData = Resources.Load<LevelsData>("LevelsData");
        }

        public void Enter()
        {
            _playerDataService = _services.Single<IPlayerDataService>();
            _playerData = _playerDataService.Load();

            Game.LevelCompleted += OnLevelCompleted;
            LevelButtonUI.LoadLevelEvent += LoadLevel;
        }

        private void OnLevelCompleted()
        {
            if (_playerData.CurrentLevelNumber == _levelsData.MaxLevel)
            {
                LevelSelectUI.ToggleNextButton(false);
                LevelSelectUI.ShowUI();
                return;
            }

            var nextLevel = _playerData.CurrentLevelNumber + 1;
            if (!_playerData.OpenedLevels.Contains(nextLevel))
            {
                _playerData.OpenedLevels.Add(nextLevel);
            }
            _playerDataService.Save(_playerData);

            if (LevelSelectUI != null)
            {
                LevelSelectUI.UpdateUI(_playerData);
                LevelSelectUI.ShowUI();
                LevelSelectUI.NextLevelButtonClicked += OnNextLevelButtonClicked;
            }
        }

        private void LoadLevel(uint levelNumber)
        {
            _playerData.CurrentLevelNumber = levelNumber;
            _playerDataService.Save(_playerData);
            _stateMachine.Enter<PrepareGameState>();
        }

        private void OnNextLevelButtonClicked()
        {
            LevelSelectUI.NextLevelButtonClicked -= OnNextLevelButtonClicked;
            LevelSelectUI.HideUI();

            _playerData.CurrentLevelNumber += 1;
            _playerDataService.Save(_playerData);
            _stateMachine.Enter<PrepareGameState>();
        }

        public void Exit()
        {
            Game.LevelCompleted -= OnLevelCompleted;
            LevelButtonUI.LoadLevelEvent -= LoadLevel;
            LevelSelectUI.NextLevelButtonClicked -= OnNextLevelButtonClicked;
        }
    }
}