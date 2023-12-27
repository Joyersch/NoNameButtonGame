using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoUtils.Logic;
using MonoUtils.Logic.Hitboxes;
using MonoUtils.Ui;
using MonoUtils;
using MonoUtils.Ui.Objects.TextSystem;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level11;

public class Shop : GameObject, IInteractable
{
    public new static Vector2 DefaultSize => Display.Size / 2;

    private LevelSave _storage;
    private readonly Random _random;
    private Rectangle _rectangle;

    private readonly ShopOption _optionOne;
    private readonly ShopOption _optionTwo;
    private readonly ShopOption _optionThree;
    private readonly ShopOption _optionFour;

    private readonly Text _beanDisplay;

    private readonly List<OverTimeInvoker> _autoClicker;
    private int _notStarted;

    public long BeanCount { get; private set; }
    public string BeanDisplay => BeanCount.ToString("n0") + _icon;

    private string _icon;

    public event Action UnlockedShop;
    private bool _unlockedShop;

    public event Action UnlockedDistraction;
    private bool _unlockedDistraction;

    public event Action PurchasedAllOptions;

    private readonly Text _infoDiplay;

    private readonly string[] _infoDisplayText;
    private readonly string[] _shopOptionNames;

    public Shop(Vector2 position, Vector2 size, LevelSave storage, Random random, string[] infos, string[] shopOptions)
        : base(position, DefaultSize,
            DefaultTexture,
            DefaultMapping)
    {
        _infoDisplayText = infos;
        _shopOptionNames = shopOptions;
        _storage = storage;
        _random = random;
        _rectangle = new Rectangle(position.ToPoint(), size.ToPoint());
        _autoClicker = new List<OverTimeInvoker>();

        BeanCount = storage.Beans;

        if (storage.Upgrade1 > 0 || storage.Upgrade2 > 0 || storage.Upgrade3 > 0 || storage.Upgrade4 > 0)
            _unlockedShop = true;

        if (storage.Upgrade4 > 0)
            SetSuspiciousIcon();
        else
            SetIcon();

        _unlockedDistraction = storage.CanSeeDistraction;

        for (int i = 0; i < storage.Upgrade1; i++)
            _autoClicker.Add(GetNewInvoker(false));
        _notStarted = storage.Upgrade1;

        _optionOne = new ShopOption(size.Y, _shopOptionNames[0], storage.Upgrade1, 50, 1.07D, 100);
        _optionOne.GetCalculator(_rectangle).OnX(0.15F).BySizeX(-0.5F).Move();
        _optionOne.Purchased += DecreaseBeanCount;
        _optionOne.Purchased += OptionOnePurchased;

        _optionTwo = new ShopOption(size.Y, _shopOptionNames[1], storage.Upgrade2, 150, 1.32D, 20);
        _optionTwo.GetCalculator(_rectangle).OnX(0.35F).BySizeX(-0.5F).Move();
        _optionTwo.Purchased += DecreaseBeanCount;
        _optionTwo.Purchased += OptionTwoPurchased;

        _optionThree = new ShopOption(size.Y, _shopOptionNames[2], storage.Upgrade3, 3000, 1.46D, 10);
        _optionThree.GetCalculator(_rectangle).OnX(0.55F).BySizeX(-0.5F).Move();
        _optionThree.Purchased += DecreaseBeanCount;
        _optionThree.Purchased += OptionThreePurchased;

        _optionFour = new ShopOption(size.Y, _shopOptionNames[3], storage.Upgrade4, 250000, 99D, 1);
        _optionFour.GetCalculator(_rectangle).OnX(0.85F).BySizeX(-0.5F).Move();
        _optionFour.Purchased += DecreaseBeanCount;
        _optionFour.Purchased += OptionFourPurchased;

        _infoDiplay = new Text(string.Empty);
        _infoDiplay.GetCalculator(_rectangle).OnCenter().OnY(0.875F).BySize(-0.5F).Move();

        UpdateIcon();

        _beanDisplay = new Text(BeanDisplay);
        _beanDisplay.GetCalculator(_rectangle).OnCenter().OnY(0.12F).Centered().Move();
    }

    public void IncreaseBeanCount()
    {
        int criticalRequirements = _random.Next(1, 100);
        bool criticalHit = criticalRequirements <= _optionTwo.Value * 5;
        BeanCount += (_optionThree.Value + 1) * (criticalHit ? 10 : 1);
        _storage.Beans = BeanCount;
    }

