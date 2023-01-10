using System;
using System.Dynamic;
using System.IO;
using Newtonsoft.Json;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame;

public class Storage : IChangeable
{
    private string BasePath;

    public event EventHandler HasChanged;

    public Storage(string path)
    {
        BasePath = path;
        Settings = new();
        Settings.HasChanged += HasChanged;
        GameData = new();
        GameData.HasChanged += HasChanged;
    }

    public Settings Settings;
    
    public GameData GameData;
    
    public void Load()
    {
        using StreamReader settingsReader = new($"{BasePath}/{nameof(Settings)}.json");
        Settings = JsonConvert.DeserializeObject<Settings>(settingsReader.ReadToEnd());

        using StreamReader gameDataReader = new($"{BasePath}/{nameof(GameData)}.json");
        GameData = JsonConvert.DeserializeObject<GameData>(gameDataReader.ReadToEnd());
    }

    public void Save()
    {
        using StreamWriter settingsWriter = new($"{BasePath}/{nameof(Settings)}.json");
        settingsWriter.Write(JsonConvert.SerializeObject(Settings));

        using StreamWriter gameDataWriter = new($"{BasePath}/{nameof(GameData)}.json");
        gameDataWriter.Write(JsonConvert.SerializeObject(GameData));
    }
}