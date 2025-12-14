using System.Collections.Generic;
using Joyersch.Monogame;
using Joyersch.Monogame.Ui;
using NoNameButtonGame.Ui;

namespace NoNameButtonGame.Console.Commands;

public sealed class LanguageCommand : ICommand
{
    [Command(Description = "Change the used language", Name = "lang")]
    [CommandOptions(Name = "de", Depth = 1, Description = "German")]
    [CommandOptions(Name = "en", Depth = 1, Description = "English")]
    public IEnumerable<string> Execute(DevConsole console, object[] options, ContextProvider context)
    {
        if (options.Length < 1)
            return new[] { "Usage:", "lang (lang)", "Current:", TextProvider.Localization.ToString()};

        if (options[0].ToString() is null)
            return new[] { "Invalid input supplied!" };

        TextProvider.Language? toUse = null;
        if (options[0].ToString()!.ToLower().Contains("de"))
            toUse = TextProvider.Language.de_DE;

        if (options[0].ToString()!.ToLower().Contains("en"))
            toUse = TextProvider.Language.en_GB;

        if (toUse is null)
            return new[] { "Unknown language" };
        TextProvider.Localization = toUse.Value;
        return new[] { "Changed language" };
    }
}