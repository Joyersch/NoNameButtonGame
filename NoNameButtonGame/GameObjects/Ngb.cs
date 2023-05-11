using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects;

public class Nbg : GameObject, IManageable
{
    public new static Vector2 DefaultSize => DefaultMapping.ImageSize;
    public new static Texture2D DefaultTexture;

    public float Speed = 60;

    private bool MoveUp;
    private bool MoveLeft;

    private SingleRandomColor _singleRandomColor;

    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(24, 16),
        Hitboxes = new[]
        {
            new Rectangle(1, 2, 21, 13)
        }
    };

    private readonly Rectangle _area;

    public Nbg(Rectangle area, Random random) : this(area, random, 1F)
    {
    }

    public Nbg(Rectangle area, Random random, float scale) : base(area.Center.ToVector2(), DefaultSize * scale,
        DefaultTexture,
        DefaultMapping)
    {
        _area = area;
        _singleRandomColor = new SingleRandomColor(random);
    }

    public override void Update(GameTime gameTime)
    {
        float by = (float) gameTime.ElapsedGameTime.TotalMilliseconds / 1000 * Speed;

        Position.Y += MoveUp ? -by : by;
        Position.X += MoveLeft ? -by : by;
        if (Position.Y <= _area.Top ||
            Position.Y + Size.Y >= _area.Bottom)
        {
            MoveUp = !MoveUp;
            SetNewColor(gameTime);
        }
        if (Position.X <= _area.Left ||
            Position.X + Size.X >= _area.Right)
        {
            MoveLeft = !MoveLeft;
            SetNewColor(gameTime);
        }
        base.Update(gameTime);
    }

    private void SetNewColor(GameTime gameTime)
    {
        _singleRandomColor.Update(gameTime);
        DrawColor = _singleRandomColor.GetColor(1)[0];
    }
}