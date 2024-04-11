using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.GlitchBlockHoldButtonChallenge;

public class Column : IManageable, IInteractable, IMoveable, IMouseActions
{
    private GlitchBlockCollection _up;
    private GlitchBlockCollection _down;

    public Rectangle Rectangle { get; private set; }

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    public Column(float distance, Vector2 singleSize, float scale)
    {
        _up = new GlitchBlockCollection(singleSize, scale);
        _up.Leave += o => Leave?.Invoke(o);
        _up.Enter += o => Enter?.Invoke(o);
        _up.Click += o => Click?.Invoke(o);

        _down = new GlitchBlockCollection(singleSize, scale);
        _down.Leave += o => Leave?.Invoke(o);
        _down.Enter += o => Enter?.Invoke(o);
        _down.Click += o => Click?.Invoke(o);
        _down.GetAnchor(_up)
            .SetMainAnchor(AnchorCalculator.Anchor.BottomRight)
            .SetSubAnchor(AnchorCalculator.Anchor.TopRight)
            .SetDistanceY(distance)
            .Move();
        Rectangle = Rectangle.Union(_up.Rectangle, _down.Rectangle);
    }

    public void Update(GameTime gameTime)
    {
        _up.Update(gameTime);
        _down.Update(gameTime);
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        _up.Draw(spriteBatch);
        _down.Draw(spriteBatch);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _up.UpdateInteraction(gameTime, toCheck);
        _down.UpdateInteraction(gameTime, toCheck);
    }

    public Vector2 GetPosition()
        => Rectangle.Location.ToVector2();

    public Vector2 GetSize()
        => Rectangle.Size.ToVector2();

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - GetPosition();
        _up.Move(_up.GetPosition() + offset);
        _down.Move(_down.GetPosition() + offset);
        Rectangle = Rectangle.Union(_up.Rectangle, _down.Rectangle);
    }
}