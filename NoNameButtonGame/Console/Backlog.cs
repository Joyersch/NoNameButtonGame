using System;
using System.Collections.Generic;
using Joyersch.Monogame.Console;

namespace NoNameButtonGame.Console;

public sealed class Backlog
{
    private readonly List<BacklogRow> _backlog;
    private int _pointer;

    public int Count => _backlog.Count;

    public int Pointer => _pointer;

    public BacklogRow this[int i] => _backlog[i];

    public Backlog()
    {
        _backlog = new();
    }

    public void MovePointerUp()
        => _pointer = _pointer-- > 0 ? _pointer : 0;

    public void MovePointerDown()
        => _pointer = _pointer++ < _backlog.Count ? _pointer : _backlog.Count;
    
    public List<BacklogRow> GetRangeFromPointer(int size)
    {
        int endIndex = Math.Min(_pointer + size, _backlog.Count);
        int startIndex = Math.Min(_pointer, endIndex);
        return _backlog.GetRange(startIndex, endIndex - startIndex);
    }

    public void Add(BacklogRow row)
    {
        _backlog.Add(row);
    }

    public void AddRange(IEnumerable<BacklogRow> rowCollection)
    {
        foreach (BacklogRow row in rowCollection)
            Add(row);
    }

    public void Clear()
    {
        _backlog.Clear();
        _pointer = 0;
    }
}