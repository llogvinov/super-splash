using Core.Services;
using Core.Services.PlayerData;
using UnityEngine;
using YG;

namespace Core.StateMachine
{
    public class BootstrapState : ISimpleState
    {
        private readonly GameStateMachine _stateMachine;
        private readonly SceneLoader _sceneLoader;
        private readonly AllServices _services;

        public BootstrapState(GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            AllServices services)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;

            RegisterServices();
        }

        public void Enter()
        {
            ResetProgress();
            _sceneLoader.LoadScene("Game", onLoaded: () =>
                _stateMachine.Enter<PrepareGameState>());
        }

        private static void ResetProgress() => 
            YG2.SetDefaultSaves();

        public void Exit()
        {

        }

        private void RegisterServices()
        {
#if UNITY_EDITOR
            _services.RegisterSingle<IPlayerDataService>(new LocalPlayerDataService());
#else
            _services.RegisterSingle<IPlayerDataService>(new YandexPlayerDataService());
#endif
        }
    }
}