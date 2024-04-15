using System;

namespace NoNameButtonGame.LevelSystem.LevelContainer.SimonSaysLevel;

public class SimonSequence
{

    private readonly int _length;

    private readonly int[] _data;
    private int _pointer;

    public int Pointer => _pointer;

    public SimonSequence(int minimum, int maximum, int length, Random random)
    {
        _length = length;

        _data = new int[length];
        for (int i = 0; i < length; i++)
            _data[i] = random.Next(minimum, maximum);
    }

    public bool Next(int limit, out int value)
    {
        value = _data[_pointer++];
        if (_pointer < _length && _pointer < limit)
            return true;
        _pointer = 0;
        return false;
    }

    public bool Compare(int index, int value)
    {
        if (index > _length || index < 0)
            return false;
        return _data[index] == value;
    }

    public int[] GetRange(int start, int length)
        => _data[new Range(start, length)];

    public int[] GetAll()
        => _data;
}