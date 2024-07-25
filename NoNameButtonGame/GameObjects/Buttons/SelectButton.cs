using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Helper;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Buttons;

namespace NoNameButtonGame.GameObjects.Buttons;

public class SelectButton : IButton
{
    private Vector2 _position;
    private readonly Vector2 _size;
    private readonly Vector2 _scale;
    private Color _color;
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
    private static readonly Vector2 ImageSize = new Vector2(16, 8);

    public SelectButton() : this(Vector2.Zero)
    {
    }

    public SelectButton(Vector2 position) : this(position, 4F)
    {
    }

    public SelectButton(Vector2 position, float scale)
    {
        _position = position;
        _size = ImageSize * scale;
        _scale = Vector2.One * scale;
        _color = Color.White;

        var hitbox = new[]
        {
            new Rectangle(1, 0, 14, 8),
            new Rectangle(0, 1, 16, 6)
        };
        _hitbox = new HitboxProvider(this, hitbox, _scale);
        _rectangle = this.GetRectangle();

        _mouseMat = new MouseActionsMat(this);
        _mouseMat.Leave += _ => Leave?.Invoke(this);
        _mouseMat.Enter += _ => Enter?.Invoke(this);
        _mouseMat.Click += _ => Click?.Invoke(this);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _mouseMat.UpdateInteraction(gameTime, toCheck);

        _imageLocation = new Rectangle(_mouseMat.IsHover ? (int)ImageSize.X : 0, 0,
            (int)ImageSize.X, (int)ImageSize.Y);
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

    public void ChangeColor(Color[] input)
    {
        _color = input[0];
    }

    public int ColorLength()
        => 1;

    public Color[] GetColor()
        => new[] { _color };
}