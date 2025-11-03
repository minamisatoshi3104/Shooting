using System;
using UnityEngine;

namespace Shooting
{
    public class TitleState : IState
    {
        public StateType Type => StateType.Title;
        private StateMachine? _machine;
        public void SetStateMachine(StateMachine machine) => _machine = machine;
        public void Enter()
        {
            Debug.Log("Enter Title");
            // Load prefab and get TitleScene component
            var scene = SceneController.Instance.LoadSceneForState<TitleScene>(StateType.Title);
            if (scene != null)
            {
                // scene.Initialize() already invoked by SceneController
            }
        }

        public void Exit()
        {
            Debug.Log("Exit Title");
        }

        public void Update()
        {
            // For editor testing, use key input
            if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
            {
                _machine?.ChangeState(StateType.InGame);
            }
        }
    }
}