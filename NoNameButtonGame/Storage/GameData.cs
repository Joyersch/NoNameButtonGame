using System;
using NoNameButtonGame.Interfaces;

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

    private int _currentBackedBeans;
    public int CurrentBackedBeans
    {
        get => _currentBackedBeans;
        set
        {
            _currentBackedBeans = value;
            HasChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    public event EventHandler HasChanged;
}