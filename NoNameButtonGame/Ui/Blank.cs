using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame;
using NoNameButtonGame.Helpers;
using NoNameButtonGame.Ui;

namespace Joyersch.Monogame.Ui;

public class Blank : IManageable, IMoveable, IRotateable, ILayerable, IColorable, IHitbox, IScaleable
{
    private Vector2 _position;
    private Vector2 _baseSize = Vector2.One;
    private Vector2 _size;

    private float _initialScale = 1f;
    private float _extendedScale = 1f;
    public float Scale => _initialScale * _extendedScale;
    
    public Microsoft.Xna.Framework.Color Color { get; set; }

    public float Layer { get; set; }

    public float Rotation { get; set; }

    public Rectangle Rectangle { get; private set; }

    public string Identifier { get; private set; }

    public Rectangle[] Hitbox => new[] { new Rectangle(_position.ToPoint(), _size.ToPoint()) };

    public static Texture2D Texture;

    public Blank(Vector2 position, float scale = 1f, string identifier = "")
    {
        _position = position;
        _initialScale = scale;
        _size = _baseSize * _initialScale;
        Identifier = identifier;
        Color = new Microsoft.Xna.Framework.Color(0, 0, 0, 150);
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
        Rectangle = this.GetRectangle();
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
    {
        if (input.Length < 1)
            return;
        Color = input[0];
    }

    public int ColorLength()
        => 1;

    public Microsoft.Xna.Framework.Color[] GetColor()
        => [Color];

    public void SetScale(ScaleProvider provider)
    {
        _extendedScale = provider.Scale;
        _size = _baseSize * Scale;
        Rectangle = this.GetRectangle();
    }
}