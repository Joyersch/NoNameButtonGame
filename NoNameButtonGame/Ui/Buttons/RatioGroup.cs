using System;
using System.Collections.Generic;
using NoNameButtonGame.Ui.Buttons;

namespace Joyersch.Monogame.Ui.Buttons;

public sealed class RatioGroup
{
    public event Action<int> UpdateStatus;

    private readonly List<RatioBox> _collection;

    public RatioGroup()
    {
        _collection = new List<RatioBox>();
    }

    public int Register(RatioBox box)
    {
        _collection.Add(box);
        return _collection.IndexOf(box);
    }

    public void Select(RatioBox box)
    {
        int index = _collection.IndexOf(box);
        if (index == -1)
            return;

        UpdateStatus?.Invoke(index);
    }

    public void Select(int index)
    {
        if (index < 0 || index >= _collection.Count)
            return;

        UpdateStatus?.Invoke(index);
    }
}