using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Collision;

namespace NoNameButtonGame.Ui;

public sealed class Invisible : IManageable, IInteractable, IMouseActions, IHitbox, IMoveable
{
    private Vector2 _position;
    private readonly Vector2 _size;
    private MouseActionsMat _mouseActionsMat;
    private HitboxProvider _hitboxProvider;

    public event Action<object>? Leave;
    public event Action<object>? Enter;
    public event Action<object>? Click;

    public Rectangle[] Hitbox { get; }
    public Rectangle Rectangle { get; private set; }

    public Invisible() : this(Vector2.Zero)
    {
    }

    public Invisible(Vector2 size) : this(Vector2.Zero, size)
    {
    }

    public Invisible(Vector2 position, Vector2 size)
    {
        _position = position;
        _size = size;
        var rectangle = new[] { new Rectangle(0, 0, 1, 1) };
        _hitboxProvider = new HitboxProvider(this, rectangle, size);
        _mouseActionsMat = new MouseActionsMat(this);
        _mouseActionsMat.Click += _ => Click?.Invoke(this);
        _mouseActionsMat.Enter += _ => Enter?.Invoke(this);
        _mouseActionsMat.Leave += _ => Leave?.Invoke(this);
    }

    public bool UpdateInteraction(GameTime gameTime, IHitbox toCheck)
        => _mouseActionsMat.UpdateInteraction(gameTime, toCheck);


    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        _position = newPosition;
    }

    public void Update(GameTime gameTime)
    {
        _hitboxProvider.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
    }
}