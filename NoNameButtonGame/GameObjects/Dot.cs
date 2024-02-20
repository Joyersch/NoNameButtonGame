using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;

namespace NoNameButtonGame.GameObjects;

public class Dot : IManageable, IMoveable, IRotateable, ILayerable, IColorable
{
    private Vector2 _position;
    private Vector2 _size;
    private Color _color;

    public float Layer { get; set; }

    public float Rotation { get; set; }

    public Rectangle Rectangle { get; private set; }

    private Vector2 ImageSize = new Vector2(1, 1);

    public new static Texture2D Texture;

    public Dot(Vector2 position, Vector2 size)
    {
        _position = position;
        _size = size;
        _color = new Color(0,0,0, 150);
        Rectangle = this.GetRectangle();
    }

    public void Update(GameTime gameTime)
    {

    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            _position,
            null,
            _color,
            Rotation,
            Vector2.Zero,
            _size,
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
        Rectangle = this.GetRectangle();
    }

    public void ChangeColor(Color[] input)
    {
        if (input.Length < 1)
            return;
        _color = input[0];
    }

    public int ColorLength()
        => 1;

    public Color[] GetColor()
        => new[] { _color };
}