using System.Collections.Generic;

namespace PongGame.Core
{
    /// <summary>
    /// State Machine to manage game states
    /// </summary>
    public class StateMachine
    {
        private IGameState? _currentState;
        private readonly Dictionary<string, IGameState> _states;

        public StateMachine()
        {
            _states = new Dictionary<string, IGameState>();
        }

        public void AddState(string name, IGameState state)
        {
            _states[name] = state;
        }

        public void ChangeState(string stateName)
        {
            if (!_states.ContainsKey(stateName))
            {
                return;
            }

            if (_currentState != null)
            {
                _currentState.Exit();
            }
            _currentState = _states[stateName];
            _currentState.Enter();
        }

        public void Update(float deltaTime)
        {
            if (_currentState != null)
            {
                _currentState.Update(deltaTime);
            }
        }

        public IGameState? GetCurrentState()
        {
            return _currentState;
        }
    }
}
