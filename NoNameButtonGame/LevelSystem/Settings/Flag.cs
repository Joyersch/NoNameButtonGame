using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Logic.Text;
using MonoUtils.Ui;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Logic;

namespace NoNameButtonGame.LevelSystem.Settings;

public class Flag : IHitbox, IManageable, IMoveable, IRotateable, ILayerable, IColorable, IInteractable, IScaleable
{
    private Vector2 _position;
    private readonly float _initialScale;
    private float _extendedScale = 1F;
    public float Scale => _initialScale * _extendedScale;
    private Vector2 _size;
    private Vector2 _drawingScale;
    private Color _color;

    public readonly TextProvider.Language Language;

    public event Action<object> Click;

    private MouseActionsMat _mouseActionsMat;

    public Rectangle[] Hitbox => _hitboxProvider.Hitbox;
    private HitboxProvider _hitboxProvider;
    public Rectangle Rectangle { get; private set; }
    public float Rotation { get; set; }
    public float Layer { get; set; }

    public static Vector2 ImageSize = new Vector2(64, 32);
    public static Texture2D Texture;
    private Rectangle _imageLocation;

    public Flag(TextProvider.Language language) : this(language, Vector2.Zero, 1F)
    {
    }

    public Flag(TextProvider.Language language, float initialScale) : this(language, Vector2.Zero, initialScale)
    {
    }

    public Flag(TextProvider.Language language, Vector2 position, float initialScale)
    {
        Language = language;
        _position = position;
        _initialScale = initialScale;
        _size = ImageSize * Scale;
        _drawingScale = Vector2.One * Scale;
        _color = Color.White;
        Rectangle = new Rectangle(_position.ToPoint(), _size.ToPoint());

        var imageLocation = new Rectangle(Vector2.Zero.ToPoint(), ImageSize.ToPoint());
        _hitboxProvider = new HitboxProvider(this, new[] { imageLocation }, _drawingScale);

        imageLocation.X = (int)ImageSize.X * (int)language;
        _imageLocation = imageLocation;
        _mouseActionsMat = new MouseActionsMat(this);
        _mouseActionsMat.Click += delegate { Click?.Invoke(this); };
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _mouseActionsMat.UpdateInteraction(gameTime, toCheck);
    }


    public void Update(GameTime gameTime)
    {
        _hitboxProvider.Update(gameTime);
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        spriteBatch.Draw(
            Texture,
            _position,
            _imageLocation,
            _color,
            Rotation,
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
        => [_color];

    public void SetScale(float scale)
    {
        _extendedScale = scale;
        _size = ImageSize * Scale;
        _drawingScale = Vector2.One * Scale;
        Rectangle = this.GetRectangle();
        _hitboxProvider.SetScale(_drawingScale);
    }
}