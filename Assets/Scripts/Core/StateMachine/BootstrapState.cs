using Core.Services;
using Core.Services.Ad;
using Core.Services.PlayerData;

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

        private void ResetProgress()
        {
            // YG2.SetDefaultSaves();
            // YG2.SaveProgress();
            // var playerData = new PlayerData();
            // playerData.CurrentLevelNumber = 1;
            // playerData.OpenedLevels =  new List<uint>() { 1 };
            // _services.Single<IPlayerDataService>().Save(playerData);
        }

        public void Exit()
        {

        }

        private void RegisterServices()
        {
            RegisterPlayerDataService();
            RegisterAdService();
        }

        private void RegisterPlayerDataService()
        {
#if UNITY_EDITOR
            _services.RegisterSingle<IPlayerDataService>(new LocalPlayerDataService());
#else
            _services.RegisterSingle<IPlayerDataService>(new YandexPlayerDataService());
#endif
        }

        private void RegisterAdService()
        {
#if UNITY_WEBGL
            _services.RegisterSingle<IAdService>(new YandexAdService());
#endif
        }
    }
}