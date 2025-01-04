using System.Collections.Generic;
using Joyersch.Monogame.Storage;

namespace NoNameButtonGame.LevelSystem.Settings;

public class VideoSettings : ISettings
{
    public Resolution Resolution { get; set; }

    public bool IsFixedStep { get; set; } = true;

    public bool IsFullscreen { get; set; } = true;

    public static List<Resolution> Resolutions =
    [
        new(1280, 720),
        new(1920, 1080),
        new(2560, 1440),
        new(3840, 2160)
    ];
}