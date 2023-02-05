using System;

namespace NoNameButtonGame;

public static class Program
{
    [STAThread]
    private static void Main()
    {
        using (var game = new NoNameGame())
            game.Run();
    }
}