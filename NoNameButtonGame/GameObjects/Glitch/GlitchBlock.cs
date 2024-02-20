using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using MonoUtils.Logic;
using MonoUtils.Ui.Objects;
using MonoUtils;
using MonoUtils.Logging;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Logic;

namespace NoNameButtonGame.GameObjects.Glitch;

internal class GlitchBlock : IHitbox, IManageable, IMoveable, IRotateable, ILayerable, IColorable, IInteractable,
    IMouseActions
{
    private Vector2 _position;
    private Vector2 _size;
    private Vector2 _scale;
    private Color _color;

    public float Layer { get; set; }

    public float Rotation { get; set; }

    public Rectangle Rectangle { get; private set; }

    public Rectangle[] Hitbox => _hitbox.Hitbox;

    private readonly AnimationProvider _animation;
    private readonly HitboxProvider _hitbox;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    private MouseActionsMat _mouseActionsMat;

    public new static Texture2D Texture;

    public static Color Color = new Color(181, 54, 54);
    public static Vector2 ImageSize = new Vector2(8, 8);

    public GlitchBlock(Vector2 position) : this(position, Vector2.One)
    {
    }

    public GlitchBlock(Vector2 position, float scale) : this(position, ImageSize * scale)
    {
    }

    public GlitchBlock(Vector2 position, Vector2 size) : this(position, size, ImageSize)
    {
    }

    public GlitchBlock(Vector2 position, Vector2 size, Vector2 fullSize)
    {
        _position = position;
        _size = size;
        var scaleToImage = size / ImageSize;
        var max = Math.Max(scaleToImage.X, scaleToImage.Y);
        _scale = fullSize / ImageSize;
        var framePosition = new Rectangle(Vector2.Zero.ToPoint(), (size / max).ToPoint());
        _mouseActionsMat = new MouseActionsMat(this);
        _mouseActionsMat.Leave += delegate { Leave?.Invoke(this); };
        _mouseActionsMat.Enter += delegate { Enter?.Invoke(this); };
        _mouseActionsMat.Click += delegate { Click?.Invoke(this); };

        _color = Color;

        _animation = new AnimationProvider(ImageSize, 128, 32, framePosition);
        var hitbox = new[] { new Rectangle(Vector2.Zero.ToPoint(), ImageSize.ToPoint()) };
        _hitbox = new HitboxProvider(this, hitbox, _scale);

        Rectangle = new Rectangle(_position.ToPoint(), _size.ToPoint());
    }

    public void Update(GameTime gameTime)
    {
        _hitbox.Update(gameTime);
        _animation.Update(gameTime);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _mouseActionsMat.UpdateInteraction(gameTime, toCheck);
    }

    public virtual void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            _position,
            _animation.ImageLocation,
            _color,
            Rotation,
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
        Rectangle = new Rectangle(_position.ToPoint(), _size.ToPoint());
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