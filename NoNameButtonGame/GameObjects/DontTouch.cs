using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.Interfaces;
using Microsoft.Xna.Framework.Input;

namespace NoNameButtonGame.GameObjects;

class DontTouch : GameObject, IMouseActions
{
    Vector2 Scale;
    int FramePos = 0;
    int FrameMax = 0;
    int FrameSpeed = 180;


    float GT;

    public event Action<object> EnterEventHandler;
    public event Action<object> LeaveEventHandler;
    public event Action<object> ClickEventHandler;

    public DontTouch(Vector2 position, Vector2 size) : base(position, size)
    {
        FrameMax = _textureHitboxMapping.AnimationsFrames;
        DrawColor = Color.White;
        ImageLocation = new Rectangle(0, 0
            , (int) _textureHitboxMapping.ImageSize.X, (int) _textureHitboxMapping.ImageSize.Y);
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<DontTouch>();
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
}