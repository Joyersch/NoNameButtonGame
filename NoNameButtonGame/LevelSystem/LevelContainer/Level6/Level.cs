using System;
using System.Net.Security;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using NoNameButtonGame.Extensions;
using NoNameButtonGame.GameObjects;
using NoNameButtonGame.GameObjects.AddOn;
using NoNameButtonGame.GameObjects.Buttons;
using NoNameButtonGame.GameObjects.Debug;
using NoNameButtonGame.GameObjects.TextSystem;
using NoNameButtonGame.LogicObjects;
using NoNameButtonGame.LogicObjects.Listener;

namespace NoNameButtonGame.LevelSystem.LevelContainer.Level6;

public class Level : SampleLevel
{
    private Storage.Storage _storage;

    private readonly Text _counter;

    private readonly LockButtonAddon _shopButtonLock;

    private readonly OverTimeMover _overTimeMover;

    private readonly Text shopOne;
    private readonly Text shopTwo;
    private readonly Text shopThree;
    private readonly Text shopFour;

    private readonly Text shopOnePrice;
    private readonly Text shopTwoPrice;
    private readonly Text shopThreePrice;
    private readonly Text shopFourPrice;

    private readonly LockButtonAddon _shopLockOne;
    private readonly LockButtonAddon _shopLockTwo;
    private readonly LockButtonAddon _shopLockThree;
    private readonly LockButtonAddon _shopLockFour;

    private int priceOne = 100;
    private int priceTwo = 10000;
    private int priceThree = 100000;
    private int priceFour = int.MaxValue / 2;

    private readonly float priceOneMultiplier = 1.13F;
    private readonly float priceTwoMultiplier = 1.76F;
    private readonly float priceThreeMultiplier = 13.21F;

    private int _bakedBeansCounter;
    private int _bakedBeansIncrement = 1;
    private int _bakedBeansCriticalIncrementChance;
    private int _bakedBeansRelativeIncrement;

    private BeanState _state;

    private enum BeanState : ushort
    {
        Started = 0,
        Reached100 = 1,
        Reached1k = 2,
        Reached5k = 3,
        Reached10k = 4,
        Reached25k = 5,
        Reached100k = 6,
        Reached250k = 7,
        Reached1m = 8
    }

