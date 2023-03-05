using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.LogicObjects.Listener;

public class PositionListener
{
    private readonly List<(IMoveable main, IMoveable sub)> _mappings;

    public PositionListener()
    {
        _mappings = new List<(IMoveable main, IMoveable sub)>();
    }

    public void Update(GameTime gameTime)
        => _mappings.ForEach(m => m.sub.Move(m.main.GetPosition()));

    public void Add(IMoveable main, IMoveable sub)
        => _mappings.Add((main, sub));
}