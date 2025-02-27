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
            _levelSelectUI = GameObject.FindObjectOfType<LevelSelectUI>();
            if (_levelSelectUI != null)
            {
                _levelSelectUI.ShowUI();
                _levelSelectUI.NextLevelButtonClicked += OnNextLevelButtonClicked;
            }
        }

        private void LoadLevel(uint levelNumber)
        {
            _playerData.CurrentLevelNumber = levelNumber;
            _services.Single<IPlayerDataService>().Save(_playerData);
            _stateMachine.Enter<PrepareGameState>();
        }

        private void OnNextLevelButtonClicked()
        {
            _levelSelectUI.NextLevelButtonClicked -= OnNextLevelButtonClicked;
            _levelSelectUI.HideUI();
            _playerData.CurrentLevelNumber++;
            _services.Single<IPlayerDataService>().Save(_playerData);
            _stateMachine.Enter<PrepareGameState>();
        }

        public void Exit()
        {
            Game.LevelCompleted -= OnLevelCompleted;
        }
    }
}