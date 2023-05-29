using System;
using MonoUtils.Logic;
using Newtonsoft.Json;
using Level6 = NoNameButtonGame.LevelSystem.LevelContainer.Level6;

namespace NoNameButtonGame.Storage;

public class GameData : IChangeable, ISettings
{
    private int _maxLevel;

    public int MaxLevel
    {
        get => _maxLevel;
        set
        {
            _maxLevel = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public Level6.StorageData Level6;

    public GameData()
    {
        Level6 = new Level6.StorageData();
        Level6.HasChanged += (_, _) => HasChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler HasChanged;
    public string Name => nameof(GameData);
    public void Load(string path)
    {
        var copy = (GameData) FileManager.LoadFile($"{path}/{Name}.json", typeof(GameData));
        MaxLevel = copy.MaxLevel;
        Level6 = copy.Level6;
    }

    public void Save(string path)
        => FileManager.SaveFile($"{path}/{Name}.json", this);

    public object Get()
        => this;
}