    public Level(Display.Display display, Vector2 window, Random random, Storage.Storage storage) : base(display,
        window, random)
    {
        Name = "Level 6 - Just like Cookie Clicker but with BEANS!";

        _state = BeanState.Started;

        _storage = storage;
        _bakedBeansCounter = (int)storage.GameData.Level6.Beans;

        var shopScreen = new Vector2(640, 0);

        var clickerButton = new TextButton("Bake a bean");
        clickerButton.Move(-clickerButton.Size / 2);
        clickerButton.Click += IncreaseBeanCounter;
        clickerButton.Click += UpdateState;
        clickerButton.Click += WriteBeanCounterToStorage;
        AutoManaged.Add(clickerButton);


        var moveToClickerButton = new TextButton("Return", "return", 0.75F);
        moveToClickerButton.Move(new Vector2(320, 180) - new Vector2(0, moveToClickerButton.Size.Y));
        moveToClickerButton.Click += MoveCamera;
        AutoManaged.Add(moveToClickerButton);

        var moveToShopButton = new TextButton("Shop", "shop", 0.75F);
        moveToShopButton.Move(moveToClickerButton.Position - new Vector2(moveToShopButton.Size.X, 0));

        _shopButtonLock = new LockButtonAddon(moveToShopButton);
        _shopButtonLock.Callback += MoveCamera;
        AutoManaged.Add(_shopButtonLock);

        shopOne = new Text(string.Empty);
        shopOne.Move(shopScreen + new Vector2(-192, -64));
        AutoManaged.Add(shopOne);

        shopTwo = new Text(string.Empty);
        shopTwo.Move(shopScreen + new Vector2(-64, -64));
        AutoManaged.Add(shopTwo);

        shopThree = new Text(string.Empty);
        shopThree.Move(shopScreen + new Vector2(64, -64));
        AutoManaged.Add(shopThree);

        shopFour = new Text("0");
        shopFour.Move(shopScreen + new Vector2(192, -64));
        AutoManaged.Add(shopFour);

        var shopOptionOne = new TextButton("Canned Beans", 0.75F, 0.5F);
        shopOptionOne.Move(shopOne.Position + shopOne.Size / 2 - shopOptionOne.Size / 2 + new Vector2(0, 48));

        _shopLockOne = new LockButtonAddon(shopOptionOne);
        _shopLockOne.Callback += ShopOptionOneClick;
        AutoManaged.Add(_shopLockOne);

        var shopOptionTwo = new TextButton("Jellybeans", 0.75F, 0.5F);
        shopOptionTwo.Move(shopTwo.Position + shopTwo.Size / 2 - shopOptionTwo.Size / 2 + new Vector2(0, 48));

        _shopLockTwo = new LockButtonAddon(shopOptionTwo);
        _shopLockTwo.Callback += ShopOptionTwoClick;
        AutoManaged.Add(_shopLockTwo);

        var shopOptionThree = new TextButton("Genetically modified", 0.75F, 0.5F);
        shopOptionThree.Move(shopThree.Position + shopThree.Size / 2 - shopOptionThree.Size / 2 + new Vector2(0, 48));

        _shopLockThree = new LockButtonAddon(shopOptionThree);
        _shopLockThree.Callback += ShopOptionThreeClick;
        AutoManaged.Add(_shopLockThree);

        var shopOptionFour = new TextButton("Magic Beans", 0.75F, 0.5F);
        shopOptionFour.Move(shopFour.Position + shopFour.Size / 2 - shopOptionFour.Size / 2 + new Vector2(0, 48));

        _shopLockFour = new LockButtonAddon(shopOptionFour);
        _shopLockFour.Callback += Finish;
        AutoManaged.Add(_shopLockFour);

        shopOnePrice = new Text(priceOne.ToString());
        shopOnePrice.Move(shopOptionOne.Rectangle.Center.ToVector2() + new Vector2(0, 64) -
                          new Vector2(shopOnePrice.Rectangle.Size.X / 2, 0));
        AutoManaged.Add(shopOnePrice);

        shopTwoPrice = new Text(priceTwo.ToString());
        shopTwoPrice.Move(shopOptionTwo.Rectangle.Center.ToVector2() + new Vector2(0, 64) -
                          new Vector2(shopTwoPrice.Rectangle.Size.X / 2, 0));
        AutoManaged.Add(shopTwoPrice);

        shopThreePrice = new Text(priceThree.ToString());
        shopThreePrice.Move(shopOptionThree.Rectangle.Center.ToVector2() + new Vector2(0, 64) -
                            new Vector2(shopThreePrice.Rectangle.Size.X / 2, 0));
        AutoManaged.Add(shopThreePrice);

        shopFourPrice = new Text(priceFour.ToString());
        shopFourPrice.Move(shopOptionFour.Rectangle.Center.ToVector2() + new Vector2(0, 64) -
                           new Vector2(shopFourPrice.Rectangle.Size.X / 2, 0));
        AutoManaged.Add(shopFourPrice);

        _overTimeMover = new OverTimeMover(Camera, new Vector2(640, 0), 500F, OverTimeMover.MoveMode.Sin);
        AutoManaged.Add(_overTimeMover);

        _bakedBeansCounter = (int)_storage.GameData.Level6.Beans;
        _counter = new Text(string.Empty);
        AutoManaged.Add(_counter);

        UpdateState();

        var cursor = new Cursor();
        Actuator = cursor;
        PositionListener.Add(_mouse, cursor);
        AutoManaged.Add(cursor);
    }

    private void ShopOptionOneClick(object obj)
    {
        _bakedBeansCounter -= priceOne;
        _bakedBeansIncrement++;
        priceOne = (int) (priceOne * priceOneMultiplier);
        shopOnePrice.ChangeText(priceOne.ToString());
        _shopLockOne.Lock();
    }

