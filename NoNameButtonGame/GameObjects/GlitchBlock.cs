using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Cache;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.GameObjects;

internal class GlitchBlock : GameObject, IMouseActions, IColorable, IMoveable
{
    private int FramePos = 0;
    private int FrameMax = 0;
    private int FrameSpeed = 1000 / 64 * 2;

    private float GT;

    private bool hover;

    public event Action<object> EnterEventHandler;
    public event Action<object> LeaveEventHandler;
    public event Action<object> ClickEventHandler;

    public new static Vector2 DefaultSize = new Vector2(32, 32);

    public GlitchBlock(Vector2 position) : this(position, DefaultSize)
    {
    }

    public GlitchBlock(Vector2 position, float scale) : this(position, DefaultSize * scale)
    {
    }

    public GlitchBlock(Vector2 position, Vector2 size) : base(position, size)
    {
        FrameMax = _textureHitboxMapping.AnimationsFrames;
        FrameSize = _scaleToTexture / (_scaleToTexture.X > _scaleToTexture.Y ? _scaleToTexture.X : _scaleToTexture.Y) *
                    FrameSize;
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<GlitchBlock>();
    }

    public void Update(GameTime gameTime, Rectangle mousePosition)
    {
        MouseState mouseState = Mouse.GetState();
        GT += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        while (GT > FrameSpeed)
        {
            GT -= FrameSpeed;
            FramePos++;
            if (FramePos == FrameMax) FramePos = 0;
            ImageLocation = new Rectangle(
                !_textureHitboxMapping.AnimationFromTop ?? false
                    ? FramePos * (int) _textureHitboxMapping.ImageSize.Y
                    : 0
                , _textureHitboxMapping.AnimationFromTop ?? true
                    ? FramePos * (int) _textureHitboxMapping.ImageSize.X
                    : 0
                , (int) FrameSize.X, (int) FrameSize.Y);
        }

        if (HitboxCheck(mousePosition))
        {
            if (!hover)
                EnterEventHandler?.Invoke(this);
            hover = true;
        }
        else if (hover)
        {
            LeaveEventHandler?.Invoke(this);
            hover = false;
        }

        base.Update(gameTime);
    }

    public void ChangeColor(Color[] input)
        => DrawColor = input[0];

    public int ColorLength() => 1;

    public Vector2 GetPosition()
        => Position;

    public bool Move(Vector2 newPosition)
    {
        Position = newPosition;
        UpdateRectangle();
        CalculateHitboxes();
        return true;
    }
}