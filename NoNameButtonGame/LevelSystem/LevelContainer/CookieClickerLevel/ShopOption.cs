using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui.Logic;
using MonoUtils.Ui.Objects.Buttons;
using MonoUtils.Ui.Objects.Buttons.AddOn;
using MonoUtils.Ui.Objects.TextSystem;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.LevelContainer.CookieClickerLevel;

public class ShopOption : IInteractable, IMoveable
{
    private Text _amountDisplay;
    private LockButtonAddon _button;
    private TextButton<SampleButton> _textButton;
    private MouseActionsMat _infoMat;
    private Text _priceDisplay;

    private int _amount;
    private int _maxAmount;
    private int _currentPrice;
    private double _priceIncrease;
    private bool _isHover;

    public bool Maxed => _amount == _maxAmount;

    private Vector2 _position;
    private Vector2 _size;

    public int Value => _amount;
    private string _icon = string.Empty;

    public event Action<int, int> Purchased;
    public event Action ButtonEnter;
    public event Action ButtonLeave;

    public ShopOption(float sizeY, string text, int startAmount, int startPrice, double priceIncrease,
        int maxAmount) : this(
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
            _currentPrice = 0;

        _textButton = new Button(text);

        _button = new LockButtonAddon(_textButton);
        _size = new Vector2(_button.GetSize().X, sizeY);
        _button.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).Move();
        _button.Click += ButtonClick;
        _button.Enter += o => ButtonEnter?.Invoke();
        _button.Leave += o => ButtonLeave?.Invoke();

        _infoMat = new MouseActionsMat(_textButton);
        _infoMat.Enter += _ => _isHover = true;
        _infoMat.Leave += _ => _isHover = false;

        _amountDisplay = new Text($"{_amount:n0}/{_maxAmount:n0}");
        _amountDisplay.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).OnY(0.3F).Move();

        _priceDisplay = new Text($"{_currentPrice:n0}{_icon}");
        _priceDisplay.GetCalculator(_position, _size).OnCenter().BySize(-0.5F).OnY(0.7F).Move();
    }

    private void ButtonClick(object obj)
    {
        if (_amount == _maxAmount)
            return;

        _amount++;
        Purchased?.Invoke(_amount, _currentPrice);
        _currentPrice = (int)(_currentPrice * _priceIncrease);

        if (_amount == _maxAmount)
            _currentPrice = 0;
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _button.UpdateInteraction(gameTime, toCheck);
        _infoMat.UpdateInteraction(gameTime, toCheck);
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

        _priceDisplay.ChangeText($"{_currentPrice:n0}{_icon}");
        _amountDisplay.ChangeText($"{_amount:n0}/{_maxAmount:n0}");

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
        _button.Move(_button.GetPosition() + offset);
        _priceDisplay.Move(_priceDisplay.Position + offset);
        _position = newPosition;
    }

    public void ChangeCurrencyIcon(string icon)
        => _icon = icon;

    public bool IsHoverOnButton()
        => _isHover;

    public void ChangeText(string newText)
        => _textButton.Text.ChangeText(newText);
}