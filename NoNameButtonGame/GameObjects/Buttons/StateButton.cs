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

public class StateButton : EmptyButton
{
    private int ammoutStates;

    public int States
    {
        get => ammoutStates;
        set
        {
            ammoutStates = value;
            CurrentStates = ammoutStates;
        }
    }

    public int CurrentStates { get; private set; }

    private TextBuilder textContainer;

    public StateButton(Vector2 position, int states) : this(position, DefaultSize, states)
    {
    }

    public StateButton(Vector2 position, Vector2 size, int states) : base(position, size)
    {
        textContainer = new TextBuilder("test", new Vector2(float.MinValue, float.MinValue), new Vector2(16, 16),
            null, 0);

        CurrentStates = states;
        ammoutStates = states;
    }

    public override void Update(GameTime gameTime, Rectangle mousePosition)
    {
        bool hover = HitboxCheck(mousePosition);
        if (hover)
        {
            if (!_hover)
                InvokeEnterEventHandler();

            if (InputReaderMouse.CheckKey(InputReaderMouse.MouseKeys.Left, true))
            {
                CurrentStates--;
                if (CurrentStates <= 0) InvokeClickEventHandler();
            }
        }
        else if (_hover)
            InvokeLeaveEventHandler();

        ImageLocation = new Rectangle(hover ? (int) FrameSize.X : 0, 0, (int) FrameSize.X, (int) FrameSize.Y);
        _hover = hover;
        textContainer.ChangeText(CurrentStates.ToString());

        textContainer.Position = Rectangle.Center.ToVector2() - textContainer.Rectangle.Size.ToVector2() / 2;
        textContainer.Position.Y -= 32;
        textContainer.Update(gameTime);
        Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        base.Draw(spriteBatch);
        textContainer.Draw(spriteBatch);
    }
}