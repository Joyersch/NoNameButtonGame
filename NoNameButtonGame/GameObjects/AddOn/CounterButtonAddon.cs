using System;
using System.Drawing;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;
using Color = Microsoft.Xna.Framework.Color;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.GameObjects.AddOn;

public class CounterButtonAddon : ButtonAddonBase
{
    private int _states;
    private readonly ButtonAddonAdapter _button;
    private int _offset;
    private readonly TextSystem.Text _text;

    public CounterButtonAddon(ButtonAddonAdapter button, int startStates) : base(button)
    {
        this._button = button;
        _states = startStates;
        _text = new TextSystem.Text(Letter.ReverseParse(Letter.Character.LockLocked).ToString(),
            button.Position);
        UpdateText();
    }

    public override int GetIndicatorOffset()
    => _button.GetIndicatorOffset() + Rectangle.Size.X;
    

    public override Rectangle GetRectangle()
        => _button.GetRectangle();

    public override void SetDrawColor(Color color)
        => _button.SetDrawColor(color);

    public override void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _button.UpdateInteraction(gameTime, toCheck);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _text.Update(gameTime);
        _button.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _button.Draw(spriteBatch);
        _text.Draw(spriteBatch);
    }

    private void UpdateText()
    {
        _text.ChangeText(_states.ToString());
    }

    protected override void ButtonCallback(object obj, IButtonAddon.CallState state)
    {
        if (state == IButtonAddon.CallState.Click)
            _states--;

        if (_states == 0)
        {
            base.ButtonCallback(obj, state);
            _text.ChangeText(string.Empty);
        }

        UpdateText();
    }

    public override Vector2 GetPosition()
        => _button.GetPosition();

    public override Vector2 GetSize()
        => _button.GetSize();

    public override void Move(Vector2 newPosition)
    {
        _button.Move(newPosition);
        _text.Move(newPosition);
        Position = newPosition+ new Vector2(_offset, 0);
    }
}