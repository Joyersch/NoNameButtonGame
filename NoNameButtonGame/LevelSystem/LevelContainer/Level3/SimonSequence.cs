using System;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level3;

public class SimonSequence
{
    private readonly int _minimum;
    private readonly int _maximum;
    private readonly int _length;
    private readonly Random _random;

    private readonly int[] data;
    private int pointer;

    public int Pointer => pointer;

    public SimonSequence(int minimum, int maximum, int length, Random random)
    {
        _minimum = minimum;
        _maximum = maximum;
        _length = length;
        _random = random;

        data = new int[length];
        for (int i = 0; i < length; i++)
            data[i] = random.Next(minimum, maximum);
    }

    public bool Next(int limit, out int value)
    {
        value = data[pointer++];
        if (pointer < _length && pointer < limit)
            return true;
        pointer = 0;
        return false;
    }
    
    public bool Compare(int index, int value)
    {
        if (index > _length || index < 0)
            return false;
        return data[index] == value;
    }
}