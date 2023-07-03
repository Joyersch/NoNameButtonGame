using System;
using MonoUtils.Logic;
using MonoUtils.Settings;

namespace NoNameButtonGame.Storage;

public class Storage : IChangeable
{
    public event EventHandler HasChanged;

    public Storage()
    {
        GameData = SettingsManager.Get<GameData>();
        GameData.HasChanged +=(_,_) => HasChanged?.Invoke(GameData, EventArgs.Empty);
        Settings = SettingsManager.Get<GeneralSettings>();
        Settings.HasChanged += (_,_) => HasChanged?.Invoke(Settings, EventArgs.Empty);
    }

    public GeneralSettings Settings;
    public GameData GameData;
    
    public void Load()
    {
        SettingsManager.Load();
    }

    public void Save()
    {
        SettingsManager.Save();
    }

    /*
    public void SetDefaults()
    {
        Settings.Resolution.Width = 1280;
        Settings.Resolution.Height = 720;
        Settings.IsFullscreen = false;
        Settings.IsFixedStep = true;
        Settings.MusicVolume = 5;
        Settings.SfxVolume = 5;
    }
    */
}