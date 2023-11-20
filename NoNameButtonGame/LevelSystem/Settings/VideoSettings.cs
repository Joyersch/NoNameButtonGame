using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Settings;

public class VideoSettings : ISettings
{
    public Resolution Resolution { get; set; } = new Resolution(1280, 720);

    public bool IsFixedStep { get; set; } = false;

    public bool IsFullscreen { get; set; } = false;
}