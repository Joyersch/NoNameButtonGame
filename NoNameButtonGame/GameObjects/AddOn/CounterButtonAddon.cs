using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.AddOn;

public class CounterButtonAddon : GameObject, IInteractable
{
    public event Action StateReachedZero;

    private int _states;
    private readonly EmptyButton _button;
    private readonly TextSystem.Text _text;

    public CounterButtonAddon(EmptyButton button, int startStates) : base(
        button.Position, button.Size, DefaultTexture, DefaultMapping)
    {
        this._button = button;
        _states = startStates;
        button.Click += ClickHandler;
        _text = new TextSystem.Text(Letter.ReverseParse(Letter.Character.LockLocked).ToString(),
            button.Position);
        UpdateText();
    }

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
            StateReachedZero?.Invoke();
            _text.ChangeText(string.Empty);
        }

        UpdateText();
    }
}