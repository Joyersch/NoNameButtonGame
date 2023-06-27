using System.Collections.Generic;
using MonoUtils.Objects;
using NoNameButtonGame.LevelSystem;

namespace NoNameButtonGame.Commands;

public class LevelCommand : ICommand
{
    [CommandAttribute(Description = "Select a level", Name = "level")]
    public IEnumerable<string> Execute(DevConsole caller, object[] options, ContextProvider context)
    {
        var levelManager = context.GetValue<LevelManager>(nameof(LevelManager));
        if (options.Length <= 1)
            return new[] {"Usage:", "level [level]"};

        var value = options[1].ToString();
        return !levelManager.ChangeLevel(value) ? new[] {"parameter is not a valid level or is the current level!"} : new[] {$"changed level to {value}!"};
    }
}