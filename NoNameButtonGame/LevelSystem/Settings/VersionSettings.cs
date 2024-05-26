using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Settings;

public class VersionSettings : ISettings
{
    public System.Version Version { get; set; } = new System.Version(0, 0, 0, 0);
}