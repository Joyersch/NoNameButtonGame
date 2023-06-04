using System;
using MonoUtils.Logic;
using Level2 = NoNameButtonGame.LevelSystem.LevelContainer.Level2;

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

    public Level2.StorageData Level2;

    public GameData()
    {
        Level2 = new Level2.StorageData();
        Level2.HasChanged += (_, _) => HasChanged?.Invoke(this, EventArgs.Empty);
    }

    public event EventHandler HasChanged;
    public string Name => nameof(GameData);
    public void Load(string path)
    {
        var copy = (GameData) FileManager.LoadFile($"{path}/{Name}.json", typeof(GameData));
        MaxLevel = copy.MaxLevel;
        Level2 = copy.Level2;
    }

    public void Save(string path)
        => FileManager.SaveFile($"{path}/{Name}.json", this);

    public object Get()
        => this;
}