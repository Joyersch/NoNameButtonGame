using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.AddOn;

public class ButtonAddonAdapter : IMoveable, IButtonAddon
{
    public Vector2 Position => _isAddon ? _addon.GetPosition() : _button.Position;
    public Vector2 Size => _isAddon ? _addon.GetSize() : _button.Size;

    public event Action<object, IButtonAddon.CallState> Callback;

    private readonly bool _isAddon;
    private readonly EmptyButton _button;
    private readonly IButtonAddon _addon;

    public ButtonAddonAdapter(EmptyButton button)
    {
        _button = button;
        _button.Click += ButtonClick;
        _button.Leave += ButtonLeave;
        _button.Enter += ButtonEnter;
    }

    public ButtonAddonAdapter(IButtonAddon addon)
    {
        _addon = addon;
        _isAddon = true;
        _addon.Callback += AddonCallback;
    }

    public void SetIndicatorOffset(int x)
    {
        if (!_isAddon)
            return;
        _addon.MoveIndicatorBy(new Vector2(x, 0));
        _addon.SetIndicatorOffset(x);
    }

    private void ButtonClick(object obj)
        => Callback?.Invoke(obj, IButtonAddon.CallState.Click);

    private void ButtonEnter(object obj)
        => Callback?.Invoke(obj, IButtonAddon.CallState.Enter);

    private void ButtonLeave(object obj)
        => Callback?.Invoke(obj, IButtonAddon.CallState.Leave);

    private void AddonCallback(object obj, IButtonAddon.CallState state)
        => Callback?.Invoke(obj, state);

    public Rectangle GetRectangle()
        => _isAddon ? _addon.GetRectangle() : _button.Rectangle;

    public void SetDrawColor(Color color)
    {
        if (_isAddon)
            _addon.SetDrawColor(color);
        else
            _button.DrawColor = color;
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        if (_isAddon)
            _addon.UpdateInteraction(gameTime, toCheck);
        else
            _button.UpdateInteraction(gameTime, toCheck);
    }

    public void Update(GameTime gameTime)
    {
        if (_isAddon)
            _addon.Update(gameTime);
        else
            _button.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        if (_isAddon)
            _addon.Draw(spriteBatch);
        else
            _button.Draw(spriteBatch);
    }

    public Vector2 GetPosition()
    {
        if (_isAddon)
            return _addon.GetPosition();
        return _button.GetPosition();
    }

    public Vector2 GetSize()
    {
        if (_isAddon)
            return _addon.GetSize();
        return _button.GetSize();
    }

    public void Move(Vector2 newPosition)
    {
        if (_isAddon)
            _addon.Move(newPosition);
        else
            _button.Move(newPosition);
    }
    
    public void MoveIndicatorBy(Vector2 newPosition)
    {
        if (_isAddon)
            _addon.MoveIndicatorBy(newPosition);
    }
}