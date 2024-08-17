using System.Collections.Generic;
using MonoUtils.Console;
using MonoUtils.Settings;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.LevelSystem.Selection;

namespace NoNameButtonGame.Commands;

public sealed class LevelCommand : ICommand
{
    [Command(Description = "Select a level", Name = "level")]
    public IEnumerable<string> Execute(DevConsole caller, object[] options, ContextProvider context)
    {
        var levelManager = context.GetValue<LevelManager>(nameof(LevelManager));
        var saves = context.GetValue<SettingsAndSaveManager<string>>(nameof(SettingsAndSaveManager<string>));
        var selectionSettings = saves.GetSave<SelectionState>();

        if (options.Length < 1)
            return new[] { levelManager.GetCurrentLevelId().ToString() };

        var value = options[0].ToString();

        if (options[0].ToString() == "complete" || options[0].ToString() == "c")
        {
            levelManager.GetCurrentLevel().Finish();
            return new[] { "Completed the current level!", $"Endless would be on: {levelManager.EndlessLevel}" };
        }

        if (!int.TryParse(value, out var ival))
            return new[] { "Usage:", "level (level)" };

        levelManager.SetAsLevelSelect();
        levelManager.ChangeLevel(ival, Level.ResolveDifficulty(selectionSettings.Difficulty));
        return new[] { $"changed level to {ival}!" };
    }
}