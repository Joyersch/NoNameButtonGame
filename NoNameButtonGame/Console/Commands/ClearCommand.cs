using System.Collections.Generic;
using Joyersch.Monogame.Ui;
using NoNameButtonGame.Ui;

namespace NoNameButtonGame.Console.Commands;

public sealed class ClearCommand : ICommand
{
    [Command(Description = "Clears the console backlog.", Name = "clear")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
    {
        console.Backlog.Clear();
        return [];
    }
}