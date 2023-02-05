using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.Input;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects.Buttons;

internal class HoldButton : EmptyButton
{
    private float _holdTime = 0F;
    public float EndHoldTime = 10000F;
    private bool _pressed = false;
    private TextBuilder textContainer;


    public HoldButton(Vector2 Position, Vector2 Size) : base(Position, Size)
    {
        textContainer = new TextBuilder("test", new Vector2(float.MinValue, float.MinValue), new Vector2(16, 16), null,
            0);
    }

    public override void Update(GameTime gameTime, Rectangle mousePosition)
    {
        textContainer.ChangeText((((EndHoldTime - _holdTime) / 1000).ToString("0.0") + "s").Replace(',', '.'));

        textContainer.Position = rectangle.Center.ToVector2() - textContainer.rectangle.Size.ToVector2() / 2;
        textContainer.Position.Y -= 32;
        textContainer.Update(gameTime);
        bool hover = HitboxCheck(mousePosition);
        if (hover)
        {
            if (!_hover)
                InvokeEnterEventHandler();

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, false) && !_pressed)
            {
                _holdTime += (float) gameTime.ElapsedGameTime.TotalMilliseconds;
                if (_holdTime > EndHoldTime)
                {
                    InvokeClickEventHandler();
                    EndHoldTime = 0;
                    _holdTime = 0;
                    _pressed = true;
                }
            }
            else if (!_pressed)
                _holdTime -= (float) gameTime.ElapsedGameTime.TotalMilliseconds / 2;
        }
        else
        {
            if (_hover)
                InvokeLeaveEventHandler();
            if (!_pressed)
                _holdTime -= (float) gameTime.ElapsedGameTime.TotalMilliseconds / 2;
        }

        if (_holdTime < 0) _holdTime = 0;
        
        ImageLocation = new Rectangle(hover ? (int) FrameSize.X : 0, 0, (int) FrameSize.X, (int) FrameSize.Y);
        _hover = hover;
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        textContainer.Draw(spriteBatch);
    }
}