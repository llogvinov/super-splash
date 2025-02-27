using System;
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
        private PlayerData _playerData;
        private LevelCompletedUI _levelCompletedUI;

        public GameLoopState(GameStateMachine stateMachine,
            AllServices services)
        {
            _stateMachine = stateMachine;
            _services = services;
        }

        public void Enter()
        {
            _playerData = _services.Single<IPlayerDataService>().Load();

            Game.LevelCompleted += OnLevelCompleted;
        }

        private void OnLevelCompleted()
        {
            _levelCompletedUI = GameObject.FindObjectOfType<LevelCompletedUI>();
            if (_levelCompletedUI != null)
            {
                _levelCompletedUI.Show();
                _levelCompletedUI.NextLevelButtonClicked += OnNextLevelButtonClicked;
            }
        }

        private void OnNextLevelButtonClicked()
        {
            _levelCompletedUI.NextLevelButtonClicked -= OnNextLevelButtonClicked;
            _levelCompletedUI.Hide();
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