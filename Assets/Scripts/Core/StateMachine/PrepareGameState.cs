using Core.Services;
using Core.Services.PlayerData;
using UI;
using UnityEngine;

namespace Core.StateMachine
{
    public class PrepareGameState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly AllServices _services;
        private PlayerData _playerData;

        public PrepareGameState(GameStateMachine stateMachine,
            AllServices services)
        {
            _stateMachine = stateMachine;
            _services = services;
        }

        public void Enter()
        {
            _playerData = _services.Single<IPlayerDataService>().Load();
            Debug.Log(_playerData.CurrentLevelNumber);

            var levelNumberUI = GameObject.FindObjectOfType<LevelNumberUI>();
            if (levelNumberUI != null)
            {
                levelNumberUI.SetLevelNumber(_playerData.CurrentLevelNumber);
            }

            var levelManager = GameObject.FindObjectOfType<LevelManager>();
            if (levelManager != null)
            {
                levelManager.Initialize(_playerData.CurrentLevelNumber);
            }

            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {

        }
    }
}