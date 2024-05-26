using System.Collections.Generic;
using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.Settings;

public class VideoSettings : ISettings
{
    public Resolution Resolution { get; set; }

    public bool IsFixedStep { get; set; } = true;

    public bool IsFullscreen { get; set; } = true;

    public static List<Resolution> Resolutions = new List<Resolution>()
    {
        new Resolution(1280, 720),
        new Resolution(1920, 1080),
        new Resolution(2560, 1440),
        new Resolution(3840, 2160),
    };
}