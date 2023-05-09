using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Colors;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects;

public class NBG : GameObject, IManageable
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

    public NBG(Rectangle area, Random random) : this(area, random, Vector2.Zero)
    {
    }

    public NBG(Rectangle area, Random random, float scale) : this(area, random, Vector2.Zero, scale)
    {
    }

    public NBG(Rectangle area, Random random, Vector2 position) : this(area, random, position, 1F)
    {
    }

    public NBG(Rectangle area, Random random, Vector2 position, float scale) : base(position, DefaultSize * scale,
        DefaultTexture,
        DefaultMapping)
    {
        _area = area;
        if (!Rectangle.Intersects(area))
            Position = area.Location.ToVector2();
        _singleRandomColor = new SingleRandomColor(random);
    }

    public override void Update(GameTime gameTime)
    {
        float by = (float) gameTime.ElapsedGameTime.TotalMilliseconds / 1000 * Speed;
        if (MoveUp)
        {
            Position.Y -= by;
            if (Position.Y <= _area.Top)
            {
                MoveUp = false;
                SetNewColor(gameTime);
            }
        }
        else
        {
            Position.Y += by;
            if (Position.Y + Size.Y >= _area.Bottom)
            {
                MoveUp = true;
                SetNewColor(gameTime);
            }
        }

        if (MoveLeft)
        {
            Position.X -= by;
            if (Position.X <= _area.Left)
            {
                MoveLeft = false;
                SetNewColor(gameTime);
            }
        }
        else
        {
            Position.X += by;

            if (Position.X + Size.X >= _area.Right)
            {
                MoveLeft = true;
                SetNewColor(gameTime);
            }
        }

        base.Update(gameTime);
    }

    private void SetNewColor(GameTime gameTime)
    {
        _singleRandomColor.Update(gameTime);
        DrawColor = _singleRandomColor.GetColor(1)[0];
    }
}