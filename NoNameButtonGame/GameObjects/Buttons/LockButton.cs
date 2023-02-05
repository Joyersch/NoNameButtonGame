using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
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
    public bool IsLocked => _locked;
    protected bool _locked;

    private TextBuilder textContainer;

    public LockButton(Vector2 position) : this(position, true)
    {
        
    }
    public LockButton(Vector2 position, bool startLocked) : this(position, DefaultSize, startLocked)
    {
        
    }

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

    public void Unlock()
    {
        _locked = false;
        UpdateText();
    }

    public void Lock()
    {
        _locked = true;
        UpdateText();
    }

    public void UpdateText()
    {
        textContainer.ChangeText(_locked ? "Locked" : "Unlocked");
        textContainer.ChangeColor(_locked ? Color.Red : Color.Green);
        textContainer.Position = rectangle.Center.ToVector2() - textContainer.rectangle.Size.ToVector2() / 2;
        textContainer.Position.Y -= 32;
    }
}