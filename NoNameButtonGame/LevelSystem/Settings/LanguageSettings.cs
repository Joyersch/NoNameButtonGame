using MonoUtils.Logic.Text;
using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Settings;

public class LanguageSettings : ISettings
{
    public TextProvider.Language Localization { get; set; } = TextProvider.Language.en_GB;
}