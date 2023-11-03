using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level2;

public class ColorComponentShuffler
{
    private readonly ColorComponent[] _components;
    private readonly Random _random;
    private Dictionary<int, int> _mappingTable;

    public ColorComponentShuffler(ColorComponent[] components, Random random)
    {
        _components = components;
        _random = random;
        _mappingTable = new Dictionary<int, int>();
        for (int i = 0; i < components.Length; i++)
        {
            _mappingTable.Add(i, i);
        }
    }

    public void Shuffle()
    {
        _mappingTable = new Dictionary<int, int>();
        List<int> freeMapping = new List<int>();

        for (int i = 0; i < _components.Length; i++)
            freeMapping.Add(i);

        for (int i = 0; i < _components.Length; i++)
        {
            int use = _random.Next(0, freeMapping.Count);
            _mappingTable.Add(i, freeMapping[use]);
            freeMapping.RemoveAt(use);
        }
    }

    public Color GetColor(int index)
        => _components[index].Color;

    public string GetText(int index)
        => _components[_mappingTable[index]].Text;
}