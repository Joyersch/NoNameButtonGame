using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class ShopOption : IInteractable
{
    private Text _AmountDisplay;
    private LockButtonAddon _button;
    private Text _priceDisplay;

    public int Amount { get; private set; }
    private float _currentPrice;
    private float _priceIncrease;

    public ShopOption(Vector2 position, string text, int startAmount, float currentPrice, float priceIncrease)
    {
        Amount = startAmount;
        _currentPrice = currentPrice;
        _priceIncrease = priceIncrease;

        var button = new TextButton(text);
        button.Move(position + new Vector2(0, button.Rectangle.Size.Y));
        button.Click += ButtonClick;
        _button = new LockButtonAddon(button);
    }

    private void ButtonClick(object obj)
    {
        Amount++;
        _currentPrice *= _priceIncrease;
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox hitbox)
    {
        _button.UpdateInteraction(gameTime, hitbox);
    }

    public void Update(GameTime gameTime, int beanCount)
    {
        if (beanCount > _currentPrice)
            _button.Unlock();
        else
            _button.Lock();

        _button.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _button.Draw(spriteBatch);
    }
}