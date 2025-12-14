using System.Collections.Generic;
using Joyersch.Monogame.Console;
using Joyersch.Monogame.Ui;
using NoNameButtonGame.Ui;

namespace NoNameButtonGame.Console;

public interface IProcessor
{
    public List<(CommandAttribute Attribute, CommandOptionsAttribute[] Options, ICommand Command)> Commands { get; }

    public IEnumerable<string> Process(DevConsole caller, string fullCommand, ContextProvider context);

    public string? PossibleMatch(string search);
}