    private void DecreaseBeanCount(int count, int value)
    {
        BeanCount -= value;
        _storage.Beans = BeanCount;
    }

    public override void Update(GameTime gameTime)
    {
        _optionOne.GetCalculator(_rectangle).OnX(0.15F).BySizeX(-0.5F).Move();
        _optionTwo.GetCalculator(_rectangle).OnX(0.38F).BySizeX(-0.5F).Move();
        _optionThree.GetCalculator(_rectangle).OnX(0.61F).BySizeX(-0.5F).Move();
        _optionFour.GetCalculator(_rectangle).OnX(0.85F).BySizeX(-0.5F).Move();
        _infoDiplay.GetCalculator(_rectangle).OnCenter().OnY(0.9F).BySize(-0.5F).Move();

        _optionOne.Update(gameTime, BeanCount);
        _optionTwo.Update(gameTime, BeanCount);
        _optionThree.Update(gameTime, BeanCount);
        _optionFour.Update(gameTime, BeanCount);
        _infoDiplay.Update(gameTime);

        if (_optionOne.Maxed && _optionTwo.Maxed && _optionThree.Maxed && _optionFour.Maxed)
            PurchasedAllOptions?.Invoke();

        _beanDisplay.ChangeText(BeanDisplay);
        _beanDisplay.GetCalculator(_rectangle).OnCenter().OnY(0.12F).Centered().Move();
        _beanDisplay.Update(gameTime);

        foreach (var invoker in _autoClicker)
            invoker.Update(gameTime);

        if (_notStarted > 0)
        {
            _autoClicker[_notStarted - 1].Start();
            _notStarted--;
        }

        if (BeanCount >= 50 || _unlockedShop)
        {
            _unlockedShop = true;
            UnlockedShop?.Invoke();
        }

        if (BeanCount >= 10000 || _unlockedDistraction)
        {
            _unlockedDistraction = true;
            _storage.CanSeeDistraction = true;
            UnlockedDistraction?.Invoke();
        }
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _optionOne.UpdateInteraction(gameTime, toCheck);
        _optionTwo.UpdateInteraction(gameTime, toCheck);
        _optionThree.UpdateInteraction(gameTime, toCheck);
        _optionFour.UpdateInteraction(gameTime, toCheck);

        if (_optionOne.IsHoverOnButton())
            _infoDiplay.ChangeText(_infoDisplayText[0]);
        else if (_optionTwo.IsHoverOnButton())
            _infoDiplay.ChangeText(_infoDisplayText[1]);
        else if (_optionThree.IsHoverOnButton())
            _infoDiplay.ChangeText(_infoDisplayText[2]);
        else if (_optionFour.IsHoverOnButton())
            _infoDiplay.ChangeText(_infoDisplayText[3]);
        else
            _infoDiplay.ChangeText(string.Empty);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _optionOne.Draw(spriteBatch);
        _optionTwo.Draw(spriteBatch);
        _optionThree.Draw(spriteBatch);
        _optionFour.Draw(spriteBatch);
        _beanDisplay.Draw(spriteBatch);
        _infoDiplay.Draw(spriteBatch);
    }

    private OverTimeInvoker GetNewInvoker(bool start = true)
    {
        var invoker = new OverTimeInvoker(1000D, start);
        invoker.Trigger += IncreaseBeanCount;
        return invoker;
    }

    private void OptionOnePurchased(int count, int value)
    {
        _autoClicker.Add(GetNewInvoker());
        _storage.Upgrade1 = count;
    }

    private void OptionTwoPurchased(int count, int value)
        => _storage.Upgrade2 = count;

    private void OptionThreePurchased(int count, int value)
        => _storage.Upgrade3 = count;

    private void OptionFourPurchased(int count, int value)
    {
        _storage.Upgrade4 = count;
        SetSuspiciousIcon();
        UpdateIcon();
    }

    private void SetSuspiciousIcon()
        => _icon = "[amongusbean]";

    private void SetIcon()
        => _icon = "[bean]";

    private void UpdateIcon()
    {
        _optionOne.ChangeCurrencyIcon(_icon);
        _optionTwo.ChangeCurrencyIcon(_icon);
        _optionThree.ChangeCurrencyIcon(_icon);
        _optionFour.ChangeCurrencyIcon(_icon);
    }
}