using System.Collections.Generic;
using MonoUtils.Console;
using NoNameButtonGame.LevelSystem;

namespace NoNameButtonGame.Commands;

public class ExitCommand : ICommand
{
    [Command(Description = "Exits the game", Name = "exit")]
    public IEnumerable<string> Execute(DevConsole caller, object[] options, ContextProvider context)
    {
        var levelManager = context.GetValue<LevelManager>(nameof(LevelManager));
        levelManager.Exit();
        return new[] { "Exiting!" };
    }
}