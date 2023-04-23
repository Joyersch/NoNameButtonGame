using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.Interfaces;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class ShopOption : IInteractable, IMoveable
{
    private Text _amountDisplay;
    private LockButtonAddon _button;
    private Text _priceDisplay;

    private int _amount;
    private int _maxAmount;
    private int _currentPrice;
    private double _priceIncrease;

    private Vector2 _position;
    private Vector2 _size;

    public int Value => _amount;
    public event Action<int, int> Purchased;

    public ShopOption(float sizeY, string text, int startAmount, int startPrice, double priceIncrease, int maxAmount) : this(
        Vector2.Zero, sizeY, text, startAmount, startPrice, priceIncrease, maxAmount)
    {
    }

    public ShopOption(Vector2 position, float sizeY, string text, int startAmount, int startPrice,
        double priceIncrease, int maxAmount)
    {
        _position = position;
        _amount = startAmount;
        _currentPrice = startPrice;
        _maxAmount = maxAmount;
        
        for (int i = 0; i < startAmount; i++)
            _currentPrice = (int)(_currentPrice * priceIncrease);
        _priceIncrease = priceIncrease;
        
        if (_amount == _maxAmount)
            _currentPrice = int.MaxValue;

        var button = new TextButton(text);
        
        _button = new LockButtonAddon(new ButtonAddonAdapter(button));       
        _size = new Vector2(_button.GetSize().X, sizeY);
        _button.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).Move();
        _button.Callback += ButtonClick;

        _amountDisplay = new Text($"{_amount:n0}/{_maxAmount:n0}");
        _amountDisplay.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).OnY(0.3F).Move();

        _priceDisplay = new Text(_currentPrice.ToString("n0"));
        _priceDisplay.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).OnY(0.7F).Move();
    }

    private void ButtonClick(object obj, IButtonAddon.CallState state)
    {
        if (state != IButtonAddon.CallState.Click)
            return;

        if (_amount == _maxAmount)
            return;
        
        _amount++;
        Purchased?.Invoke(_amount, _currentPrice);
        _currentPrice =  (int)(_currentPrice * _priceIncrease);
        
        if (_amount == _maxAmount)
            _currentPrice = int.MaxValue;
        
        _priceDisplay.ChangeText(_currentPrice.ToString("n0"));
        _amountDisplay.ChangeText($"{_amount:n0}/{_maxAmount:n0}");
        
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _button.UpdateInteraction(gameTime, toCheck);
    }

    public void Update(GameTime gameTime, long beanCount)
    {
        if (beanCount >= _currentPrice)
            _button.Unlock();
        else
            _button.Lock();
        
        _button.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).Move();
        _amountDisplay.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).OnY(0.3F).Move();
        _priceDisplay.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).OnY(0.7F).Move();
        
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