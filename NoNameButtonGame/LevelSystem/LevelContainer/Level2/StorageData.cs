using System;
using MonoUtils.Logic;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level2;

public class StorageData : IChangeable
{
    public event EventHandler HasChanged;

    private long _beans;

    public long Beans
    {
        get => _beans;
        set
        {
            _beans = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _upgrade1;
    public int Upgrade1
    {
        get => _upgrade1;
        set
        {
            _upgrade1 = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _upgrade2;
    public int Upgrade2
    {
        get => _upgrade2;
        set
        {
            _upgrade2 = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private int _upgrade3;
    public int Upgrade3
    {
        get => _upgrade3;
        set
        {
            _upgrade3 = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    
    private int _upgrade4;
    public int Upgrade4
    {
        get => _upgrade4;
        set
        {
            _upgrade4 = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private bool _snake;
    public bool Snake
    {
        get => _snake;
        set
        {
            _snake = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}