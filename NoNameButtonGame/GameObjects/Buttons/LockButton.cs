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

public class LockButton : EmptyButton
{
    public bool Locked
    {
        get => _locked;
        set => _locked = value;
    }

    protected bool _locked;

    TextBuilder textContainer;


    public LockButton(Vector2 position, Vector2 size, bool startState) : base(position, size)
    {
        textContainer = new TextBuilder(
            string.Empty
            , new Vector2(float.MinValue, float.MinValue)
            , new Vector2(16, 16)
            , 0);
        _locked = startState;
    }

    public override void Update(GameTime gameTime, Rectangle mousePosition)
    {
        textContainer.ChangeText(_locked ? "Locked" : "Unlocked");
        textContainer.Position = rectangle.Center.ToVector2() - textContainer.rectangle.Size.ToVector2() / 2;
        textContainer.Position.Y -= 32;
        textContainer.Update(gameTime);
        bool hover = HitboxCheck(mousePosition);
        if (hover)
        {
            if (!_hover)
                InvokeEnterEventHandler();

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true) && !_locked)
                InvokeClickEventHandler();
        }
        else if (_hover)
            InvokeLeaveEventHandler();

        ImageLocation =
            new Rectangle(hover || _locked ? (int) FrameSize.X : 0, 0, (int) FrameSize.X, (int) FrameSize.Y);
        _hover = hover;
        base.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        textContainer.Draw(spriteBatch);
    }
}