using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Logic.Management;
using MonoUtils.Ui.Objects;
using NoNameButtonGame.GameObjects.Glitch;

namespace NoNameButtonGame.LevelSystem.LevelContainer.GlitchBlockHoldButtonChallenge;

public class Row : IManageable, IInteractable, IMoveable, IMouseActions
{
    private GlitchBlockCollection _left;
    private GlitchBlockCollection _right;

    public event Action<object> Leave;
    public event Action<object> Enter;
    public event Action<object> Click;

    public Rectangle Rectangle { get; private set; }

    public Row(float distance, Vector2 singleSize, float scale)
    {
        _left = new GlitchBlockCollection(singleSize, scale);
        _left.Leave += o => Leave?.Invoke(o);
        _left.Enter += o => Enter?.Invoke(o);
        _left.Click += o => Click?.Invoke(o);

        _right = new GlitchBlockCollection(singleSize, scale);
        _right.Leave += o => Leave?.Invoke(o);
        _right.Enter += o => Enter?.Invoke(o);
        _right.Click += o => Click?.Invoke(o);
        _right.GetAnchor(_left)
            .SetMainAnchor(AnchorCalculator.Anchor.TopRight)
            .SetSubAnchor(AnchorCalculator.Anchor.TopLeft)
            .SetDistanceX(distance)
            .Move();
        Rectangle = Rectangle.Union(_left.Rectangle, _right.Rectangle);
    }

    public void Update(GameTime gameTime)
    {
        _left.Update(gameTime);
        _right.Update(gameTime);
    }


    public void Draw(SpriteBatch spriteBatch)
    {
        _left.Draw(spriteBatch);
        _right.Draw(spriteBatch);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _left.UpdateInteraction(gameTime, toCheck);
        _right.UpdateInteraction(gameTime, toCheck);
    }

    public Vector2 GetPosition()
        => Rectangle.Location.ToVector2();

    public Vector2 GetSize()
        => Rectangle.Size.ToVector2();

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - GetPosition();
        _left.Move(_left.GetPosition() + offset);
        _right.Move(_right.GetPosition() + offset);
        Rectangle = Rectangle.Union(_left.Rectangle, _right.Rectangle);
    }
}