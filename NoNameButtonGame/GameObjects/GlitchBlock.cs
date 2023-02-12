using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Cache;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.GameObjects;

internal class GlitchBlock : GameObject, IMouseActions, IColorable
{
    private int FramePos = 0;
    private int FrameMax = 0;
    private int FrameSpeed = 180;


    private float GT;

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

    public GlitchBlock(Vector2 position, Vector2 canvas) : base(position, canvas)
    {
        FrameMax = _textureHitboxMapping.AnimationsFrames;
        DrawColor = Color.White;
        ImageLocation = new Rectangle(0, 0
            , (int) _textureHitboxMapping.ImageSize.X, (int) _textureHitboxMapping.ImageSize.Y);
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Globals.Textures.GetMappingFromCache<GlitchBlock>();
    }

    public void Update(GameTime gt, Rectangle mousePosition)
    {
        MouseState mouseState = Mouse.GetState();
        GT += (float) gt.ElapsedGameTime.TotalMilliseconds;
        while (GT > FrameSpeed)
        {
            GT -= FrameSpeed;
            FramePos++;
            if (FramePos == FrameMax) FramePos = 0;
            ImageLocation = new Rectangle(0, FramePos * (int) FrameSize.X, (int) FrameSize.X, (int) FrameSize.Y);
        }

        if (HitboxCheck(mousePosition))
            EnterEventHandler(this);
        base.Update(gt);
    }

    public void ChangeColor(Color[] input)
        => DrawColor = input[0];

    public int ColorLength() => 1;
}