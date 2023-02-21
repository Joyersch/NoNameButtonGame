using System;
using System.IO;
using Newtonsoft.Json;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.Storage;

public class Storage : IChangeable
{
    private string basePath;

    public event EventHandler HasChanged;

    public Storage(string path)
    {
        basePath = path;
        Settings = new Settings();
        Settings.HasChanged += HasChanged;
        GameData = new GameData();
        GameData.HasChanged += HasChanged;
    }

    public Settings Settings;
    
    public GameData GameData;
    
    public void Load()
    {
        using StreamReader settingsReader = new($"{basePath}/{nameof(Settings)}.json");
        Settings = JsonConvert.DeserializeObject<Settings>(settingsReader.ReadToEnd());

        using StreamReader gameDataReader = new($"{basePath}/{nameof(GameData)}.json");
        GameData = JsonConvert.DeserializeObject<GameData>(gameDataReader.ReadToEnd());
    }

    public void Save()
    {
        using StreamWriter settingsWriter = new($"{basePath}/{nameof(Settings)}.json");
        settingsWriter.Write(JsonConvert.SerializeObject(Settings));

        using StreamWriter gameDataWriter = new($"{basePath}/{nameof(GameData)}.json");
        gameDataWriter.Write(JsonConvert.SerializeObject(GameData));
    }
}