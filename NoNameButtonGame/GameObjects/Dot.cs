using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects;

namespace NoNameButtonGame.GameObjects;

public class Dot : IManageable, IMoveable, IRotateable, ILayerable, IColorable, IHitbox
{
    private Vector2 _position;
    private Vector2 _size;

    public Color Color { get; set; }

    public float Layer { get; set; }

    public float Rotation { get; set; }

    public Rectangle Rectangle { get; private set; }

    public string Identifier { get; private set; }

    public Rectangle[] Hitbox => new[] { new Rectangle(_position.ToPoint(), _size.ToPoint()) };

    public static Texture2D Texture;

    public Dot(Vector2 position, Vector2 size, string identifier = "")
    {
        _position = position;
        _size = size;
        Identifier = identifier;
        Color = new Color(0, 0, 0, 150);
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
            Color,
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
        Color = input[0];
    }

    public int ColorLength()
        => 1;

    public Color[] GetColor()
        => new[] { Color };
}