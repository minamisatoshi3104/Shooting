using System;
using System.Collections.Generic;

namespace Shooting
{
    public enum StateType { Title, InGame, Result }

    public interface IState
    {
        StateType Type { get; }
        void SetStateMachine(StateMachine machine);
        void Enter();
        void Exit();
        void Update();
    }

    public class StateMachine
    {
        private Dictionary<StateType, IState> _states = new Dictionary<StateType, IState>();
        private IState? _current;

        public void RegisterState(IState state)
        {
            _states[state.Type] = state;
            state.SetStateMachine(this);
        }

        public void ChangeState(StateType type)
        {
            if (_current != null && _current.Type == type) return;

            _current?.Exit();
            if (_states.TryGetValue(type, out var next))
            {
                _current = next;
                _current.Enter();
            }
            else
            {
                throw new Exception($"State not found: {type}");
            }
        }

        public void Update()
        {
            _current?.Update();
        }
    }
}