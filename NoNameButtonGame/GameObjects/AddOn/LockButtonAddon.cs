using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.GameObjects.AddOn;

public class LockButtonAddon : GameObject, IInteractable
{
    public bool IsLocked { get; private set; } = true;

    private readonly EmptyButton _button;
    private readonly Text _text;

    public event Action<object> Callback;


    public LockButtonAddon(EmptyButton button) : base(button.Rectangle.Center.ToVector2(), new Vector2(2, 2),
        DefaultTexture, DefaultMapping)
    {
        this._button = button;
        button.Click += ClickHandler;
        _text = new Text(Letter.ReverseParse(Letter.Character.LockLocked).ToString(), button.Position);
        UpdateText();
    }

    private void ClickHandler(object sender)
    {
        if (!IsLocked)
            Callback?.Invoke(sender);
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
            _button.DrawColor = Color.DarkGray;
        else
            _button.DrawColor = Color.White;
        
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
}