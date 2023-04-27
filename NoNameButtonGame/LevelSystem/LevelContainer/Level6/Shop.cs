using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.Texture;
using NoNameButtonGame.Interfaces;
using NoNameButtonGame.LogicObjects;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class Shop : GameObject, IInteractable
{
    public new static Vector2 DefaultSize => Display.Display.Size / 2;

    private StorageData _storage;
    private readonly Random _random;
    private Rectangle _rectangle;

    private readonly ShopOption _optionOne;
    private readonly ShopOption _optionTwo;
    private readonly ShopOption _optionThree;
    private readonly ShopOption _optionFour;

    private readonly List<OverTimeInvoker> _autoClicker;
    private int _notStarted;

    public long BeanCount { get; private set; }

    public event Action UnlockedShop;
    private bool _unlockedShop;

    public event Action UnlockedDistraction;
    private bool _unlockedDistraction;

    public Shop(Vector2 position, Vector2 size, StorageData storage, Random random) : base(position, DefaultSize,
        DefaultTexture,
        DefaultMapping)
    {
        _storage = storage;
        _random = random;
        _rectangle = new Rectangle(position.ToPoint(), size.ToPoint());
        _autoClicker = new List<OverTimeInvoker>();

        BeanCount = storage.Beans;
        var yOffset = DefaultSize / 2;

        if (storage.Upgrade1 > 0 || storage.Upgrade2 > 0 || storage.Upgrade3 > 0 || storage.Upgrade4 > 0)
            _unlockedShop = true;
        

        for (int i = 0; i < storage.Upgrade1; i++)
            _autoClicker.Add(GetNewInvoker(false));
        _notStarted = storage.Upgrade1;

        _optionOne = new ShopOption(size.Y, "Auto Beans", storage.Upgrade1, 50, 1.07D, 100);
        _optionOne.GetCalculator(_rectangle).OnX(0.20F).BySizeX(-0.5F).Move();
        _optionOne.Purchased += DecreaseBeanCount;
        _optionOne.Purchased += OptionOnePurchased;

        _optionTwo = new ShopOption(size.Y, "Jelly Beans", storage.Upgrade2, 150, 1.32D, 20);
        _optionTwo.GetCalculator(_rectangle).OnX(0.40F).BySizeX(-0.5F).Move();
        _optionTwo.Purchased += DecreaseBeanCount;
        _optionTwo.Purchased += OptionTwoPurchased;

        _optionThree = new ShopOption(size.Y, "Magic Beans", storage.Upgrade3, 3000, 1.46D, 10);
        _optionThree.GetCalculator(_rectangle).OnX(0.60F).BySizeX(-0.5F).Move();
        _optionThree.Purchased += DecreaseBeanCount;
        _optionThree.Purchased += OptionThreePurchased;

        _optionFour = new ShopOption(size.Y, "Suspicious Beans", storage.Upgrade4, 1000000, 99D, 1);
        _optionFour.GetCalculator(_rectangle).OnX(0.80F).BySizeX(-0.5F).Move();
        _optionFour.Purchased += DecreaseBeanCount;
        _optionFour.Purchased += OptionFourPurchased;
    }

    public void IncreaseBeanCount()
    {
        // ToDo: Increase based on purchased items
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
        _optionOne.GetCalculator(_rectangle).OnX(0.20F).BySizeX(-0.5F).Move();
        _optionTwo.GetCalculator(_rectangle).OnX(0.40F).BySizeX(-0.5F).Move();
        _optionThree.GetCalculator(_rectangle).OnX(0.60F).BySizeX(-0.5F).Move();
        _optionFour.GetCalculator(_rectangle).OnX(0.80F).BySizeX(-0.5F).Move();

        _optionOne.Update(gameTime, BeanCount);
        _optionTwo.Update(gameTime, BeanCount);
        _optionThree.Update(gameTime, BeanCount);
        _optionFour.Update(gameTime, BeanCount);

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
            _storage.Snake = true;
            UnlockedDistraction?.Invoke();
        }
    }

    public void UpdateInteraction(GameTime gameTime, IHitbox toCheck)
    {
        _optionOne.UpdateInteraction(gameTime, toCheck);
        _optionTwo.UpdateInteraction(gameTime, toCheck);
        _optionThree.UpdateInteraction(gameTime, toCheck);
        _optionFour.UpdateInteraction(gameTime, toCheck);
    }

    public override void Draw(SpriteBatch spriteBatch)
    {
        _optionOne.Draw(spriteBatch);
        _optionTwo.Draw(spriteBatch);
        _optionThree.Draw(spriteBatch);
        _optionFour.Draw(spriteBatch);
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
        => _storage.Upgrade4 = count;
}