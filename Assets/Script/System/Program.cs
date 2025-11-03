using System;
using System.Threading.Tasks;

namespace Shooting
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var game = new Game();
            await game.Run();
        }
    }
}