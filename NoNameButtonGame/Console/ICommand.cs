using System.Collections.Generic;
using Joyersch.Monogame.Console;
using Joyersch.Monogame.Ui;
using NoNameButtonGame.Ui;

namespace NoNameButtonGame.Console;

public interface ICommand
{
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context);
}