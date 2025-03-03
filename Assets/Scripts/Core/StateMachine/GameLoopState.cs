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
        private LevelCompletedUI _levelCompletedUI;

        private LevelSelectUI LevelSelectUI => _levelSelectUI ??= GameObject.FindObjectOfType<LevelSelectUI>();
        private LevelCompletedUI LevelCompletedUI => _levelCompletedUI ??= GameObject.FindObjectOfType<LevelCompletedUI>();

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
                LevelCompletedUI.ToggleNextButton(false);
            }
            LevelCompletedUI.ShowUI();

            var nextLevel = _playerData.CurrentLevelNumber + 1;
            if (!_playerData.OpenedLevels.Contains(nextLevel))
            {
                _playerData.OpenedLevels.Add(nextLevel);
            }
            _playerDataService.Save(_playerData);

            LevelCompletedUI.NextLevelButtonClicked += OnNextLevelButtonClicked;
        }

        private void LoadLevel(uint levelNumber)
        {
            _playerData.CurrentLevelNumber = levelNumber;
            _playerDataService.Save(_playerData);
            LevelSelectUI.UpdateUI(_playerData);
            _stateMachine.Enter<PrepareGameState>();
        }

        private void OnNextLevelButtonClicked()
        {
            LevelCompletedUI.NextLevelButtonClicked -= OnNextLevelButtonClicked;
            LevelCompletedUI.HideUI();
            LevelSelectUI.HideUI();

            _playerData.CurrentLevelNumber += 1;
            _playerDataService.Save(_playerData);
            LevelSelectUI.UpdateUI(_playerData);
            _stateMachine.Enter<PrepareGameState>();
        }

        public void Exit()
        {
            Game.LevelCompleted -= OnLevelCompleted;
            LevelButtonUI.LoadLevelEvent -= LoadLevel;
            LevelCompletedUI.NextLevelButtonClicked -= OnNextLevelButtonClicked;
        }
    }
}