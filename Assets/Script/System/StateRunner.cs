using UnityEngine;

// MonoBehaviour bridge to run StateMachine in Unity playmode
public class StateRunner : MonoBehaviour
{
    public Shooting.StateMachine machine = new Shooting.StateMachine();

    void Start()
    {
        machine.RegisterState(new Shooting.TitleState());
        machine.RegisterState(new Shooting.InGameState());
        machine.RegisterState(new Shooting.ResultState());

        machine.ChangeState(Shooting.StateType.Title);
    }

    void Update()
    {
        machine.Update();
    }
}