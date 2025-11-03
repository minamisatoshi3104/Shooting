using System;
using System.Threading.Tasks;

namespace Shooting
{
    public class Game
    {
        private StateMachine _stateMachine = new StateMachine();
        public Game()
        {
            _stateMachine.RegisterState(new TitleState());
            _stateMachine.RegisterState(new InGameState());
            _stateMachine.RegisterState(new ResultState());

            _stateMachine.ChangeState(StateType.Title);
        }

        public async Task Run()
        {
            Console.WriteLine("Game started. Press Ctrl+C to exit.");
            while (true)
            {
                _stateMachine.Update();
                await Task.Delay(100);
            }
        }
    }
}