using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Text;
using NoNameButtonGame.Text;

namespace NoNameButtonGame.GameObjects.AddOn;

public class LockButtonAddon : GameObject
{
    public bool IsLocked { get; private set; } = true;

    private readonly EmptyButton button;
    private readonly TextBuilder text;

    public event Action<object> Callback;

    
    public LockButtonAddon(EmptyButton button) : base(button.Rectangle.Center.ToVector2(), new Vector2(2, 2))
    {
        this.button = button;
        button.ClickEventHandler += ClickHandler;
        text = new TextBuilder(Letter.ReverseParse(Letter.Character.LockLocked).ToString(), button.Position);
        UpdateText();
    }

    public override void Initialize()
    {
        textureHitboxMapping = Globals.Textures.GetMappingFromCache<LockButtonAddon>();
    }

    private void ClickHandler(object sender)
    {
        if (!IsLocked)
            Callback?.Invoke(sender);
    }

    public void Update(GameTime gameTime, Rectangle mousePosition)
    {
        base.Update(gameTime);
        button.Update(gameTime, !IsLocked ? mousePosition : Rectangle);
        text.Update(gameTime);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        button.Draw(spriteBatch);
        text.Draw(spriteBatch);
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
        text.ChangeText(IsLocked
            ? Letter.ReverseParse(Letter.Character.LockLocked).ToString()
            : Letter.ReverseParse(Letter.Character.LockUnlocked).ToString());
        text.ChangeColor(IsLocked ? Color.Gray : Color.DarkGray);
    }
}