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
        private IPlayerDataService _playerDataService;
        private PlayerData _playerData;
        private LevelSelectUI _levelSelectUI;

        public GameLoopState(GameStateMachine stateMachine,
            AllServices services)
        {
            _stateMachine = stateMachine;
            _services = services;
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
            var nextLevel = _playerData.CurrentLevelNumber + 1;
            _playerData.OpenedLevels.Add(nextLevel);
            _playerDataService.Save(_playerData);
            Debug.Log(string.Join(", ", _playerDataService.Load().OpenedLevels));

            _levelSelectUI = GameObject.FindObjectOfType<LevelSelectUI>();
            if (_levelSelectUI != null)
            {
                _levelSelectUI.UpdateUI(_playerData);
                _levelSelectUI.ShowUI();
                _levelSelectUI.NextLevelButtonClicked += OnNextLevelButtonClicked;
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
            _levelSelectUI.NextLevelButtonClicked -= OnNextLevelButtonClicked;
            _levelSelectUI.HideUI();

            _playerData.CurrentLevelNumber += 1;
            _playerDataService.Save(_playerData);
            _stateMachine.Enter<PrepareGameState>();
        }

        public void Exit()
        {
            Game.LevelCompleted -= OnLevelCompleted;
            LevelButtonUI.LoadLevelEvent -= LoadLevel;
            _levelSelectUI.NextLevelButtonClicked -= OnNextLevelButtonClicked;
        }
    }
}