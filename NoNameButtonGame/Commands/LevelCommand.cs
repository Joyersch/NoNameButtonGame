using System.Collections.Generic;
using MonoUtils.Console;
using NoNameButtonGame.LevelSystem;

namespace NoNameButtonGame.Commands;

public class LevelCommand : ICommand
{
    [Command(Description = "Select a level", Name = "level")]
    public IEnumerable<string> Execute(DevConsole caller, object[] options, ContextProvider context)
    {
        var levelManager = context.GetValue<LevelManager>(nameof(LevelManager));


        if (options.Length < 1)
            return new[] { levelManager.GetCurrentLevelId().ToString() };

        var value = options[0].ToString();

        if (options[0].ToString() == "complete")
        {
            levelManager.GetCurrentLevel().Finish();
            return new[] { "Completed the current level!" };
        }

        if (!int.TryParse(value, out int ival))
            return new[] { "Usage:", "level (level)"};

        levelManager.SetAsLevelSelect();
        levelManager.ChangeLevel(ival);
        return new[] { $"changed level to {ival}!" };
    }
}