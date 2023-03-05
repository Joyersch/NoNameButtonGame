using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.GameObjects.Texture;

namespace NoNameButtonGame.GameObjects;

internal class GlitchBlock : GameObject, IMouseActions, IColorable, IMoveable
{
    private int _framePosition;
    private readonly int _frameMaximum;
    private const int FrameSpeed = 1000 / 64 * 2;

    private float _savedGameTime;

    private bool _hover;

    public event Action<object> Enter;
    public event Action<object> Leave;
    public event Action<object> Click;

    public new static Vector2 DefaultSize = DefaultMapping.ImageSize * 2;
    
    public new static Texture2D DefaultTexture;
    public new static TextureHitboxMapping DefaultMapping => new TextureHitboxMapping()
    {
        ImageSize = new Vector2(16, 16),
        Hitboxes = new[]
        {
            new Rectangle(0, 0, 16, 16)
        },
        AnimationsFrames = 64,
        AnimationFromTop = true
    };

    public GlitchBlock(Vector2 position) : this(position, DefaultSize)
    {
    }

    public GlitchBlock(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public GlitchBlock(Vector2 position, Vector2 size) : base(position, size, DefaultTexture, DefaultMapping)
    {
        var initialScale = size / DefaultMapping.ImageSize;
        _frameMaximum = TextureHitboxMapping.AnimationsFrames;
        FrameSize = initialScale / (initialScale.X > initialScale.Y ? initialScale.X : initialScale.Y) *
                    FrameSize;
    }

    public void Update(GameTime gameTime, Rectangle mousePosition)
    {
        Mouse.GetState();
        _savedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        while (_savedGameTime > FrameSpeed)
        {
            _savedGameTime -= FrameSpeed;
            _framePosition++;
            if (_framePosition == _frameMaximum) _framePosition = 0;
            ImageLocation = new Rectangle(
                !TextureHitboxMapping.AnimationFromTop ?? false
                    ? _framePosition * (int) TextureHitboxMapping.ImageSize.Y
                    : 0
                , TextureHitboxMapping.AnimationFromTop ?? true
                    ? _framePosition * (int) TextureHitboxMapping.ImageSize.X
                    : 0
                , (int) FrameSize.X, (int) FrameSize.Y);
        }

        if (HitboxCheck(mousePosition))
        {
            if (!_hover)
                Enter?.Invoke(this);
            _hover = true;
        }
        else if (_hover)
        {
            Leave?.Invoke(this);
            _hover = false;
        }

        base.Update(gameTime);
    }

    public void ChangeColor(Color[] input)
        => DrawColor = input[0];

    public int ColorLength() => 1;

    public Vector2 GetPosition()
        => Position;

    public void Move(Vector2 newPosition)
    {
        Position = newPosition;
        UpdateRectangle();
        CalculateHitboxes();
    }
}