using System;

namespace NoNameButtonGame
{
    public static class Program
    {
        [STAThread]
        static void Main() {
            using (var game = new NoNameGame())
                game.Run();

        }
    }
}
