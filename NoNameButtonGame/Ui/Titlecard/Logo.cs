using Joyersch.Monogame;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame_IDrawable = NoNameButtonGame.IDrawable;

namespace NoNameButtonGame.Ui.Titlecard;

public class Logo : NoNameButtonGame_IDrawable, IColorable, IMoveable, IScaleable
{
    public static Texture2D Texture;

    private Microsoft.Xna.Framework.Color _color;
    private readonly float _baseScale;
    private float _extendedScale;

    private Vector2 _position;

    private Vector2 BaseSize => new Vector2(16, 16);
    public float Scale => _baseScale * _extendedScale;
    private Vector2 _size;

    public Rectangle Rectangle { private set; get; }

    public Logo(float baseScale)
    {
        _baseScale = baseScale;
        _size = BaseSize * Scale;
        Rectangle = new(_position.ToPoint(), _size.ToPoint());
        _color = Microsoft.Xna.Framework.Color.White;
    }

    public static void LoadContent(ContentManager content)
    {
        Texture = content.GetTexture("Titlecard/Logo");
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            _position,
            null,
            _color,
            0,
            Vector2.Zero,
            Scale,
            SpriteEffects.None,
            0);
    }

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
        => _color = input[0];

    public int ColorLength()
        => 1;

    public Microsoft.Xna.Framework.Color[] GetColor()
        => [_color];

    public Vector2 GetPosition() => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        _position = newPosition;
    }

    
    public void SetScale(ScaleProvider provider)
    {
        _extendedScale = provider.Scale;
        _size = BaseSize * Scale;
        Rectangle = new(_position.ToPoint(), _size.ToPoint());
    }
}