using System.Linq;
using Core.Services;
using Core.Services.Ad;
using Core.Services.PlayerData;
using UI;
using UnityEngine;

namespace Core.StateMachine
{
    public class PrepareGameState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly AllServices _services;
        private readonly LevelsData _levelsData;
        private PlayerData _playerData;

        private LevelManager _levelManager;
        private LevelManager LevelManager => _levelManager ??= GameObject.FindObjectOfType<LevelManager>();

        private LevelNumberUI _levelNumberUI;
        private LevelNumberUI LevelNumberUI => _levelNumberUI ??= GameObject.FindObjectOfType<LevelNumberUI>();

        private SkipLevelUI _skipLevelUI;
        private SkipLevelUI SkipLevelUI => _skipLevelUI ??= GameObject.FindObjectOfType<SkipLevelUI>();

        public PrepareGameState(GameStateMachine stateMachine,
            AllServices services)
        {
            _stateMachine = stateMachine;
            _services = services;
            _levelsData = Resources.Load<LevelsData>("LevelsData");
        }

        public void Enter()
        {
            AllServices.Container.Single<IAdService>().ShowInterstitialAd();
            _playerData = _services.Single<IPlayerDataService>().Load();
            Debug.Log(_playerData.CurrentLevelNumber);

            LevelNumberUI.SetLevelNumber(_playerData.CurrentLevelNumber);

            if (IsNextLevelOpened())
                SkipLevelUI.HideUI();
            else
                SkipLevelUI.ShowUI();

            var levelData = GetDataByLevelNumberNumber(_playerData.CurrentLevelNumber);
            LevelManager.Initialize(levelData);

            _stateMachine.Enter<GameLoopState>();
        }

        public void Exit()
        {

        }

        private LevelData GetDataByLevelNumberNumber(uint levelNumber) =>
            _levelsData.LevelDataList.FirstOrDefault(l => l.LevelNumber == levelNumber);

        private bool IsNextLevelOpened() => 
            _playerData.OpenedLevels.Contains(_playerData.CurrentLevelNumber + 1);
    }
}