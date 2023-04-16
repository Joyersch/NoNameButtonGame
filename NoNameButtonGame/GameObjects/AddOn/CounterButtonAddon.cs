using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.AddOn;

public class CounterButtonAddon : GameObject, IInteractable, IMoveable, IButtonAddon
{
    public event Action<object> Callback;

    private int _states;
    private readonly ButtonAddonAdapter _button;
    private readonly TextSystem.Text _text;

    public CounterButtonAddon(ButtonAddonAdapter button, int startStates) : base(
        button.Position, button.Size, DefaultTexture, DefaultMapping)
    {
        this._button = button;
        _states = startStates;
        button.Click += ClickHandler;
        _text = new TextSystem.Text(Letter.ReverseParse(Letter.Character.LockLocked).ToString(),
            button.Position);
        UpdateText();
    }

    public Rectangle GetRectangle()
        => _button.GetRectangle();

    public void SetDrawColor(Color color)
        => _button.SetDrawColor(color);

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
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

    private void ClickHandler(object obj)
    {
        _states--;
        if (_states == 0)
        {
            Callback?.Invoke(obj);
            _text.ChangeText(string.Empty);
        }

        UpdateText();
    }

    public Vector2 GetPosition()
        => _button.GetPosition();

    public Vector2 GetSize()
        => _button.GetSize();

    public void Move(Vector2 newPosition)
    {
        _button.Move(newPosition);
        _text.Move(newPosition);
        Position = newPosition;
    }
}