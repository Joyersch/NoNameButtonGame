using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace NoNameButtonGame.Listener;

public sealed class PositionListener
{
    private readonly List<(IMoveable main, IMoveable sub, Vector2 offset)> _mappings;

    public PositionListener()
    {
        _mappings = new List<(IMoveable main, IMoveable sub, Vector2 offset)>();
    }

    public void Update(GameTime gameTime)
        => _mappings.ForEach(m => m.sub.Move(m.main.GetPosition() + m.offset));

    public void Add(IMoveable main, IMoveable sub, Vector2 offset)
        => _mappings.Add((main, sub, offset));

    public void Add(IMoveable main, IMoveable sub)
        => Add(main, sub, sub.GetPosition() - main.GetPosition());
}