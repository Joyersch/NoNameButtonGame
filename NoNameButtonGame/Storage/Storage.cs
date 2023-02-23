using System;
using System.IO;
using Newtonsoft.Json;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.Storage;

public class Storage : IChangeable
{
    private readonly string _basePath;

    public event EventHandler HasChanged;

    public Storage(string path)
    {
        _basePath = path;
        Settings = new Settings();
        Settings.HasChanged += HasChanged;
        GameData = new GameData();
        GameData.HasChanged += HasChanged;
    }

    public Settings Settings;
    
    public GameData GameData;
    
    public bool Load()
    {
        string path = $"{_basePath}/{nameof(Settings)}.json";
        
        // If file does not exits default settings have to be loaded.
        // This only matters for settings as game-data does not need to have a value
        if (!File.Exists(path))
            return false;
        
        try
        {
            using StreamReader settingsReader = new(path);
            Settings = JsonConvert.DeserializeObject<Settings>(settingsReader.ReadToEnd());
        }
        catch
        {
            return false;
        }

        try
        {
            using StreamReader gameDataReader = new($"{_basePath}/{nameof(GameData)}.json");
            GameData = JsonConvert.DeserializeObject<GameData>(gameDataReader.ReadToEnd());
        }
        catch { } // can be ignored as empty game-data is handleable
        return true;
    }

    public void Save()
    {
        using StreamWriter settingsWriter = new($"{_basePath}/{nameof(Settings)}.json");
        settingsWriter.Write(JsonConvert.SerializeObject(Settings));

        using StreamWriter gameDataWriter = new($"{_basePath}/{nameof(GameData)}.json");
        gameDataWriter.Write(JsonConvert.SerializeObject(GameData));
    }

    public void SetDefaults()
    {
        Settings.Resolution.Width = 1280;
        Settings.Resolution.Height = 720;
        Settings.IsFullscreen = false;
        Settings.IsFixedStep = true;
        Settings.MusicVolume = 5;
        Settings.SfxVolume = 5;
    }
}