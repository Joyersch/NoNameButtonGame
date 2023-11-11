using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logic;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level12.Overworld;

public class Human : GameObject
{
    private readonly Random _random;
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;
    public new static Texture2D DefaultTexture;
    private OverTimeInvoker _invoker;
    private Rectangle _area;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(16, 16),
        Hitboxes = new[]
        {
            new Rectangle(1, 2, 21, 13)
        }
    };

    public override void Update(GameTime gameTime)
    {
        _invoker.Update(gameTime);
        base.Update(gameTime);
    }

    public Human(Vector2 position, float scale, Random random) : base(position, DefaultSize * scale,
        DefaultTexture,
        DefaultMapping)
    {
        _random = random;
        _area = new Rectangle((int) position.X - 10, (int) position.Y - 10, 20, 20);
        _invoker = new OverTimeInvoker(75);
        _invoker.Trigger += MoveAround;
        DrawColor = Color.Pink;
    }

    private void MoveAround()
    {
        Vector2 potentialPosition = Vector2.Zero;
        do
        {
            potentialPosition = Position + new Vector2(_random.Next(-1, 2), _random.Next(-1, 2));
        } while (!_area.Intersects(new Rectangle(potentialPosition.ToPoint(), new Point(1, 1))));

        Move(potentialPosition);
    }
}