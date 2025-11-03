using System;
using System.Threading.Tasks;
using UnityEngine;

namespace Shooting
{
    public class InGameState : IState
    {
        public StateType Type => StateType.InGame;
        private int _time = 0;
        private StateMachine? _machine;
        public void SetStateMachine(StateMachine machine) => _machine = machine;
        public void Enter()
        {
            Debug.Log("Enter InGame");
            _time = 0;
            var scene = SceneController.Instance.LoadSceneForState<InGameScene>(StateType.InGame);
        }

        public void Exit()
        {
            Debug.Log("Exit InGame");
        }

        public void Update()
        {
            // Simple timer
            _time++;
            if (_time % 10 == 0)
            {
                Debug.Log($"InGame running... {_time}");
            }
            if (_time > 50)
            {
                // go to result
                _machine?.ChangeState(StateType.Result);
            }
        }
    }
}
