using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class ShopOption : IInteractable, IMoveable
{
    private Text _amountDisplay;
    private LockButtonAddon _button;
    private Text _priceDisplay;

    private int _amount;
    private float _currentPrice;
    private float _priceIncrease;

    private Vector2 _position;
    private Vector2 _size;
    public event Action<int> Purchased;

    public ShopOption(float sizeY, string text, int startAmount, float startPrice, float priceIncrease) : this(
        Vector2.Zero, sizeY, text, startAmount, startPrice, priceIncrease)
    {
    }

    public ShopOption(Vector2 position, float sizeY, string text, int startAmount, float startPrice,
        float priceIncrease)
    {
        _position = position;
        _amount = startAmount;
        _currentPrice = startPrice;

        for (int i = 0; i < startAmount; i++)
            _currentPrice *= priceIncrease;
        _priceIncrease = priceIncrease;

        var button = new TextButton(text);


        _button = new LockButtonAddon(button);       
        _size = new Vector2(_button.GetSize().X, sizeY);
        _button.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).Move();
        _button.Callback += ButtonClick;

        _amountDisplay = new Text(_amount.ToString());
        _amountDisplay.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).OnY(0.3F).Move();

        _priceDisplay = new Text(_currentPrice.ToString());
        _priceDisplay.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).OnY(0.7F).Move();
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
        
        Console.WriteLine(_button.GetSize());
        
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

    public Vector2 GetPosition()
        => _position;

    public Vector2 GetSize()
        => _size;

    public void Move(Vector2 newPosition)
    {
        var offset = newPosition - _position;
        _amountDisplay.Move(_amountDisplay.Position + offset);
        _button.Move(_button.Position + offset);
        _priceDisplay.Move(_priceDisplay.Position + offset);
        _position = newPosition;
    }
}