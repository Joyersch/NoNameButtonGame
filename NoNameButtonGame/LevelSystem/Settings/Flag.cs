using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Logic.Text;
using MonoUtils.Ui.Color;
using MonoUtils.Ui.Logic;

namespace NoNameButtonGame.LevelSystem.Settings;

public class Flag : IHitbox, IManageable, IMoveable, IRotateable, ILayerable, IColorable, IInteractable
{
    private Vector2 _position;
    private Vector2 _size;
    private Vector2 _scale;
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

    public Flag(TextProvider.Language language, float scale) : this(language, Vector2.Zero, scale)
    {
    }

    public Flag(TextProvider.Language language, Vector2 position, float scale)
    {
        Language = language;
        _position = position;
        _size = ImageSize * scale;
        _scale = Vector2.One * scale;
        _color = Color.White;
        Rectangle = new Rectangle(_position.ToPoint(), _size.ToPoint());

        var imageLocation = new Rectangle(Vector2.Zero.ToPoint(), ImageSize.ToPoint());
        _hitboxProvider = new HitboxProvider(this, new[] { imageLocation }, _scale);

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