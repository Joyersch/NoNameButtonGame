using System;
using Joyersch.Monogame.Ui.Buttons;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Collision;

namespace NoNameButtonGame.Ui.Buttons;

public sealed class SampleButton : IButton
{
    private Vector2 _position;

    private readonly float _initalScale;
    private float _extendedScale = 1F;
    public float Scale => _initalScale * _extendedScale;
    private Vector2 _size;
    private Vector2 _drawingScale;
    private Microsoft.Xna.Framework.Color _color;
    private Rectangle _imageLocation;

    private MouseActionsMat _mouseMat;
    private HitboxProvider _hitbox;

    public Rectangle[] Hitbox => _hitbox.Hitbox;
    private Rectangle _rectangle;
    public Rectangle Rectangle => _rectangle;
    public float Layer { get; set; }

    public bool IsHover => _mouseMat.IsHover;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    public static Texture2D Texture;
    private static readonly Vector2 ImageSize = new Vector2(32, 16);

    public static float DefaultScale { get; set; } = 4F;

    public SampleButton() : this(Vector2.Zero)
    {
    }

    public SampleButton(Vector2 position) : this(position, DefaultScale)
    {
    }

    public SampleButton(Vector2 position, float initalScale)
    {
        _position = position;
        _initalScale = initalScale;
        _size = ImageSize * Scale;
        _drawingScale = Vector2.One * Scale;
        _color = Microsoft.Xna.Framework.Color.White;

        var hitbox = new[]
        {
            new Rectangle(2, 1, 28, 14),
            new Rectangle(1, 2, 30, 12)
        };
        _hitbox = new HitboxProvider(this, hitbox, _drawingScale);
        _rectangle = this.GetRectangle();
        _imageLocation = new Rectangle(0, 0, (int)ImageSize.X, (int)ImageSize.Y);

        _mouseMat = new MouseActionsMat(this);
        _mouseMat.Leave += _ => Leave?.Invoke(this);
        _mouseMat.Enter += _ => Enter?.Invoke(this);
        _mouseMat.Click += _ => Click?.Invoke(this);
    }

    public bool UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        bool @return = false;
        @return |= _mouseMat.UpdateInteraction(gameTime, toCheck);

        _imageLocation = new Rectangle(_mouseMat.IsHover ? (int)ImageSize.X : 0, 0,
            (int)ImageSize.X, (int)ImageSize.Y);
        return @return;
    }

    public void Update(GameTime gameTime)
    {
        _hitbox.Update(gameTime);
        _rectangle = this.GetRectangle();
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            _position,
            _imageLocation,
            _color,
            0F,
            Vector2.Zero,
            _drawingScale,
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

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
        => _color = input[0];

    public int ColorLength()
        => 1;

    public Microsoft.Xna.Framework.Color[] GetColor()
        => [_color];

    public void SetScale(ScaleProvider provider)
    {
        _extendedScale = provider.Scale;
        _size = ImageSize * Scale;
        _drawingScale = Vector2.One * Scale;
        _rectangle = this.GetRectangle();
        _hitbox.SetScale(_drawingScale);
    }
}