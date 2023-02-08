using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.Hitboxes;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects;

public class ButtonStateAddon : GameObject
{
    public event Action StateReachedZero;

    public int _states;
    private EmptyButton button;
    private TextBuilder text;

    public ButtonStateAddon(EmptyButton button, int startStates) : base(
        button.Position, button.Size)
    {
        this.button = button;
        _states = startStates;
        button.ClickEventHandler += ClickHandler;
        text = new TextBuilder(Letter.ReverseParse(Letter.Character.LockLocked).ToString(),
            button.Position + new Vector2(TextButton.DefaultTextSize.X, 0));
        UpdateText();
    }

    public void Update(GameTime gameTime, Rectangle mousePosition)
    {
        base.Update(gameTime);
        button.Update(gameTime, mousePosition);
        text.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        button.Draw(spriteBatch);
        text.Draw(spriteBatch);
    }

    private void UpdateText()
    {
        text.ChangeText(_states.ToString());
    }

    public override void Initialize()
    {
        _textureHitboxMapping = Mapping.GetMappingFromCache<ButtonLock>();
    }

    private void ClickHandler(object obj)
    {
        _states--;
        if (_states == 0)
        {
            StateReachedZero?.Invoke();
            text.ChangeText(string.Empty);
        }

        UpdateText();
    }
}