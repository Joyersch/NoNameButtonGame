using System;
using MonoUtils.Logic;
using MonoUtils.Settings;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level11;

public class LevelSettings : ISettings
{
    public long Beans { get; set; }

    public int Upgrade1 { get; set; }
    public int Upgrade2 { get; set; }
    public int Upgrade3 { get; set; }
    public int Upgrade4 { get; set; }

    public bool CanSeeDistraction { get; set; }
}