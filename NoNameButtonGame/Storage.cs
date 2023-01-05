using System;
using System.IO;
using Newtonsoft.Json;

namespace NoNameButtonGame;

public class Storage
{
    public string BasePath;

    public Storage(string path)
    {
        BasePath = path;
        Settings = new ();
        GameData = new ();
    }
    public SettingsClass Settings;

    public class SettingsClass
    {
        public bool IsFixedStep { get; set; }
        public bool IsFullscreen { get; set; }
    }

    public GameDataClass GameData;

    public class GameDataClass
    {
        public int MaxLevel { get; set; }
    }

    public void Load()
    {
        using StreamReader settingsReader = new StreamReader(string.Format("{0}/{1}", BasePath, nameof(Settings)));
        Settings = JsonConvert.DeserializeObject<SettingsClass>(settingsReader.ReadToEnd());

        using StreamReader gameDataReader = new StreamReader(string.Format("{0}/{1}", BasePath, nameof(GameData)));
        GameData = JsonConvert.DeserializeObject<GameDataClass>(gameDataReader.ReadToEnd());
    }

    public void Save()
    {
        using StreamWriter settingsWriter = new StreamWriter(string.Format("{0}/{1}", BasePath, nameof(Settings)));
        settingsWriter.Write(JsonConvert.SerializeObject(Settings));

        using StreamWriter gameDataWriter = new StreamWriter(string.Format("{0}/{1}", BasePath, nameof(GameData)));
        settingsWriter.Write(JsonConvert.SerializeObject(GameData));
    }
}