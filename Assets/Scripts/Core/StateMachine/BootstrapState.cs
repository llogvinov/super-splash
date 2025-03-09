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
        private readonly ICoroutineRunner _coroutineRunner;

        public BootstrapState(GameStateMachine stateMachine,
            SceneLoader sceneLoader,
            AllServices services,
            ICoroutineRunner coroutineRunner)
        {
            _stateMachine = stateMachine;
            _sceneLoader = sceneLoader;
            _services = services;
            _coroutineRunner = coroutineRunner;
            RegisterServices();
        }

        public void Enter()
        {
            _sceneLoader.LoadScene("Game", onLoaded: () =>
                _stateMachine.Enter<PrepareGameState>());
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