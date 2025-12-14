using Joyersch.Monogame;
using Joyersch.Monogame.Ui;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Collision;

namespace NoNameButtonGame.Ui;

public class SampleObject : IHitbox, IManageable, IMoveable, IRotateable, ILayerable, IColorable
{
    private Vector2 _position;
    private Vector2 _size;
    private Vector2 _scale;
    private Microsoft.Xna.Framework.Color _color;

    private readonly AnimationProvider _animation;

    private readonly HitboxProvider _hitbox;

    public float Layer { get; set; }

    public float Rotation { get; set; }

    public Rectangle Rectangle { get; private set; }

    public static Texture2D Texture;

    public Rectangle[] Hitbox => _hitbox.Hitbox;


    public SampleObject() : this(Vector2.Zero, Vector2.Zero)
    {
    }

    public SampleObject(Vector2 position) : this(position, Vector2.Zero)
    {
    }

    public SampleObject(Vector2 position, Vector2 size)
    {
        var smallestSize = new Vector2(16, 16);
        var smallestHitbox = new[]
        {
            new Rectangle(0, 0, 16, 16)
        };

        _position = position;
        _size = size;
        _scale = _size / smallestSize;
        _color = Microsoft.Xna.Framework.Color.White;

        _animation = new AnimationProvider(smallestSize, 125D, 4);
        _hitbox = new HitboxProvider(this, smallestHitbox, _scale);

        Rectangle = new Rectangle(_position.ToPoint(), _size.ToPoint());
    }

    public virtual void Update(GameTime gameTime)
    {
        _hitbox.Update(gameTime);
        _animation.Update(gameTime);
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

    public void ChangeColor(Microsoft.Xna.Framework.Color[] input)
    {
        if (input.Length < 1)
            return;
        _color = input[0];
    }

    public int ColorLength()
        => 1;

    public Microsoft.Xna.Framework.Color[] GetColor()
        => new[] { _color };
}