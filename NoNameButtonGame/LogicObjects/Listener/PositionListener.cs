using System.Collections.Generic;
using Microsoft.Xna.Framework;
using NoNameButtonGame.GameObjects;

namespace NoNameButtonGame.LogicObjects.Listener;

public class PositionListener
{
    private readonly List<(GameObject main, GameObject sub)> _mappings;

    public PositionListener()
    {
        _mappings = new List<(GameObject main, GameObject sub)>();
    }

    public void Update(GameTime gameTime)
        => _mappings.ForEach(m => m.sub.Position = m.main.Position);

    public void Add(GameObject main, GameObject sub)
        => _mappings.Add((main, sub));
}