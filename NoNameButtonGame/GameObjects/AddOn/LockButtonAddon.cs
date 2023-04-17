using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.AddOn;

public class LockButtonAddon : GameObject, IInteractable, IMoveable, IButtonAddon
{
    public bool IsLocked { get; private set; } = true;

    private readonly ButtonAddonAdapter _button;
    private readonly Text _text;
    private int _offset;

    public event Action<object, IButtonAddon.CallState> Callback;

    public LockButtonAddon(ButtonAddonAdapter button) : base(button.GetRectangle().Center.ToVector2(), new Vector2(2, 2),
        DefaultTexture, DefaultMapping)
    {
        this._button = button;
        button.Callback += ClickHandler;
        _text = new Text(Letter.ReverseParse(Letter.Character.LockLocked).ToString(), button.Position);
        UpdateText();
        button.SetIndicatorOffset((int)Size.X);
    }
    
    public void SetIndicatorOffset(int x)
    {
        _offset = x;
        Move(Position);
    }

    private void ClickHandler(object sender, IButtonAddon.CallState state)
    {
        if (!IsLocked)
            Callback?.Invoke(sender, state);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        if (!IsLocked)
            _button.UpdateInteraction(gameTime, toCheck);
    }

    public override void Update(GameTime gameTime)
    {
        base.Update(gameTime);
        _text.Update(gameTime);

        if (IsLocked)
            _button.SetDrawColor(Color.DarkGray);
        else
            _button.SetDrawColor(Color.White);
        
        _button.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _button.Draw(spriteBatch);
        _text.Draw(spriteBatch);
    }

    public void Unlock()
    {
        IsLocked = false;
        UpdateText();
    }

    public void Lock()
    {
        IsLocked = true;
        UpdateText();
    }

    private void UpdateText()
    {
        _text.ChangeText(IsLocked
            ? Letter.ReverseParse(Letter.Character.LockLocked).ToString()
            : Letter.ReverseParse(Letter.Character.LockUnlocked).ToString());
        _text.ChangeColor(IsLocked ? Color.Gray : Color.DarkGray);
    }
    
    public Vector2 GetPosition()
        => _button.GetPosition();

    public Vector2 GetSize()
        => _button.GetSize();
    
    public Rectangle GetRectangle()
        => _button.GetRectangle();

    public void SetDrawColor(Color color)
        => _button.SetDrawColor(color);

    public void Move(Vector2 newPosition)
    {
        _button.Move(newPosition);
        _text.Move(newPosition);
        Position = newPosition + new Vector2(_offset, 0);
    }
}