using System;
using Core.Services;
using Core.StateMachine;

namespace Core
{
    public class Game
    {
        private readonly GameStateMachine _stateMachine;

        public GameStateMachine StateMachine => _stateMachine;

        public static Action LevelCompleted;

        public Game(ICoroutineRunner coroutineRunner)
        {
            _stateMachine = new GameStateMachine(this,
                new SceneLoader(coroutineRunner),
                AllServices.Container);
        }

    }
}