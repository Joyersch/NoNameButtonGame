using System.Collections.Generic;
using Joyersch.Monogame.Console;
using Joyersch.Monogame.Ui;
using NoNameButtonGame.Console;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.Ui;

namespace NoNameButtonGame.Commands;

public sealed class ExitCommand : ICommand
{
    [Command(Description = "Exits the game", Name = "exit")]
    public IEnumerable<string> Execute(DevConsole caller, object[] options, ContextProvider context)
    {
        var levelManager = context.GetValue<LevelManager>(nameof(LevelManager));
        levelManager.Exit();
        return new[] { "Exiting!" };
    }
}