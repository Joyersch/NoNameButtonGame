using System;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame;

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

    public event EventHandler HasChanged;
}