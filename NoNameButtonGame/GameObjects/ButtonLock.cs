using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects;

public class ButtonLock : GameObject
{
    public bool IsLocked => _locked;

    private bool _locked = true;
    private EmptyButton button;
    private TextBuilder text;

    public event Action<object> Callback;

    public ButtonLock(EmptyButton button) : base(button.rectangle.Center.ToVector2(), new Vector2(2, 2))
    {
        this.button = button;
        button.ClickEventHandler += ClickHandler;
        text = new TextBuilder(Letter.ReverseParse(Letter.Character.LockLocked).ToString(), button.Position);
        UpdateText();
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<ButtonLock>();
    }

    private void ClickHandler(object sender)
    {
        if (!_locked)
            Callback(sender);
    }

    public void Update(GameTime gameTime, Rectangle mousePosition)
    {
        base.Update(gameTime);
        button.Update(gameTime, !_locked ? mousePosition : rectangle);
        text.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        button.Draw(spriteBatch);
        text.Draw(spriteBatch);
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

    private void UpdateText()
    {
        text.ChangeText(_locked
            ? Letter.ReverseParse(Letter.Character.LockLocked).ToString()
            : Letter.ReverseParse(Letter.Character.LockUnlocked).ToString());
        text.ChangeColor(_locked ? Color.Red : Color.Green);
    }
}