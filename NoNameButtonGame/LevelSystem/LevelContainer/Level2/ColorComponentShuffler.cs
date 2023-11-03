using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level2;

public class ColorComponentShuffler
{
    private readonly ColorComponent[] _components;
    private readonly Random _random;
    private Dictionary<int, int> _mappingTableColor;
    private Dictionary<int, int> _mappingTableText;

    public ColorComponentShuffler(ColorComponent[] components, Random random)
    {
        _components = components;
        _random = random;
        _mappingTableColor = new Dictionary<int, int>();
        _mappingTableText = new Dictionary<int, int>();
        for (int i = 0; i < components.Length; i++)
        {
            _mappingTableColor.Add(i, i);
            _mappingTableText.Add(i, i);
        }
    }

    public void Shuffle()
    {
        _mappingTableColor = new Dictionary<int, int>();
        _mappingTableText = new Dictionary<int, int>();

        List<int> freeMapping = new List<int>();

        for (int i = 0; i < _components.Length; i++)
            freeMapping.Add(i);

        for (int i = 0; i < _components.Length; i++)
        {
            int use = _random.Next(0, freeMapping.Count);
            _mappingTableColor.Add(i, freeMapping[use]);
            freeMapping.RemoveAt(use);
        }

        for (int i = 0; i < _components.Length; i++)
            freeMapping.Add(i);

        for (int i = 0; i < _components.Length; i++)
        {
            int use = _random.Next(0, freeMapping.Count);
            _mappingTableText.Add(i, freeMapping[use]);
            freeMapping.RemoveAt(use);
        }
    }

    public Color GetColor(int index)
        => _components[_mappingTableColor[index]].Color;

    public string GetText(int index)
        => _components[_mappingTableText[index]].Text;

    public int ResolveColor(int index)
        => _mappingTableColor.First(k => k.Value == index).Key;
    public int ResolveText(int index)
        => _mappingTableText.First(k => k.Value == index).Key;
}