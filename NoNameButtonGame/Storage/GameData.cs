using System;
using NoNameButtonGame.Interfaces;
using Level6 = NoNameButtonGame.LevelSystem.LevelContainer.Level6;

namespace NoNameButtonGame.Storage;

public class GameData : IChangeable
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
}