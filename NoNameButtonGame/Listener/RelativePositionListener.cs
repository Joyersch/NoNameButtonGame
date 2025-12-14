using System.Collections.Generic;
using Joyersch.Monogame.Listener;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace NoNameButtonGame.Listener;

public class RelativePositionListener : IManageable
{
    private readonly List<PositionData> _mappings;

    public RelativePositionListener()
    {
        _mappings = new List<PositionData>();
    }

    public Rectangle Rectangle => Rectangle.Empty;

    public void Update(GameTime gameTime)
    {
        foreach (var valueTuple in _mappings)
        {
            var position = valueTuple.Main.GetPosition();
            if (position != valueTuple.OldPosition)
            {
                var subPosition = valueTuple.Sub.GetPosition();
                var newPosition = subPosition + (position - valueTuple.OldPosition);
                valueTuple.Sub.Move(newPosition);
            }

            valueTuple.OldPosition = position;
        }
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }

    public void Add(IMoveable main, IMoveable sub)
        => _mappings.Add(new PositionData(main, main.GetPosition(), sub));
}