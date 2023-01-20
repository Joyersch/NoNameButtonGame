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

    public event EventHandler EnterEventHandler;
    public event EventHandler LeaveEventHandler;
    public event EventHandler ClickEventHandler;

    public DontTouch(Vector2 Pos, Vector2 Size) : base(Pos, Size)
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

    public void Update(GameTime gt, Rectangle MousePos)
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

        if (HitboxCheck(MousePos))
            EnterEventHandler(this, EventArgs.Empty);
        base.Update(gt);
    }
}