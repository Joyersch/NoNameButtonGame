using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui;
using NoNameButtonGame.Colors;

namespace NoNameButtonGame.LevelSystem.LevelContainer.CookieClickerLevel;

public class Nbg : IManageable, IMoveable, ILayerable, IScaleable
{
    public float Scale => _baseScale * _extendedScale;

    public float Layer { get; set; }

    private Vector2 _position;
    private Vector2 _size;
    private Color _color;

    private Rectangle _rectangle;
    public Rectangle Rectangle => _rectangle;

    private readonly SingleRandomColor _singleRandomColor;

    private readonly Rectangle _area;
    private readonly float _baseScale;
    private float _extendedScale;

    public float Speed = 40;

    private bool _moveUp;
    private bool _moveLeft;

    public static Texture2D Texture;
    private Vector2 baseSize = new Vector2(21, 13);


    public Nbg(Rectangle area, Random random) : this(area, random, 1F)
    {
    }

    public Nbg(Rectangle area, Random random, float baseScale)
    {
        _size =  baseSize * baseScale;
        _position = area.Center.ToVector2() - _size / 2;
        _area = area;
        _baseScale = baseScale;
        _singleRandomColor = new SingleRandomColor(random);
        _color = _singleRandomColor.GetColor(1)[0];
    }

    public void Update(GameTime gameTime)
    {
        float moveBy = (float)(gameTime.ElapsedGameTime.TotalSeconds * Speed * Scale);

        var position = _position;
        position.Y += _moveUp ? -moveBy : moveBy;
        position.X += _moveLeft ? -moveBy : moveBy;
        bool newColor = false;
        bool hasChanged;
        do
        {
            hasChanged = false;
            if (position.Y <= _area.Top)
            {
                var difference = _area.Top - position.Y;

                _moveUp = !_moveUp;
                newColor = true;
                hasChanged = true;


                moveBy -= difference;
                position.Y += moveBy * (_moveUp ? -1 : 1);
            }

            if (position.Y + _size.Y >= _area.Bottom)
            {
                var difference = _area.Bottom - (position.Y + _size.Y);

                _moveUp = !_moveUp;
                newColor = true;
                hasChanged = true;
                moveBy -= difference;
                position.Y += moveBy * (_moveUp ? -1 : 1);
            }

            if (position.X <= _area.Left)
            {
                var difference = _area.Left - position.X;


                _moveLeft = !_moveLeft;
                newColor = true;
                hasChanged = true;

                moveBy -= difference;
                position.X += moveBy * (_moveLeft ? -1 : 1);
            }

            if (position.X + _size.X >= _area.Right)
            {
                var difference = _area.Right - (position.X + _size.X);

                if (Math.Abs(difference - moveBy) < float.Epsilon)
                    difference *= 0;

                _moveLeft = !_moveLeft;
                newColor = true;
                hasChanged = true;
                moveBy -= difference;
                position.X += moveBy * (_moveLeft ? -1 : 1);
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
            Scale,
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
    
    public void SetScale(float scale)
    {
        _extendedScale = scale;
        _size =  baseSize * Scale;
        _rectangle = this.GetRectangle();
    }
}