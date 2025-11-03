using System;
using UnityEngine;

namespace Shooting
{
    public class ResultState : IState
    {
        public StateType Type => StateType.Result;
        private StateMachine? _machine;
        public void SetStateMachine(StateMachine machine) => _machine = machine;
        public void Enter()
        {
            Debug.Log("Enter Result");
            var scene = SceneController.Instance.LoadSceneForState<ResultScene>(StateType.Result);
        }

        public void Exit()
        {
            Debug.Log("Exit Result");
        }

        public void Update()
        {
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _machine?.ChangeState(StateType.Title);
            }
        }
    }
}