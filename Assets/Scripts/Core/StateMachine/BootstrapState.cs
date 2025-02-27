using Core.Services;
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
            _sceneLoader.LoadScene("Game", onLoaded: () => 
                _stateMachine.Enter<PrepareGameState>());
        }

        public void Exit()
        {
            
        }

        private void RegisterServices()
        {
            _services.RegisterSingle<IPlayerDataService>(new LocalPlayerDataService());
        }
    }
}