using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Settings;

public class AdvancedSettings : ISettings
{
    public bool ConsoleEnabled { get; set; }

    public bool ShowElapsedTime { get; set; }
}