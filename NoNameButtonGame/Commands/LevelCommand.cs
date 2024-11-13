using System;
using System.Collections.Generic;
using MonoUtils.Console;
using MonoUtils.Settings;
using NoNameButtonGame.LevelSystem;
using NoNameButtonGame.LevelSystem.Selection;

namespace NoNameButtonGame.Commands;

public sealed class LevelCommand : ICommand
{
    [Command(Description = "Select a level", Name = "level")]
    [CommandOptions(Name = "menu", Depth = 1)]
    [CommandOptions(Name = "settings", Depth = 1)]
    [CommandOptions(Name = "credits", Depth = 1)]
    [CommandOptions(Name = "select", Depth = 1)]
    [CommandOptions(Name = "endless", Depth = 1)]
    [CommandOptions(Name = "complete", Depth = 1)]
    [CommandOptions(Name = "current", Depth = 1)]
    public IEnumerable<string> Execute(DevConsole caller, object[] options, ContextProvider context)
    {
        var levelManager = context.GetValue<LevelManager>(nameof(LevelManager));
        var saves = context.GetValue<SettingsAndSaveManager<string>>(nameof(SettingsAndSaveManager<string>));
        var selectionSettings = saves.GetSave<SelectionState>();

        if (options.Length < 1)
            return ["level (level) | number 1-10 or string. See \"help level\" for list of strings."];

        var value = options[0].ToString();

        switch (options[0].ToString())
        {
            case "complete":
                levelManager.GetCurrentLevel().Finish();
                return ["Completed the current level!", $"Endless would be on: {levelManager.EndlessLevel}"];
            case "current":
                var state = levelManager.GetCurrenState();
                bool returnId = state is LevelManager.LevelState.EndlessLevel or LevelManager.LevelState.SelectLevel
                    or LevelManager.LevelState.Level;
                object text = returnId ? levelManager.GetCurrentLevelId() : state;
                return [$"Current level id is: {text}"];
        }

        if (int.TryParse(value, out var level))
        {
            levelManager.SetLevelState(LevelManager.LevelState.SelectLevel);
            levelManager.ChangeLevel(level, Level.ResolveDifficulty(selectionSettings.Difficulty));
            return [$"changed level to {level}!"];
        }

        Enum.TryParse(typeof(LevelManager.LevelState), value, true, out var result);
        if (result is not null)
        {
            var state = (LevelManager.LevelState)result;

            if (state is not (LevelManager.LevelState.EndlessLevel or LevelManager.LevelState.SelectLevel
                or LevelManager.LevelState.Level))
            {
                levelManager.SetLevelState(state);
                return [$"Updated level to {result}"];
            }
        }

        return ["level (level) | number 1-10 or string. See \"help level\" for list of strings."];
    }
}