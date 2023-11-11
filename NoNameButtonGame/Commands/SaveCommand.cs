using System;
using System.Collections.Generic;
using MonoUtils.Logic.Text;
using MonoUtils.Ui.Objects.Console;
using NoNameButtonGame.LevelSystem;

namespace NoNameButtonGame.Commands;

public class SaveCommand : ICommand
{
    [CommandAttribute(Description = "Overwrite data in save file. Not all options supported!", Name = "save")]
    public IEnumerable<string> Execute(DevConsole caller, object[] options, ContextProvider context)
    {
        var levelManager = context.GetValue<LevelManager>(nameof(LevelManager));
        if (options.Length < 1)
            return new[] { "Usage:", "save [key] [value]" };

        string key = options[0].ToString().ToLower();
        string value = null;

        if (options.Length >= 2)
            value = options[1].ToString();

        /*
        var storage = context.GetValue<Storage.Storage>(nameof(Storage));

        if (key == "maxlevel")
        {
            if (value is null)
                return new[] { storage.GameData.MaxLevel.ToString() };
            if (!int.TryParse(value, out int level))
                return new[] { $"Bad value for key \"{key}\"!" };

            storage.GameData.MaxLevel = level;
            return new[] { $"Changed max reached level to {level}!" };
        }

        if (key == "lang")
        {
            if (value is null)
                return new[] { storage.Settings.Localization.ToString() };

            if (!Enum.TryParse<TextProvider.Language>(value, out TextProvider.Language language))
                return new[] { "Invalid value provided!" };
            storage.Settings.Localization = language;
            return new[] { $"Change language to \"{language.ToString()}\"" };
        }

        if (key == "save")
        {
            storage.Save();
            return new[] { $"Saved!" };
        }
        */

        return new[] { $"Unknown key!" };
    }
}