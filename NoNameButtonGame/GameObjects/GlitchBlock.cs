using System;
using Microsoft.Xna.Framework;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.GameObjects;

internal class GlitchBlock : GameObject, IMouseActions, IColorable, IMoveable
{
    private int framePos;
    private readonly int frameMax;
    private const int FrameSpeed = 1000 / 64 * 2;

    private float savedGameTime;

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
        frameMax = textureHitboxMapping.AnimationsFrames;
        FrameSize = scaleToTexture / (scaleToTexture.X > scaleToTexture.Y ? scaleToTexture.X : scaleToTexture.Y) *
                    FrameSize;
    }

    public override void Initialize()
    {
        textureHitboxMapping = Globals.Textures.GetMappingFromCache<GlitchBlock>();
    }

    public void Update(GameTime gameTime, Rectangle mousePosition)
    {
        Mouse.GetState();
        savedGameTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
        while (savedGameTime > FrameSpeed)
        {
            savedGameTime -= FrameSpeed;
            framePos++;
            if (framePos == frameMax) framePos = 0;
            ImageLocation = new Rectangle(
                !textureHitboxMapping.AnimationFromTop ?? false
                    ? framePos * (int) textureHitboxMapping.ImageSize.Y
                    : 0
                , textureHitboxMapping.AnimationFromTop ?? true
                    ? framePos * (int) textureHitboxMapping.ImageSize.X
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