    private void ShopOptionTwoClick(object obj)
    {
        _bakedBeansCounter -= priceTwo;
        _bakedBeansCriticalIncrementChance++;
        priceTwo = (int) (priceTwo * priceOneMultiplier);
        shopTwoPrice.ChangeText(priceTwo.ToString());
        _shopLockTwo.Lock();
    }

    private void ShopOptionThreeClick(object obj)
    {
        _bakedBeansCounter -= priceThree;
        _bakedBeansRelativeIncrement++;
        priceThree = (int) (priceThree * priceOneMultiplier);
        shopThreePrice.ChangeText(priceThree.ToString());
        _shopLockThree.Lock();
    }

    public override void Update(GameTime gameTime)
    {
        shopOne.ChangeText(_bakedBeansIncrement.ToString());
        if (_bakedBeansCriticalIncrementChance > 0)
            shopTwo.ChangeText(_bakedBeansCriticalIncrementChance.ToString());
        if (_bakedBeansRelativeIncrement > 0)
            shopThree.ChangeText(_bakedBeansRelativeIncrement.ToString());

        if (_bakedBeansCounter >= priceOne)
            _shopLockOne.Unlock();

        if (_bakedBeansCounter >= priceTwo)
            _shopLockTwo.Unlock();

        if (_bakedBeansCounter >= priceThree)
            _shopLockThree.Unlock();

        if (_bakedBeansCounter >= priceFour)
            _shopLockFour.Unlock();
        base.Update(gameTime);

        if (_state >= BeanState.Reached100)
            _shopButtonLock.Unlock();

        if (_bakedBeansCounter > 0)
            _counter.ChangeText(_bakedBeansCounter.ToString() + Letter.ReverseParse(Letter.Character.AmongUsBean));
        _counter.Move(-_counter.Rectangle.Size.ToVector2() / 2 + new Vector2(0, -64));
        _counter.Update(gameTime);

        Console.WriteLine(Camera.Rectangle);
    }

    private void MoveCamera(object obj)
    {
        _overTimeMover.Start();
        _overTimeMover.ChangeDestination(((TextButton) obj).Name == "return" ? Vector2.Zero : new Vector2(640, 0));
    }

    private void WriteBeanCounterToStorage(object obj)
    {
        _storage.GameData.Level6.Beans = (long)_bakedBeansCounter;
        _storage.Save();
    }

    private void IncreaseBeanCounter(object obj)
    {
        int increment = _bakedBeansIncrement;
        increment += _bakedBeansCounter / 1000 * _bakedBeansRelativeIncrement;
        var crit = Random.Next(0, 100);
        if (crit <= _bakedBeansCriticalIncrementChance)
            increment *= 10;

        _bakedBeansCounter += increment;
    }

    private void UpdateState(object obj)
        => UpdateState();

    private void UpdateState()
    {
        while (ShouldStateIncrease())
            _state++;
    }

    private bool ShouldStateIncrease()
    {
        switch (_state)
        {
            case BeanState.Started:
                if (_bakedBeansCounter >= 100)
                    return true;
                break;
            case BeanState.Reached100:
                if (_bakedBeansCounter >= 1000)
                    return true;
                break;
            case BeanState.Reached1k:
                if (_bakedBeansCounter >= 5000)
                    return true;
                break;
            case BeanState.Reached5k:
                if (_bakedBeansCounter >= 10000)
                    return true;
                break;
            case BeanState.Reached10k:
                if (_bakedBeansCounter >= 25000)
                    return true;
                break;
            case BeanState.Reached25k:
                if (_bakedBeansCounter >= 100000)
                    return true;
                break;
            case BeanState.Reached100k:
                if (_bakedBeansCounter >= 250000)
                    return true;
                break;
            case BeanState.Reached250k:
                if (_bakedBeansCounter >= 1000000)
                    return true;
                break;
        }

        return false;
    }
}