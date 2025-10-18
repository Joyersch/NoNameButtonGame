using System;
using Joyersch.Monogame;
using Joyersch.Monogame.Ui;
using Joyersch.Monogame.Ui.Buttons;
using Joyersch.Monogame.Ui.Buttons.AddOn;
using Joyersch.Monogame.Ui.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.GameObjects.Buttons;

namespace NoNameButtonGame.LevelSystem.LevelContainer.CookieClickerLevel;

public class ShopOption : IInteractable, IMoveable, IRectangle, IScaleable
{
    private BasicText _amountDisplay;
    private LockButtonAddon _button;
    private TextButton<SampleButton> _textButton;
    private MouseActionsMat _infoMat;
    private BasicText _priceDisplay;

    private int _amount;
    private int _maxAmount;
    private int _currentPrice;
    private double _priceIncrease;
    private bool _isHover;

    public bool Maxed => _amount == _maxAmount;

    private Vector2 _position;
    private Vector2 _size;

    public Rectangle Rectangle => new(_position.ToPoint(), _size.ToPoint());

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

        _button = new LockButtonAddon(_textButton, 3f);
        _size = new Vector2(_button.GetSize().X, sizeY);
        _button.InRectangle(this).OnCenter().BySize(-0.5F).Apply();
        _button.Click += ButtonClick;
        _button.Enter += _ => ButtonEnter?.Invoke();
        _button.Leave += _ => ButtonLeave?.Invoke();

        _infoMat = new MouseActionsMat(_textButton);
        _infoMat.Enter += _ => _isHover = true;
        _infoMat.Leave += _ => _isHover = false;

        _amountDisplay = new BasicText($"{_amount:n0}/{_maxAmount:n0}", 3f);
        _amountDisplay.InRectangle(this).OnCenter().BySize(-0.5F).OnY(0.3F).Apply();

        _priceDisplay = new BasicText($"{_currentPrice:n0}{_icon}", 3f);
        _priceDisplay.InRectangle(this).OnCenter().BySize(-0.5F).OnY(0.7F).Apply();
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

    public bool UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        bool @return = false;
        @return |= _button.UpdateInteraction(gameTime, toCheck);
        @return |= _infoMat.UpdateInteraction(gameTime, toCheck);
        return @return;
    }

    public void Update(GameTime gameTime, long beanCount)
    {
        if (beanCount >= _currentPrice)
            _button.Unlock();
        else
            _button.Lock();

        _button.InRectangle(this)
            .OnCenter()
            .BySize(-0.5F)
            .Apply();
        _amountDisplay.InRectangle(this)
            .OnCenter()
            .BySize(-0.5F)
            .OnY(0.3F)
            .Apply();
        _priceDisplay.InRectangle(this)
            .OnCenter()
            .BySize(-0.5F)
            .OnY(0.7F)
            .Apply();

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

    public float Scale { private set; get; }
    public void SetScale(ScaleProvider provider)
    {
        Scale = provider.Scale;

        _button.SetScale(provider);
        _button.InRectangle(this)
            .OnCenter()
            .BySize(-0.5F)
            .Apply();

        _amountDisplay.SetScale(provider);
        _amountDisplay.InRectangle(this)
            .OnCenter()
            .BySize(-0.5F)
            .OnY(0.3F)
            .Apply();

        _priceDisplay.SetScale(provider);
        _priceDisplay.InRectangle(this)
            .OnCenter()
            .BySize(-0.5F)
            .OnY(0.7F)
            .Apply();
    }

    public Rectangle[] Hitbox => [];
}