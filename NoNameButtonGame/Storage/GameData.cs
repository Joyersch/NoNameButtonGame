using System;
using MonoUtils.Logic;
using MonoUtils.Settings;
using Level11 = NoNameButtonGame.LevelSystem.LevelContainer.Level11;

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

    public Level11.StorageData Level11;

    public GameData()
    {
        Level11 = new Level11.StorageData();
        Level11.HasChanged += (_, _) => HasChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler HasChanged;
    public string Name => nameof(GameData);
    public void Load(string path)
    {
        var copy = (GameData) FileManager.LoadFile($"{path}/{Name}.json", typeof(GameData));
        MaxLevel = copy.MaxLevel;
        Level11 = copy.Level11;
    }

    public void Save(string path)
        => FileManager.SaveFile($"{path}/{Name}.json", this);

    public object Get()
        => this;
}