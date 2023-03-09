using System;
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
    private Text _amountDisplay;
    private LockButtonAddon _button;
    private Text _priceDisplay;

    private int _amount;
    private float _currentPrice;
    private float _priceIncrease;

    public event Action<int> Purchased;

    public ShopOption(Vector2 position, string text, int startAmount, float startPrice, float priceIncrease)
    {
        _amount = startAmount;
        _currentPrice = startPrice;

        for (int i = 0; i < startAmount; i++)
            _currentPrice *= priceIncrease;
        _priceIncrease = priceIncrease;

        var button = new TextButton(text);
        button.Move(position - button.Size / 2);
        _button = new LockButtonAddon(button);
        _button.Callback += ButtonClick;

        _amountDisplay = new Text(_amount.ToString());
        _amountDisplay.Move(position);

        _priceDisplay = new Text(_currentPrice.ToString());
        _priceDisplay.Move(position);
    }

    private void ButtonClick(object obj)
    {
        _amount++;
        _currentPrice *= _priceIncrease;
        _priceDisplay.ChangeText(((int) _currentPrice).ToString());
        _amountDisplay.ChangeText(_amount.ToString());
        Purchased?.Invoke(_amount);
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _button.UpdateInteraction(gameTime, toCheck);
    }

    public void Update(GameTime gameTime, long beanCount)
    {
        if (beanCount > _currentPrice)
            _button.Unlock();
        else
            _button.Lock();

        _button.Update(gameTime);
        _amountDisplay.Update(gameTime);
        _priceDisplay.Update(gameTime);
    }

    public void Draw(SpriteBatch spriteBatch)
    {
        _button.Draw(spriteBatch);
        _amountDisplay.Draw(spriteBatch);
        _priceDisplay.Draw(spriteBatch);
    }
}