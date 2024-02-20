using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using NoNameButtonGame.Colors;

namespace NoNameButtonGame.GameObjects;

public class Nbg : IManageable, IMoveable, ILayerable
{
    public float Scale => _scale;
    public float Layer { get; set; }

    private Vector2 _position;
    private Vector2 _size;
    private Color _color;

    private Rectangle _rectangle;
    public Rectangle Rectangle => _rectangle;

    private readonly SingleRandomColor _singleRandomColor;

    private readonly Rectangle _area;
    private readonly float _scale;

    public float Speed = 60;

    private bool MoveUp;
    private bool MoveLeft;

    public static Texture2D Texture;


    public Nbg(Rectangle area, Random random) : this(area, random, 1F)
    {
    }

    public Nbg(Rectangle area, Random random, float scale)
    {
        _size = new Vector2(21, 13) * scale;
        _position = area.Center.ToVector2() - _size / 2;
        _area = area;
        _scale = scale;
        _singleRandomColor = new SingleRandomColor(random);
        _color = _singleRandomColor.GetColor(1)[0];
    }

    public void Update(GameTime gameTime)
    {
        float by = (float)gameTime.ElapsedGameTime.TotalMilliseconds / 1000 * Speed;

        var position = _position;
        position.Y += MoveUp ? -by : by;
        position.X += MoveLeft ? -by : by;
        bool newColor = false;
        bool hasChanged = false;
        do
        {
            hasChanged = false;
            if (position.Y <= _area.Top)
            {
                var difference = _area.Top - position.Y;

                MoveUp = !MoveUp;
                newColor = true;
                hasChanged = true;


                by -= difference;
                position.Y += by * (MoveUp ? -1 : 1);
            }

            if (position.Y + _size.Y >= _area.Bottom)
            {
                var difference = _area.Bottom - (position.Y + _size.Y);

                MoveUp = !MoveUp;
                newColor = true;
                hasChanged = true;
                by -= difference;
                position.Y += by * (MoveUp ? -1 : 1);
            }

            if (position.X <= _area.Left)
            {
                var difference = _area.Left - position.X;


                MoveLeft = !MoveLeft;
                newColor = true;
                hasChanged = true;

                by -= difference;
                position.X += by * (MoveLeft ? -1 : 1);
            }

            if (position.X + _size.X >= _area.Right)
            {
                var difference = _area.Right - (position.X + _size.X);

                if (Math.Abs(difference - by) < float.Epsilon)
                    difference *= 0;

                MoveLeft = !MoveLeft;
                newColor = true;
                hasChanged = true;
                by -= difference;
                position.X += by * (MoveLeft ? -1 : 1);
            }
        } while (hasChanged);

        if (newColor)
        {
            _singleRandomColor.Update(gameTime);
            _color = _singleRandomColor.GetColor(1)[0];
        }

        Move(position);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            _position,
            null,
            _color,
            0F,
            Vector2.Zero,
            _scale,
            SpriteEffects.None,
            Layer);
    }

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        _position = newPosition;
        _rectangle = this.GetRectangle();
    }
}